Imports System.IO
Imports CommonTreapVB.TreapVB
Imports B_Data.Funny
Imports B_String.Funny
Imports B_Token.Funny


Namespace com.Funny.Segmentation

    Public Class C_Convert

        Public pTreapWord As C_Treap_Funny(Of Treap(Of C_Word_Convert)) = New C_Treap_Funny(Of Treap(Of C_Word_Convert))

        ''' <summary>
        ''' ��Ҫ����Ľṹ
        ''' </summary>
        ''' <remarks></remarks>
        Private pTreapActive As New Treap(Of Treap(Of C_Word_Convert))


        ''' <summary>
        ''' ת��
        ''' </summary>
        ''' <param name="pToken_Link"></param>
        Public Function getConvert( _
                ByRef pToken_Link As C_Token_Link) As C_Token_Link_and_Add

            Dim bConvert As Boolean = False '����ֵ������ΪFalse
            Dim pLinkR As C_Token_Link = New C_Token_Link()
            pLinkR.strSentence = pToken_Link.strSentence
            Dim pToken_Old As C_Token

            For i As Integer = 0 To pToken_Link.Count - 1
                pToken_Old = pToken_Link.Item(i)

                If pToken_Old.bCheckConvert = False Then '���û��ת����
                    Dim bAdd As Boolean = False
                    Dim pLinkNew2 As C_Token_Link_and_Add = getConvert_Token(New Treap(Of C_Word_Convert), pToken_Old) '���token��ɵ�link����ΪConvert���ܻ��һ�����ʱ�Ϊ�������
                    Dim pLinkNew As C_Token_Link = pLinkNew2.pLink

                    'If bAdd Then bConvert = True

                    If pLinkNew2.bAdd Then
                        bConvert = True '����ղ��������Token��Ϊ�����ʾ������ת��
                        If pLinkNew.Count = 1 Then  '���ת�����Ծ���һ������
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
                        ElseIf pLinkNew.Count > 1 Then     '���ת����Ϊ������ʣ����磬����=���� ��֢
                            For j As Integer = 0 To pLinkNew.Count - 1
                                pLinkR.Add(pLinkNew.Item(j))
                            Next
                        Else                            '0�� ����һ�㲻�ᵽ��
                            pLinkR.Add(pToken_Old)
                        End If
                    Else
                        pLinkR.Add(pToken_Old)
                    End If
                Else
                    pLinkR.Add(pToken_Old)
                End If
            Next

            If bConvert Then    '�����Ѿ��滻��
                pLinkR.iStart = pLinkR.Item(0).iStart
                pLinkR.iEnd = pLinkR.Item(pLinkR.Count() - 1).iEnd
            Else                '�����ͬ������ԭ�ȵ�
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
        ''' <param name="pTreap_Active">����Ľṹ</param>
        ''' <param name="pToken"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getConvert_Token(
            ByRef pTreap_Active As Treap(Of C_Word_Convert),
            ByRef pToken As C_Token) As C_Token_Link_and_Add

            If pToken.Count = 0 Then Return Nothing

            pToken.bCheckConvert = True

            Dim bAdd As Boolean = False

            Dim strKey As String = pToken.Item(0) '��һ����ԭʼ��δת���Ĵʻ�

            Dim pTreap_Token As New Treap(Of C_Word_Convert) '�����Ƿ����µģ�����оʹ洢����

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
                    If pTreap_Token.find(New C_K_Str(strOutput)) Is Nothing Then '��������֣������ŵ� {INT} ������
                        bAdd = True
                        pConvert = New C_Word_Convert(strKey, strOutput, "")
                        pTreap_Token.insert(New C_K_Str(strOutput), pConvert)
                    End If
                ElseIf Len(strKey) < 5 AndAlso S_Strings.isEnglish(strKey) Then '����ǵ��ʸ����ŵ� {Word.L.x} ������
                    strOutput = "Word.L." & Len(strKey)
                    If pTreap_Token.find(New C_K_Str(strOutput)) Is Nothing Then
                        bAdd = True
                        pConvert = New C_Word_Convert(strKey, strOutput, "")
                        pTreap_Token.insert(New C_K_Str(strOutput), pConvert)
                    End If
                End If

                Dim pTreapReplace As Treap(Of C_Word_Convert) = pTreapWord.find(New C_K_Str(strKey)) '�����Ƿ��������� ���Է��� �ļ���
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
                            p2.Add(New C_Token(strSplit(i), 0, 0, 1)) 'iStart iEnd ��������
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
        ''' �ж��Ƿ��ڼ���ṹ��������ṹ
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


            'ͬһ��word����Ľṹ����һ��
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
