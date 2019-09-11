Imports System.IO

Imports CommonTreapVB.TreapVB
Imports B_Data.Funny
Imports B_String.Funny

Namespace com.Funny.Segmentation

    Public Class C_Filter_Invert

        Public pTreapWord As New C_Treap_Funny(Of Treap(Of C_Word_Convert))

        Public Function InitDictionary(ByVal strFile As String) As String
            '把单词读取到内存，这样就可以分词了
            '从Txt文件中读取内容
            If File.Exists(strFile) = True Then
                Dim pFS As FileStream = New FileStream( _
                    strFile, FileMode.OpenOrCreate, FileAccess.Read)
                Dim pSR As StreamReader = New StreamReader( _
                    pFS, System.Text.Encoding.UTF8)

                Dim Count As Integer = 0

                pSR.BaseStream.Seek(0, SeekOrigin.Begin)
                Do While pSR.Peek <> -1
                    read_A_Line_FromDictionary(pSR.ReadLine)
                    Count += 1
                Loop
                Debug.Print("ListCount:" & pTreapWord.Size & ",WordCount:" & Count)

                pSR.Close() : pFS.Close()
                pSR = Nothing : pFS = Nothing
                Return "初始化：" & strFile
            Else
                Return "词库文件 \Data\Word.Txt 不存在"
            End If
        End Function

        Public Function InitDictionary_Remove(ByVal strFile As String) As String
            '把单词读取到内存，这样就可以分词了
            '从Txt文件中读取内容
            If File.Exists(strFile) = True Then
                Dim pFS As FileStream = New FileStream( _
                    strFile, FileMode.OpenOrCreate, FileAccess.Read)
                Dim pSR As StreamReader = New StreamReader( _
                    pFS, System.Text.Encoding.UTF8)

                Dim Count As Integer = 0

                pSR.BaseStream.Seek(0, SeekOrigin.Begin)
                Do While pSR.Peek <> -1
                    read_A_Line_and_Remove(pSR.ReadLine)
                    Count += 1
                Loop
                Debug.Print("Remove ListCount:" & pTreapWord.Size & ",WordCount:" & Count)

                pSR.Close() : pFS.Close()
                pSR = Nothing : pFS = Nothing
                Return "初始化：" & strFile
            Else
                Return "词库文件不存在"
            End If
        End Function



        Public Sub read_A_Line_FromDictionary(ByVal strLine As String)
            If strLine.StartsWith("'") Then Exit Sub

            Dim pTreap As Treap(Of C_Word_Convert), strLeft As String
            Dim pWord As C_Word_Convert
            strLeft = Strings.Left(strLine, 1)
            pTreap = pTreapWord.find(New C_K_Str(strLeft))
            If pTreap Is Nothing Then
                pTreap = New Treap(Of C_Word_Convert)
                pTreapWord.insert(New C_K_Str(strLeft), pTreap)
            End If
            pWord = New C_Word_Convert(strLeft, "", "")
            pTreap.insert(New C_K_Str(pWord.Word), pWord)
            If Len(strLine) > pTreap.KeyMaxLen Then
                pTreap.KeyMaxLen = Len(strLine)
            End If
        End Sub

        Public Sub read_A_Line_and_Remove(ByVal strLine As String)
            If strLine.StartsWith("'") Then Exit Sub

            Dim pTreap As Treap(Of C_Word_Convert), strLeft As String
            Dim pWord As C_Word_Convert
            strLeft = Strings.Left(strLine, 1)
            pTreap = pTreapWord.find(New C_K_Str(strLeft))
            If pTreap Is Nothing Then
                pTreap = New Treap(Of C_Word_Convert)
                pTreapWord.insert(New C_K_Str(strLeft), pTreap)
            End If
            pWord = New C_Word_Convert(strLeft, "", "")
            pTreap.Remove(New C_K_Str(pWord.Word))
        End Sub

        Public Function ifHasThisWord(ByVal strWord As String) As Boolean
            If strWord = "" Then Return False

            Dim pTreap As Treap(Of C_Word_Convert) = pTreapWord.find(New C_K_Str(strWord.Substring(0, 1)))
            If Not pTreap Is Nothing Then
                Dim pWordConvert As C_Word_Convert = pTreap.find(New C_K_Str(strWord))
                If Not pWordConvert Is Nothing Then
                    Return True
                End If
            End If
            Return False
        End Function

        Public Function Segmentation_CompareWord(
            ByRef pTreap As Treap(Of C_Word_Convert),
            ByVal strSplit() As String,
            ByVal i As Integer, ByVal MaxI As Integer) As String

            Dim pWordConvert As C_Word_Convert
            Dim strTmp As String = ""
            Dim strReturn As String = ""
            Dim j As Integer

            For j = i To Math.Min(MaxI, i + pTreap.KeyMaxLen - 1)
                strTmp &= strSplit(j)
            Next

            For j = Len(strTmp) To 2 Step -1
                pWordConvert = pTreap.find(New C_K_Str(Left(strTmp, j)))
                If Not pWordConvert Is Nothing Then
                    If pWordConvert.OutPutWord <> "" Then
                        strReturn = pWordConvert.OutPutWord
                    Else
                        strReturn = pWordConvert.Word
                    End If
                    Exit For
                End If
            Next

            Return strReturn
        End Function


        Public Function Segmentation_List( _
            ByVal strContent As String, _
            ByVal bNumFilter As Boolean) As String

            Dim pTreap As Treap(Of String) = Segmentation(strContent, True, bNumFilter)
            Dim p As TreapEnumerator = pTreap.Elements(True)
            Dim strKey As String
            Dim strReturn As String = ""

            Do While (p.HasMoreElements())
                strKey = CType(p.NextElement(), String)
                strReturn &= strKey & " "
            Loop

            Return strReturn.Trim()

        End Function

        Public Function Segmentation(
            ByVal strContent As String,
            ByVal bList As Boolean,
            ByVal bNumFilter As Boolean) As Treap(Of String)
            '分词，返回结果为一棵词树，这棵树都是以词汇的第一个汉字为Key，他的子树势这个汉字对应的词
            Dim pTreapReturn As Treap(Of String) = Segmentation_Sub(pTreapReturn, strContent, bList, bNumFilter)

            Return pTreapReturn
        End Function

        Public Function Filter(ByVal strInput As String) As String
            Dim strSplit() As String = strInput.Split(" ")
            Dim strReturn As String = ""
            For i As Integer = 0 To strSplit.Length - 1
                If Me.ifHasThisWord(strSplit(i)) Then
                    strReturn += strSplit(i) + " "
                End If
            Next

            Return Trim(strReturn)
        End Function

        Public Function Segmentation_Sub(
            ByRef pTreapReturn As Treap(Of String),
            ByVal strContent As String,
            ByVal bList As Boolean,
            ByVal bNumFilter As Boolean) As Treap(Of String)

            strContent = S_Strings.Blank_Chinese(strContent)
            Dim strSplit() As String = Split(strContent, " ")
            Dim i, j As Integer
            Dim strReturnWord As String = ""
            Dim strSplitTmp() As String
            Dim pTreap As Treap(Of C_Word_Convert)
            Dim MaxI As Integer = UBound(strSplit)
            For i = 0 To MaxI
                If Len(strSplit(i)) = 1 Then
                    '如果是中文
                    pTreap = pTreapWord.find(New C_K_Str(strSplit(i)))
                    If Not pTreap Is Nothing Then
                        strReturnWord = Segmentation_CompareWord(pTreap, strSplit, i, MaxI)
                        If strReturnWord <> "" Then
                            strSplitTmp = Split(strReturnWord, " ")
                            For j = 0 To UBound(strSplitTmp)
                                If bList Then
                                    pTreapReturn.insert(New C_K_ID(i * 10 + j), strSplitTmp(j))
                                Else
                                    pTreapReturn.insert(New C_K_Str(strSplitTmp(j)), strSplitTmp(j))
                                End If
                                i = i + strSplitTmp(j).Length
                            Next
                            i = i - 1
                        Else
                            strReturnWord = strSplit(i)
                            If bList Then
                                pTreapReturn.insert(New C_K_ID(i * 10), strReturnWord)
                            Else
                                pTreapReturn.insert(New C_K_Str(strReturnWord), strReturnWord)
                            End If
                        End If
                    Else
                        strReturnWord = strSplit(i)
                        If bList Then
                            pTreapReturn.insert(New C_K_ID(i * 10), strReturnWord)
                        Else
                            pTreapReturn.insert(New C_K_Str(strReturnWord), strReturnWord)
                        End If
                    End If
                ElseIf Len(strSplit(i)) > 1 Then
                    strReturnWord = strSplit(i)
                    If bNumFilter Then
                        '过滤数字
                        If IsNumeric(strReturnWord) = False Then
                            If bList Then
                                pTreapReturn.insert(New C_K_ID(i * 10), strReturnWord)
                            Else
                                pTreapReturn.insert(New C_K_Str(strReturnWord), strReturnWord)
                            End If
                        End If
                    Else
                        If bList Then
                            pTreapReturn.insert(New C_K_ID(i * 10), strReturnWord)
                        Else
                            pTreapReturn.insert(New C_K_Str(strReturnWord), strReturnWord)
                        End If
                    End If

                End If
            Next
            Return pTreapReturn
        End Function

    End Class
End Namespace
