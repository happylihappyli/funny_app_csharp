Imports System.IO
Imports B_TreapVB.TreapVB
Imports B_Core.Funny

Namespace com.Funny.Segmentation

    Public Class C_Segmentation_Struct

        Public pTreapKeys As New Treap

        Public Delegate Sub ReadLine_CallBack( _
                ByVal strLine As String, _
                ByVal strType As String)

        Public Function InitDictionary( _
                ByRef pCallBack As ReadLine_CallBack, _
                ByVal strFile As String) As String
            '把单词读取到内存，这样就可以分词了
            '从Txt文件中读取内容
            If File.Exists(strFile) = True Then
                Dim pFS As FileStream = New FileStream( _
                    strFile, FileMode.OpenOrCreate, FileAccess.Read)
                Dim pSR As StreamReader = New StreamReader( _
                    pFS, System.Text.Encoding.UTF8)

                Dim Count As Int32 = 0
                Dim strLine As String

                pSR.BaseStream.Seek(0, SeekOrigin.Begin)
                strLine = pSR.ReadLine
                Do While strLine IsNot Nothing
                    strLine = Trim(strLine)
                    If strLine.IndexOf(" ") Then
                        readLine_fromDic(pCallBack, strLine)
                        Count += 1
                    End If
                    strLine = pSR.ReadLine
                Loop
                Debug.Print("ListCount:" & pTreapKeys.Size & ",WordCount:" & Count)

                pSR.Close() : pFS.Close()
                pSR = Nothing : pFS = Nothing
                Return "词库初始化完毕！"
            Else
                Return "词库文件 \Data\Word.Txt 不存在"
            End If
        End Function

        Public Sub readLine_fromDic( _
                    ByRef pCallBack As ReadLine_CallBack, _
                    ByVal strLine As String)
            strLine = strLine.Trim()

            If strLine.StartsWith("'") Then Exit Sub
            If strLine = "" Then Exit Sub


            Dim pos As Int32
            Dim strConvert As String = ""
            pos = strLine.IndexOf("==")
            If pos > 0 Then
                strConvert = strLine.Substring(pos + 2)
                strLine = strLine.Substring(0, pos)


                strConvert = strLine.Replace(" ", "&") + "|" + strConvert
                pCallBack(strConvert, "convert")
            End If

            pos = strLine.IndexOf(" ")
            If (pos > 0) Then
                pCallBack(strLine, "")

                Dim strSplit() As String = strLine.Split(" ")

                If strSplit.Length = 2 OrElse strSplit.Length = 3 Then
                    Dim pStruct As C_Seg_Struct
                    pStruct = pTreapKeys.find(New C_K_Str(strLine))

                    If pStruct Is Nothing Then
                        pStruct = New C_Seg_Struct
                        pStruct.strLine = strLine
                        pTreapKeys.insert(New C_K_Str(strLine), pStruct)
                        If strSplit.Length = 2 Then
                            GetSubTree("1." & strSplit(0)).insert(New C_K_Str(pStruct.strLine), pStruct)
                            GetSubTree("2." & strSplit(1)).insert(New C_K_Str(pStruct.strLine), pStruct)
                            pStruct.iMax = 2
                        ElseIf strSplit.Length = 3 Then
                            GetSubTree("1." & strSplit(0)).insert(New C_K_Str(pStruct.strLine), pStruct)
                            GetSubTree("2." & strSplit(1)).insert(New C_K_Str(pStruct.strLine), pStruct)
                            GetSubTree("3." & strSplit(2)).insert(New C_K_Str(pStruct.strLine), pStruct)
                            pStruct.iMax = 3
                        End If
                    End If
                End If
            End If
        End Sub

        Private Function GetSubTree(ByVal strKey As String) As Treap
            Dim pSubTreap As Treap = pTreapKeys.find(New C_K_Str(strKey))

            If pSubTreap Is Nothing Then
                pSubTreap = New Treap
                pTreapKeys.insert(New C_K_Str(strKey), pSubTreap)
            End If

            Return pSubTreap
        End Function

        Public Function ifHasThisWord(ByVal strLine As String) As Boolean
            Dim strSplit() As String = strLine.Split(" ")
            Dim pTreap As Treap = pTreapKeys.find(New C_K_Str(strLine))
            If Not pTreap Is Nothing Then
                Return True
            End If
            Return False
        End Function

        Public Function Segmentation_CompareWord( _
            ByRef pLink As C_Token_Link, _
            ByVal index As Int32, _
            ByVal iCount As Int32) As C_Token

            '标记一下，代表已经计算过
            If pLink Is Nothing OrElse pLink.Count < index + 2 Then
                Return Nothing
            End If

            If pLink.Item(index + 0).pNextToken IsNot Nothing AndAlso _
                pLink.Item(index + 0).pNextToken.ID = pLink.Item(index + 1).ID Then
                Return Nothing
            End If


            Dim pTreapCache As Treap = New Treap

            Dim pToken(iCount) As C_Token
            Dim MaxI As Int32 = pLink.Count
            Dim pStruct As C_Seg_Struct
            Dim iStart As Int32
            Dim iEnd As Int32
            Dim strTmp As String
            Dim p As TreapEnumerator

            Dim iMax As Int32 = Math.Min(iCount, MaxI - index) - 1
            For j As Int32 = 0 To iMax
                pToken(j) = pLink.Item(index + j)
                If j = 0 Then
                    iStart = pToken(j).iStart
                End If
                If j = iMax Then
                    iEnd = pToken(j).iEnd
                End If

                Dim pSubTree As Treap
                For k As Int32 = 0 To pToken(j).Count - 1
                    strTmp = (j + 1) & "." & pToken(j).Item(k)
                    'Debug.Print(strTmp)
                    pSubTree = Me.GetSubTree(strTmp)

                    If pSubTree IsNot Nothing Then
                        p = pSubTree.Elements
                        Do While p.HasMoreElements
                            pStruct = p.NextElement()
                            pStruct.iCount += 1 '通过计数的方法统计
                            If pStruct.strLine.Split(" ").Length = iMax AndAlso _
                                j < iMax AndAlso _
                                pStruct.strLine.Split(" ")(j) = pToken(j).Item(k) Then

                                'If pStruct.pFrom(k) IsNot Nothing Then
                                '    Stop
                                'End If
                                pStruct.pFrom(j) = pToken(j)
                                'pToken(j).
                                'If pStruct.strLine.StartsWith("windows {INT}") AndAlso pStruct.iCount > 1 Then
                                '    Stop
                                'End If
                                pTreapCache.insert(New C_K_Str(pStruct.strLine), pStruct)
                            End If
                        Loop
                    End If
                Next
            Next

            '标记一下，代表已经计算过
            pLink.Item(index + 0).pNextToken = pLink.Item(index + 1)

            'Dim strSplit() As String
            Dim strReturn As String
            Dim bCheck As Boolean
            strReturn = ""
            p = pTreapCache.Elements
            Do While p.HasMoreElements
                pStruct = p.NextElement
                If pStruct.iCount = 3 AndAlso pStruct.iMax = 3 Then
                    bCheck = True
                    For i As Int32 = 0 To 2
                        If pStruct.pFrom(i) Is Nothing Then
                            bCheck = False
                        End If
                    Next
                    If bCheck Then
                        strReturn &= Replace(pStruct.strLine, " ", "&") & "|"
                    End If
                End If
            Loop

            If strReturn <> "" Then
                p = pTreapCache.Elements
                Do While p.HasMoreElements
                    pStruct = p.NextElement
                    pStruct.iCount = 0
                Loop
                Return New C_Token(strReturn, iStart, iEnd)
            End If



            strReturn = ""
            p = pTreapCache.Elements
            Do While p.HasMoreElements
                pStruct = p.NextElement
                If pStruct.iCount = 2 AndAlso pStruct.iMax = 2 Then
                    bCheck = True
                    For i As Int32 = 0 To 1
                        If pStruct.pFrom(i) Is Nothing Then
                            bCheck = False
                        End If
                    Next
                    If bCheck Then
                        strReturn &= Replace(pStruct.strLine, " ", "&") & "|"
                    End If
                    'strReturn &= Replace(pStruct.strLine, " ", "&") & "|"
                End If
            Loop

            If strReturn <> "" Then
                p = pTreapCache.Elements
                Do While p.HasMoreElements
                    pStruct = p.NextElement
                    pStruct.iCount = 0
                Loop
                Return New C_Token(strReturn, iStart, iEnd)
            End If

            p = pTreapCache.Elements
            Do While p.HasMoreElements
                pStruct = p.NextElement
                pStruct.iCount = 0
            Loop

            Return Nothing
        End Function

        Public Sub AddString( _
                    ByRef pArray As ArrayList, _
                    ByRef pToken As C_Token, _
                    ByVal bReplace As Boolean)

            Dim strTmp As String = ""
            Dim Count As Int32
            If pArray.Count > 0 Then
                Count = pArray.Count
                For m As Int32 = 0 To pToken.Count - 2
                    For k As Int32 = 0 To Count - 1
                        pArray.Add(pArray.Item(k))
                    Next
                Next

                Count = pArray.Count
                For k As Int32 = 0 To pArray.Count - 1
                    strTmp = S_Strings.ReplaceNLP_Invert(pToken.Item(k \ (pArray.Count / pToken.Count)))
                    If bReplace Then
                        strTmp = ReplaceX(strTmp)
                    End If
                    pArray.Item(k) = pArray.Item(k) + " " + strTmp
                Next
            Else
                For m As Int32 = 0 To pToken.Count - 1
                    strTmp = S_Strings.ReplaceNLP_Invert(pToken.Item(m))
                    If bReplace Then
                        strTmp = ReplaceX(strTmp)
                    End If
                    pArray.Add(strTmp)
                Next
            End If
        End Sub

        Public Function getSubString( _
                        ByRef pToken_Link As C_Token_Link, _
                        ByRef pToken As C_Token, _
                        ByVal i As Int32, ByVal MaxI As Int32, _
                        ByVal bReplaceX As Boolean) As ArrayList

            Dim j As Int32 = 0
            Dim strTmp As String = ""
            Dim pArray As ArrayList = New ArrayList

            AddString(pArray, pToken, bReplaceX)
            For j = i + 1 To MaxI - 1
                AddString(pArray, pToken_Link.Item(j), bReplaceX)
            Next
            Return pArray
        End Function


        Public Function ReplaceX(ByVal strTmp2 As String) As String
            If strTmp2 IsNot Nothing AndAlso _
                strTmp2.StartsWith("{") AndAlso _
                strTmp2.EndsWith(".类}") Then

                strTmp2 = "{x.类}"
            End If
            Return strTmp2
        End Function


        Public Function Segmentation_List( _
            ByRef pToken_Link As C_Token_Link, _
            ByVal iCount As Int32) As C_Token_Link

            Dim pLinkR As C_Token_Link = New C_Token_Link()
            Dim bList As Boolean = True

            Dim pToken, pToken_Old As C_Token
            Dim Count As Int32 = 0

            'Dim bMatch As Boolean = False

            For i As Int32 = 0 To pToken_Link.Count - 1
                pToken = Nothing
                pToken = Segmentation_CompareWord(pToken_Link, i, iCount)
                If pToken IsNot Nothing Then
                    pLinkR.Add(pToken)
                    For k As Int32 = 0 To pToken.Item(0).Split("&").Length() - 1

                        pToken_Old = pToken_Link.Item(Count)

                        If pToken_Old Is Nothing Then
                            '调试
                            Stop
                            pToken = Segmentation_CompareWord(pToken_Link, i, iCount)
                        End If
                        pToken.pChild.Add(pToken_Old)
                        Count += 1
                    Next

                    pToken.iStart = pToken.pChild(0).iStart
                    pToken.iEnd = pToken.pChild(pToken.pChild.Count - 1).iEnd
                    i = Count - 1
                Else
                    pToken_Old = pToken_Link.Item(i)
                    pLinkR.Add(pToken_Old)

                    Count += 1
                End If
            Next

            If pLinkR.Count = pToken_Link.Count Then
                Return pToken_Link
            Else
                pLinkR.iStart = pLinkR.Item(0).iStart
                pLinkR.iEnd = pLinkR.Item(pLinkR.Count() - 1).iEnd

                Return pLinkR
            End If
        End Function

    End Class
End Namespace
