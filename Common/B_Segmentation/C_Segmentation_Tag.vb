Imports B_Data.Funny
Imports B_String.Funny

Imports System.IO
Imports System.Data.SQLite
Imports B_Token.Funny
Imports CommonTreapVB.TreapVB


Namespace com.Funny.Segmentation

    Public Class C_Segmentation_Tag

        Public pTreapKeys As New Struct_Treap '(Of Treap(Of C_Seg_Struct))

        Public Delegate Sub ReadLine_CallBack( _
                ByVal strLine As String, _
                ByVal strType As String)


        Public Sub InitSQLite( _
                ByRef pConvert As C_Convert, _
                ByVal strFile As String)
            Dim SQLconnect As New SQLite.SQLiteConnection()
            SQLconnect.ConnectionString = "Data Source=" & strFile ' Application.StartupPath & "\AI.sqlite;"
            SQLconnect.Open()
            Dim SQLcommand As SQLiteCommand = SQLconnect.CreateCommand

            Dim Query As New SQLiteCommand("SELECT SFrom,STo,iNext FROM Struct ", SQLconnect)
            Dim Reader As SQLiteDataReader
            Reader = Query.ExecuteReader
            Do While Reader.Read
                If IsDBNull(Reader("SFrom")) = False Then
                    readLine_fromDic(pConvert, Reader("SFrom"), Reader("STo"), IIf(IsDBNull(Reader("iNext")), 0, Reader("iNext")))
                End If
            Loop

            SQLcommand.Dispose()
            SQLconnect.Close()
        End Sub

        Public Function InitDictionary( _
                ByRef pConvert As C_Convert, _
                ByVal strFile As String) As String
            '把单词读取到内存，这样就可以分词了
            '从Txt文件中读取内容
            If File.Exists(strFile) = True Then
                Dim pFS As FileStream = New FileStream( _
                    strFile, FileMode.OpenOrCreate, FileAccess.Read)
                Dim pSR As StreamReader = New StreamReader( _
                    pFS, System.Text.Encoding.UTF8)

                Dim Count As Integer = 0
                Dim strLine As String
                Dim strSplit() As String
                Dim strConvert As String = ""
                Dim iNext As Integer = 0

                pSR.BaseStream.Seek(0, SeekOrigin.Begin)
                strLine = pSR.ReadLine
                Do While strLine IsNot Nothing
                    strLine = Trim(strLine)
                    If strLine.IndexOf(" ") Then
                        strSplit = Split(strLine, "==")
                        strLine = strSplit(0)
                        If strSplit.Length > 1 Then
                            strConvert = strSplit(1)
                        Else
                            strConvert = ""
                        End If
                        If strSplit.Length > 2 Then
                            iNext = Val(strSplit(2))
                        End If
                        If strConvert <> "" Then
                            readLine_fromDic(pConvert, strLine, strConvert, iNext)
                            Count += 1
                        End If
                    End If
                    strLine = pSR.ReadLine
                Loop
                Debug.Print("ListCount:" & pTreapKeys.Size & ",WordCount:" & Count)

                pSR.Close() : pFS.Close()
                pSR = Nothing : pFS = Nothing
                Return "初始化：" & strFile
            Else
                Return "词库文件 \Data\Word.Txt 不存在"
            End If
        End Function

        Public Sub readLine_fromDic2( _
                    ByRef pConvert As C_Convert, _
                    ByVal strLine As String, _
                    ByVal iNext As Integer)
            Dim pos As Integer
            Dim strConvert As String = ""
            pos = strLine.IndexOf("==")
            If strConvert <> "" Then
                strConvert = strLine.Substring(pos + 2)
                strLine = strLine.Substring(0, pos)
            End If
            readLine_fromDic(pConvert, strLine, strConvert, iNext)
        End Sub

        Public Sub readLine_fromDic( _
                    ByRef pConvert As C_Convert, _
                    ByVal strLine As String, _
                    ByVal strConvert As String, _
                    ByVal iNext As Integer)
            strLine = strLine.Trim()

            If strLine.StartsWith("'") Then Exit Sub
            If strLine = "" Then Exit Sub

            Dim strTmp As String = ""
            Dim strSplit() As String

            If strConvert <> "" Then
                strSplit = strLine.Split(" ")
                pConvert.read_fromDic(strLine, strConvert, "")
            End If

            Dim pos As Integer = strLine.IndexOf(" ")
            If (pos > 0) Then
                strSplit = strLine.Split(" ")

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
                            'pStruct.iMax = 2
                        ElseIf strSplit.Length = 3 Then
                            GetSubTree("1." & strSplit(0)).insert(New C_K_Str(pStruct.strLine), pStruct)
                            GetSubTree("2." & strSplit(1)).insert(New C_K_Str(pStruct.strLine), pStruct)
                            GetSubTree("3." & strSplit(2)).insert(New C_K_Str(pStruct.strLine), pStruct)
                            'pStruct.iMax = 3
                        End If
                    End If
                    pStruct.iNext = iNext
                    pStruct.Size = strSplit.Length
                End If
            End If
        End Sub

        Private Function GetSubTree(ByVal strKey As String) As Treap(Of C_Seg_Struct)
            Dim pSubTreap As Treap(Of C_Seg_Struct) = pTreapKeys.pTreapTreap.find(New C_K_Str(strKey))

            If pSubTreap Is Nothing Then
                pSubTreap = New Treap(Of C_Seg_Struct)
                pTreapKeys.pTreapTreap.insert(New C_K_Str(strKey), pSubTreap)
            End If

            Return pSubTreap
        End Function

        'Public Function ifHasThisWord(ByVal strLine As String) As Boolean
        '    Dim strSplit() As String = strLine.Split(" ")
        '    Dim pTreap As Treap(Of C_Seg_Struct) = pTreapKeys.find(New C_K_Str(strLine))
        '    If Not pTreap Is Nothing Then
        '        Return True
        '    End If
        '    Return False
        'End Function



        Public Function Segmentation_CompareWord2( _
            ByRef pLink As C_Token_Link, _
            ByVal index As Integer, _
            ByVal iCount As Integer) As C_Token

            '标记一下，代表已经计算过
            If pLink Is Nothing OrElse pLink.Count < index + 2 Then
                Return Nothing
            End If

            If pLink.Item(index + 0).pNextToken2 IsNot Nothing AndAlso _
                pLink.Item(index + 0).pNextToken2.ID = pLink.Item(index + 1).ID Then
                Return Nothing
            End If

            Dim pTreapCache As New Treap(Of C_Seg_Struct)

            Dim pToken_Array(iCount) As C_Token
            Dim MaxI As Integer = pLink.Count
            Dim pStruct As C_Seg_Struct = Nothing
            Dim iStart As Integer
            Dim iEnd As Integer
            Dim strTmp As String
            Dim p As TreapEnumerator

            Dim iMax As Integer = Math.Min(iCount, MaxI - index) - 1
            For j As Integer = 0 To iMax
                pToken_Array(j) = pLink.Item(index + j)
                If j = 0 Then
                    iStart = pToken_Array(j).iStart
                End If
                If j = iMax Then
                    iEnd = pToken_Array(j).iEnd
                End If

                Dim pSubTree As Treap(Of C_Seg_Struct)
                For k As Integer = 0 To pToken_Array(j).Count - 1
                    strTmp = (j + 1) & "." & pToken_Array(j).Item(k)
                    pSubTree = Me.GetSubTree(strTmp) '要匹配的结构树，一个个比较

                    If pSubTree IsNot Nothing Then
                        p = pSubTree.Elements
                        Do While p.HasMoreElements
                            pStruct = p.NextElement()
                            'pStruct.iCount += 1 '通过计数的方法统计
                            If j < pStruct.strLine.Split(" ").Length AndAlso _
                                pStruct.strLine.Split(" ")(j) = pToken_Array(j).Item(k) Then
                                pStruct.pFrom(j) = pToken_Array(j)
                                pTreapCache.insert(New C_K_Str(pStruct.strLine), pStruct)
                            End If
                        Loop
                    End If
                Next
            Next

            '标记一下，代表已经计算过
            pLink.Item(index + 0).pNextToken2 = pLink.Item(index + 1)

            Dim strReturn As String = ""
            Dim bCheck As Boolean

            p = pTreapCache.Elements
            Do While p.HasMoreElements
                pStruct = p.NextElement
                If index + 1 < pLink.Count AndAlso pStruct.Size = 2 Then 'AndAlso pStruct.iCount = 2 
                    bCheck = True
                    For i As Integer = 0 To 1
                        If pStruct.pFrom(i) Is Nothing Then
                            bCheck = False
                        End If
                    Next
                    If bCheck Then
                        strReturn = pStruct.pFrom(pStruct.iNext - 1).ToString
                        Exit Do
                    End If
                End If
            Loop

            If strReturn <> "" AndAlso pStruct IsNot Nothing Then
                Dim pToken As C_Token = New C_Token(strReturn, iStart, iEnd, 2)
                pToken.pChild.Add(pStruct.pFrom(0))
                pToken.pChild.Add(pStruct.pFrom(1))

                'p = pTreapCache.Elements
                'Do While p.HasMoreElements
                '    pStruct = p.NextElement
                '    'pStruct.iCount = 0
                '    pStruct.pFrom(0) = Nothing
                '    pStruct.pFrom(1) = Nothing
                '    pStruct.pFrom(2) = Nothing
                '    pStruct.pFrom(3) = Nothing
                'Loop
                Return pToken
            End If

            'p = pTreapCache.Elements
            'Do While p.HasMoreElements
            '    pStruct = p.NextElement
            '    pStruct.iCount = 0
            'Loop

            Return Nothing
        End Function


        Public Sub AddString( _
                    ByRef pArray As ArrayList, _
                    ByRef pToken As C_Token, _
                    ByVal bReplace As Boolean)

            Dim strTmp As String = ""
            Dim Count As Integer
            If pArray.Count > 0 Then
                Count = pArray.Count
                For m As Integer = 0 To pToken.Count - 2
                    For k As Integer = 0 To Count - 1
                        pArray.Add(pArray.Item(k))
                    Next
                Next

                Count = pArray.Count
                For k As Integer = 0 To pArray.Count - 1
                    strTmp = S_Strings.ReplaceNLP_Invert(pToken.Item(k \ (pArray.Count / pToken.Count)))
                    If bReplace Then
                        strTmp = ReplaceX(strTmp)
                    End If
                    pArray.Item(k) = pArray.Item(k) + " " + strTmp
                Next
            Else
                For m As Integer = 0 To pToken.Count - 1
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
                        ByVal i As Integer, ByVal MaxI As Integer, _
                        ByVal bReplaceX As Boolean) As ArrayList

            Dim j As Integer = 0
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



        Public Function Segmentation_List2( _
            ByRef pLink As C_Token_Link) As C_Token_Link

            Dim iCount As Integer = 2
            Dim pLinkR As C_Token_Link = New C_Token_Link()
            pLinkR.strSentence = pLink.strSentence
            Dim bList As Boolean = True

            Dim pToken, pToken_Old As C_Token
            'Dim Count As Integer = 0

            For i As Integer = 0 To pLink.Count - 1
                pToken = Nothing
                pToken = Segmentation_CompareWord2(pLink, i, iCount)
                If pToken IsNot Nothing Then
                    pLinkR.Add(pToken)
                    pToken.iStart = pToken.pChild(0).iStart
                    pToken.iEnd = pToken.pChild(pToken.Child_Size - 1).iEnd
                    i = i + pToken.Child_Size - 1
                Else
                    pToken_Old = pLink.Item(i)
                    pLinkR.Add(pToken_Old)
                    'Count += 1
                End If
            Next

            If pLinkR.Count = pLink.Count Then
                Return pLink
            Else
                pLinkR.iStart = pLinkR.Item(0).iStart
                pLinkR.iEnd = pLinkR.Item(pLinkR.Count() - 1).iEnd

                Return pLinkR
            End If
        End Function
    End Class
End Namespace
