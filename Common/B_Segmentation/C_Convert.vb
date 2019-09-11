Imports System.IO
Imports CommonTreapVB.TreapVB
Imports B_Data.Funny
Imports B_String.Funny
Imports B_Token.Funny


Namespace com.Funny.Segmentation

    Public Class C_Convert

        Public pTreapWord As C_Treap_Funny(Of Treap(Of C_Word_Convert)) = New C_Treap_Funny(Of Treap(Of C_Word_Convert))

        ''' <summary>
        ''' 需要激活的结构
        ''' </summary>
        ''' <remarks></remarks>
        Private pTreapActive As New Treap(Of Treap(Of C_Word_Convert))


        ''' <summary>
        ''' 转化
        ''' </summary>
        ''' <param name="pToken_Link"></param>
        Public Function getConvert( _
                ByRef pToken_Link As C_Token_Link) As C_Token_Link_and_Add

            Dim bConvert As Boolean = False '返回值先设置为False
            Dim pLinkR As C_Token_Link = New C_Token_Link()
            pLinkR.strSentence = pToken_Link.strSentence
            Dim pToken_Old As C_Token

            For i As Integer = 0 To pToken_Link.Count - 1
                pToken_Old = pToken_Link.Item(i)

                If pToken_Old.bCheckConvert = False Then '如果没有转化过
                    Dim bAdd As Boolean = False
                    Dim pLinkNew2 As C_Token_Link_and_Add = getConvert_Token(New Treap(Of C_Word_Convert), pToken_Old) '多个token组成的link，因为Convert可能会把一个单词变为多个单词
                    Dim pLinkNew As C_Token_Link = pLinkNew2.pLink

                    'If bAdd Then bConvert = True

                    If pLinkNew2.bAdd Then
                        bConvert = True '如果刚才有添加新Token行为，则表示发生了转化
                        If pLinkNew.Count = 1 Then  '如果转化后仍旧是一个单词
                            Dim pToken As C_Token = pLinkNew.Item(0)
                            If pToken_Old.pChild.Count > 1 Then
                                For j As Integer = 0 To pToken_Old.pChild.Count - 1
                                    pToken.pChild.Add(pToken_Old.pChild.Item(j))
                                Next
                            Else
                                pToken.pChild.Add(pToken_Old)
                            End If

                            pToken.iStart = pToken_Old.iStart
                            pToken.iEnd = pToken_Old.iEnd
                            pLinkR.Add(pToken)
                        ElseIf pLinkNew.Count > 1 Then     '如果转化后为多个单词，比如，抗癌=治疗 癌症
                            For j As Integer = 0 To pLinkNew.Count - 1
                                pLinkR.Add(pLinkNew.Item(j))
                            Next
                        Else                            '0个 错误，一般不会到这
                            pLinkR.Add(pToken_Old)
                        End If
                    Else
                        pLinkR.Add(pToken_Old)
                    End If
                Else
                    pLinkR.Add(pToken_Old)
                End If
            Next

            If bConvert Then    '代表已经替换过
                pLinkR.iStart = pLinkR.Item(0).iStart
                pLinkR.iEnd = pLinkR.Item(pLinkR.Count() - 1).iEnd
            Else                '如果相同的则用原先的
                pLinkR = pToken_Link
            End If

            Dim p As C_Token_Link_and_Add = New C_Token_Link_and_Add
            p.pLink = pLinkR
            p.bAdd = bConvert
            Return p
        End Function


        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="pTreap_Active">激活的结构</param>
        ''' <param name="pToken"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getConvert_Token(
            ByRef pTreap_Active As Treap(Of C_Word_Convert),
            ByRef pToken As C_Token) As C_Token_Link_and_Add

            If pToken.Count = 0 Then Return Nothing

            pToken.bCheckConvert = True

            Dim bAdd As Boolean = False

            Dim strKey As String = pToken.Item(0) '第一个，原始的未转化的词汇

            Dim pTreap_Token As New Treap(Of C_Word_Convert) '看看是否有新的，如果有就存储在这

            Dim pConvert As C_Word_Convert

            If strKey.Split(" ").Length = 1 Then
                For i As Integer = 0 To pToken.Count() - 1
                    strKey = pToken.Item(i)
                    pConvert = New C_Word_Convert(strKey, "", "")
                    pTreap_Token.insert(New C_K_Str(strKey), pConvert)
                Next
            End If

            Dim strOutput As String = ""
            Dim p2 As C_Token_Link = New C_Token_Link()

            For i As Integer = 0 To pToken.Count() - 1
                strKey = pToken.Item(i)

                If IsNumeric(strKey) Then
                    strOutput = "{INT}"
                    If pTreap_Token.find(New C_K_Str(strOutput)) Is Nothing Then '如果是数字，给它放到 {INT} 集合里
                        bAdd = True
                        pConvert = New C_Word_Convert(strKey, strOutput, "")
                        pTreap_Token.insert(New C_K_Str(strOutput), pConvert)
                    End If
                ElseIf Len(strKey) < 5 AndAlso S_Strings.isEnglish(strKey) Then '如果是单词给它放到 {Word.L.x} 集合里
                    strOutput = "Word.L." & Len(strKey)
                    If pTreap_Token.find(New C_K_Str(strOutput)) Is Nothing Then
                        bAdd = True
                        pConvert = New C_Word_Convert(strKey, strOutput, "")
                        pTreap_Token.insert(New C_K_Str(strOutput), pConvert)
                    End If
                End If

                Dim pTreapReplace As Treap(Of C_Word_Convert) = pTreapWord.find(New C_K_Str(strKey)) '查找是否有其他从 可以泛化 的集合
                If pTreapReplace IsNot Nothing Then
                    Dim p As TreapEnumerator
                    p = pTreapReplace.Elements(True)
                    Do While (p.HasMoreElements())
                        pConvert = p.NextElement()
                        If pTreap_Token.find(New C_K_Str(pConvert.OutPutWord)) Is Nothing _
                            AndAlso (pConvert.Active_Word = "" OrElse pTreap_Active.find(New C_K_ID(pConvert.ID)) IsNot Nothing) Then
                            bAdd = True
                            pTreap_Token.insert(New C_K_Str(pConvert.OutPutWord), pConvert)
                        End If
                    Loop
                End If

                If pToken.Count = 1 Then
                    If pTreapReplace Is Nothing Then
                        strOutput = "{any}"
                        If pTreap_Token.find(New C_K_Str(strOutput)) Is Nothing Then
                            bAdd = True
                            pConvert = New C_Word_Convert(strKey, strOutput, "")
                            pTreap_Token.insert(New C_K_Str(strOutput), pConvert)
                        End If
                    End If
                End If
            Next

            Dim strSplit() As String
            Dim pTokenR As New C_Token()
            If pTreap_Token.Size > 0 Then
                Dim p As TreapEnumerator
                p = pTreap_Token.Elements(False)
                Do While (p.HasMoreElements())
                    pConvert = p.NextElement()
                    strSplit = pConvert.OutPutWord.Split(" ")
                    If strSplit.Length > 1 Then
                        For i As Integer = 0 To strSplit.Length - 1
                            p2.Add(New C_Token(strSplit(i), 0, 0, 1)) 'iStart iEnd 外面设置
                        Next
                    Else
                        If pConvert.OutPutWord <> "" Then
                            pTokenR.Add(pConvert.OutPutWord)
                        Else
                            pTokenR.Add(pConvert.Word)
                        End If
                    End If
                Loop
                If p2.Count = 0 Then
                    p2.Add(pTokenR)
                End If
            Else
                pTokenR.Copy(pToken, 0)
                p2.Add(pTokenR)
            End If

            Dim p3 As C_Token_Link_and_Add = New C_Token_Link_and_Add
            p3.pLink = p2
            p3.bAdd = bAdd

            Return p3 ' pTokenReturn
        End Function

        ''' <summary>
        ''' 判断是否在激活结构里有这个结构
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Check_In_Active_Treap() As Boolean

        End Function

        Public Sub read_fromLine(ByVal strLine As String)

            Dim pos As Integer = strLine.IndexOf("|")
            If pos = -1 Then Exit Sub

            Dim strLeft As String = strLine.Substring(0, pos)
            Dim strRight As String = strLine.Substring(pos + 1)

            read_fromDic(strLeft, strRight, "")
        End Sub

        Public iCount_Read As Int32 = 0

        Public Sub read_fromDic(ByVal strLeft As String, ByVal strRight As String, ByVal strActive As String)
            strLeft = strLeft.Trim()
            If strLeft = "" Then Exit Sub
            If strLeft.StartsWith("'") Then Exit Sub

            iCount_Read += 1
            System.Console.WriteLine("CRL=" & iCount_Read & ":" & strLeft & ":" & strRight & ":" & strActive)

            Dim pTreapReplace As Treap(Of C_Word_Convert) = pTreapWord.find(New C_K_Str(strLeft))
            If pTreapReplace Is Nothing Then
                pTreapReplace = New Treap(Of C_Word_Convert)
                pTreapWord.insert(New C_K_Str(strLeft), pTreapReplace)
            End If
            Dim pWord As C_Word_Convert = New C_Word_Convert(strLeft, strRight, strActive)

            pTreapReplace.insert(New C_K_Str(strRight), pWord)


            '同一个word激活的结构放在一起
            Dim pTreapWord_Convert As Treap(Of C_Word_Convert) = GetTreap(strActive)
            If pTreapWord_Convert Is Nothing Then
                pTreapWord_Convert = New Treap(Of C_Word_Convert)
                pTreapActive.insert(New C_K_Str(strActive), pTreapWord_Convert)
            End If
            pTreapWord_Convert.insert(New C_K_ID(pWord.ID), pWord)
        End Sub

        Public Function GetTreap(ByVal strKey As String) As Treap(Of C_Word_Convert)
            Return pTreapActive.find(New C_K_Str(strKey))
        End Function

    End Class
End Namespace
