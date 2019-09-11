
Imports B_Data.Funny
Imports B_String.Funny

Imports System.IO
Imports System.Data.SQLite
Imports B_TreapVB.TreapVB

Namespace com.Funny.Segmentation

    Public Class C_Segmentation

        Public pTreapWord As New C_Treap_Funny(Of Treap(Of C_Word_Seg))

        Public Sub InitDictionaryByDir(ByVal strDir As String) ', ByRef pConvert As C_Convert)
            pTreapWord = New C_Treap_Funny(Of Treap(Of C_Word_Seg))
            Dim strFile() As String
            Try
                strFile = Directory.GetFiles(strDir, "*.txt")

                Dim strTmp As String
                For i As Integer = 0 To UBound(strFile)
                    strTmp = InitFile(strFile(i)) ', pConvert)
                Next
            Catch ex As Exception

            End Try

        End Sub

        Public Function InitFile(ByVal strFile As String) As String ', ByRef pConvert As C_Convert
            '把单词读取到内存，这样就可以分词了
            '从Txt文件中读取内容
            If File.Exists(strFile) = True Then
                Dim pFS As FileStream = New FileStream( _
                    strFile, FileMode.OpenOrCreate, FileAccess.Read)
                Dim pSR As StreamReader = New StreamReader( _
                    pFS, System.Text.Encoding.UTF8)

                Dim strLine As String

                pSR.BaseStream.Seek(0, SeekOrigin.Begin)
                Do While pSR.Peek >= 0
                    strLine = pSR.ReadLine
                    readLine_fromDic(strLine)
                Loop

                pSR.Close() : pFS.Close()
                pSR = Nothing : pFS = Nothing
                Return "初始化：" & strFile
            Else
                Return "词库文件 \Data\Word.Txt 不存在"
            End If
        End Function

        Public Sub InitSQLite(ByVal strFile As String)
            Dim SQLconnect As New SQLite.SQLiteConnection()
            SQLconnect.ConnectionString = "Data Source=" & strFile ' Application.StartupPath & "\AI.sqlite;"
            SQLconnect.Open()
            Dim SQLcommand As SQLiteCommand = SQLconnect.CreateCommand


            Dim strLine As String

            Dim Query As New SQLiteCommand( _
                                "SELECT Name FROM Seg", SQLconnect)
            Dim Reader As SQLiteDataReader
            Reader = Query.ExecuteReader
            Do While Reader.Read
                If IsDBNull(Reader("Name")) = False Then
                    strLine = Reader("Name")
                    readLine_fromDic(strLine)
                End If
            Loop

            SQLcommand.Dispose()
            SQLconnect.Close()
        End Sub

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



        Public Sub readLine_fromDic(ByVal strLine As String)
            If strLine Is Nothing Then Return
            strLine = strLine.Trim()
            If strLine.StartsWith("'") Then Exit Sub
            If strLine = "" Then Return


            Dim pTreap As Treap(Of C_Word_Seg), strLeft As String
            Dim pWord As C_Word_Seg
            Dim strLine2 As String = S_Strings.Blank_Chinese(strLine)
            Dim strSplit() As String = Split(strLine2, " ")

            strLeft = strSplit(0)  ' Strings.Left(strLine, 1)
            pTreap = pTreapWord.find(New C_K_Str(strLeft))
            If pTreap Is Nothing Then
                pTreap = New Treap(Of C_Word_Seg)
                pTreapWord.insert(New C_K_Str(strLeft), pTreap)
            End If
            If Len(strLine) > pTreap.KeyMaxLen Then
                pTreap.KeyMaxLen = strSplit.Length ' Len(strLine)
            End If


            Dim strSplit2() As String = Split(strLine, "=")

            If strSplit2.Length > 1 Then
                pWord = New C_Word_Seg(strSplit2(0), strSplit2(1), "")
            Else
                pWord = New C_Word_Seg(strSplit2(0), "", "")
            End If

            If pWord.Word.IndexOf(" ") > -1 Then Exit Sub

            Dim pWord_Seg As C_Word_Seg = pTreap.find(New C_K_Str(pWord.Word))

            If pWord_Seg IsNot Nothing Then
                If pWord_Seg.OutPutWord = "" Then
                    pTreap.insert(New C_K_Str(pWord.Word), pWord)
                End If
            Else
                pTreap.insert(New C_K_Str(pWord.Word), pWord)
            End If


        End Sub

        Public Sub read_A_Line_and_Remove(ByVal strLine As String)
            If strLine.StartsWith("'") Then Exit Sub

            Dim pTreap As Treap(Of C_Word_Seg), strLeft As String
            Dim pWord_Seg As C_Word_Seg
            strLeft = Strings.Left(strLine, 1)
            pTreap = pTreapWord.find(New C_K_Str(strLeft))
            If pTreap Is Nothing Then
                pTreap = New Treap(Of C_Word_Seg)
                pTreapWord.insert(New C_K_Str(strLeft), pTreap)
            End If
            pWord_Seg = New C_Word_Seg(strLeft, "", "")
            pTreap.Remove(New C_K_Str(pWord_Seg.Word))
        End Sub


        Public Function Segmentation_CompareWord(
            ByRef pTreap As Treap(Of C_Word_Seg),
            ByVal strSplit() As String,
            ByVal i As Integer, ByVal Token_Max As Integer) As String()

            Dim pWord_Seg As C_Word_Seg
            Dim strTmp As String = ""
            Dim strReturn As String = ""
            Dim j As Integer
            Dim strTokens(Token_Max + 10) As String

            Dim k As Int32 = 1

            Dim Token_Max_Now As Int32 = Math.Min(Token_Max, i + pTreap.KeyMaxLen)
            For j = i To Token_Max_Now
                strTmp &= strSplit(j)
                strTokens(k) = strTmp
                k += 1
            Next

            Dim Count As Int32 = 0
            Dim strKey As String
            For j = Token_Max_Now + 1 To 1 Step -1
                strKey = strTokens(j)
                pWord_Seg = pTreap.find(New C_K_Str(strKey)) 'Left(strTmp, j)))
                If Not pWord_Seg Is Nothing Then
                    Count = j
                    If pWord_Seg.OutPutWord <> "" Then
                        strReturn = pWord_Seg.OutPutWord
                    Else
                        strReturn = pWord_Seg.Word
                    End If
                    Exit For
                End If
            Next

            Return {strReturn, Count}
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

        Public Function Segmentation_FilterChar( _
            ByVal strContent As String) As String

            Dim pTreap As Treap(Of String) = Segmentation(strContent, True, False)
            Dim p As TreapEnumerator = pTreap.Elements(True)
            Dim strKey As String
            Dim strReturn As String = ""

            Do While (p.HasMoreElements())
                strKey = CType(p.NextElement(), String)
                If strKey.StartsWith("{") = False Then
                    strReturn &= strKey & " "
                End If
            Loop

            Return strReturn.Trim()

        End Function

        ''' <summary>
        ''' 分词，返回结果为一棵词树，
        ''' 这棵树都是以词汇的第一个汉字为Key，
        ''' 它的子树势这个汉字对应的词
        ''' </summary>
        ''' <param name="strContent"></param>
        ''' <param name="bList"></param>
        ''' <param name="bNumFilter"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Segmentation(
            ByVal strContent As String,
            ByVal bList As Boolean,
            ByVal bNumFilter As Boolean) As Treap(Of String)

            Dim pTreapReturn As New Treap(Of String)

            Return Segmentation_Sub(pTreapReturn, strContent, bList, bNumFilter)
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
            Dim pTreap As Treap(Of C_Word_Seg)
            Dim Token_Max As Integer = UBound(strSplit)
            For i = 0 To Token_Max
                pTreap = pTreapWord.find(New C_K_Str(strSplit(i)))
                If Not pTreap Is Nothing Then
                    Dim pArray() As String = Segmentation_CompareWord(pTreap, strSplit, i, Token_Max)
                    strReturnWord = pArray(0)
                    Dim Count As Int32 = pArray(1)

                    If strReturnWord <> "" Then
                        strSplitTmp = Split(strReturnWord, " ")
                        For j = 0 To UBound(strSplitTmp)
                            If bList Then
                                pTreapReturn.insert(New C_K_ID(i * 10 + j), strSplitTmp(j))
                            Else
                                pTreapReturn.insert(New C_K_Str(strSplitTmp(j)), strSplitTmp(j))
                            End If
                        Next
                        i = i + Count - 1
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
            Next
            Return pTreapReturn
        End Function

    End Class
End Namespace
