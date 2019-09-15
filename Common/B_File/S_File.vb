Imports System.IO
Imports System.Text



Namespace Funny

    Public Class S_File
        Private Structure SHFILEOPSTRUCT
            Dim hwnd As Integer
            Dim wFunc As Integer
            Dim pFrom As String
            Dim pTo As String
            Dim fFlags As Short
            Dim fAnyOperationsAborted As Boolean
            Dim hNameMappings As Integer
            Dim lpszProgressTitle As String
        End Structure

        Private Const FO_DELETE As Short = &H3S
        Private Const FOF_ALLOWUNDO As Short = &H40S
        Private Const FOF_NOCONFIRMATION As Short = &H10S

        Private Declare Function SHFileOperation Lib "shell32.dll" Alias _
          "SHFileOperationA" (ByRef lpFileOp As SHFILEOPSTRUCT) As Integer

        Public Shared Function Recycle(ByVal sPath As String,
                                       Optional bDialog As Boolean = False) As Integer

            Try
                If bDialog = False Then
                    My.Computer.FileSystem.DeleteFile(sPath,
                            FileIO.UIOption.OnlyErrorDialogs,
                            FileIO.RecycleOption.SendToRecycleBin)
                Else

                    My.Computer.FileSystem.DeleteFile(sPath,
                            FileIO.UIOption.AllDialogs,
                            FileIO.RecycleOption.SendToRecycleBin)
                End If
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try
            Return 0
        End Function

        Public Shared Function Size(ByVal strFile As String) As Long

            Return New FileInfo(strFile).Length
        End Function

        '判断是否存在
        Public Shared Function Exists(ByVal strFile As String) As Boolean
            Return File.Exists(strFile)

        End Function

        Public Shared Sub MoveFile(ByVal strFile1 As String, ByVal strFile2 As String)
            '
            Try
                File.Move(strFile1, strFile2)
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try
        End Sub

        '获取文件修改时间
        Public Shared Function FileModifyTime(ByVal strFile As String) As Date
            Dim fi As New FileInfo(strFile)
            Return fi.LastWriteTime
        End Function

        Public Shared Function ByteArrayToString(ByVal arrInput() As Byte) As String

            Dim sb As New System.Text.StringBuilder(arrInput.Length * 2)

            For i As Integer = 0 To arrInput.Length - 1
                sb.Append(arrInput(i).ToString("X2"))
            Next

            Return sb.ToString().ToLower

        End Function

        Public Shared Function MD5CalcFile(ByVal filepath As String) As String
            Try

                Using reader As New System.IO.FileStream(filepath, IO.FileMode.Open, IO.FileAccess.Read)
                    Using md5 As New System.Security.Cryptography.MD5CryptoServiceProvider

                        Dim hash() As Byte = md5.ComputeHash(reader)
                        Return ByteArrayToString(hash)
                    End Using
                End Using
            Catch ex As Exception

            End Try
            Return "error"

        End Function



        Public Shared Function Getcode(ByVal Path As String) As String ' System.Text.Encoding

            Dim fs As New FileStream(Path, FileMode.Open, FileAccess.Read)

            Dim r As New BinaryReader(fs, System.Text.Encoding.Default)

            Dim ss As Byte() = r.ReadBytes(100)
            r.Close()
            fs.Close()
            If ss.Length > 2 Then
                If Hex(ss(0)) >= "EF" Then
                    If Hex(ss(0)) = "EF" And Hex(ss(1)) = "BB" And Hex(ss(2)) = "BF" Then
                        Return System.Text.Encoding.UTF8.WebName
                    ElseIf Hex(ss(0)) = "FE" And Hex(ss(1)) = "FF" Then
                        Return System.Text.Encoding.BigEndianUnicode.WebName
                    ElseIf Hex(ss(0)) = "FF" And Hex(ss(1)) = "FE" Then
                        Return System.Text.Encoding.Unicode.WebName
                    Else
                        Return System.Text.Encoding.Default.WebName
                    End If
                Else
                    If OnUtf8(ss) Then
                        Return System.Text.Encoding.UTF8.WebName
                    Else
                        Return System.Text.Encoding.Default.WebName
                    End If
                End If
            Else
                Return System.Text.Encoding.UTF8.WebName
            End If
        End Function



        Public Shared Function Read_Ini( _
             ByVal strFile As String, ByVal strEncode As String) As Dictionary(Of String, String)

            Dim pDic As Dictionary(Of String, String) = New Dictionary(Of String, String)

            '从Txt文件中读取内容
            Dim strReturn As String = ""
            Dim pEncoding As Text.Encoding
            If strEncode = "" Then
                pEncoding = System.Text.Encoding.UTF8
            Else
                pEncoding = System.Text.Encoding.GetEncoding(strEncode)
            End If
            Dim pFS As FileStream
            Dim pSR As StreamReader
            Try
                Dim bFileExist As Boolean = File.Exists(strFile)
                If bFileExist = True Then
                    pFS = New FileStream(strFile, FileMode.OpenOrCreate, FileAccess.Read)
                    pSR = New StreamReader(pFS, pEncoding)

                    pSR.BaseStream.Seek(0, SeekOrigin.Begin)

                    Dim strLine As String = "'"
                    Dim strSection As String = "main"
                    pSR.BaseStream.Seek(0, SeekOrigin.Begin)
                    Do While pSR.Peek >= 0
                        strLine = pSR.ReadLine
                        If strLine.StartsWith("#") Then
                        ElseIf strLine.StartsWith("'") Then
                        ElseIf strLine.StartsWith("/") Then
                        ElseIf strLine.StartsWith("[") Then
                            strSection = strLine.Trim().Replace("[", "").Replace("]", "")
                        Else
                            Dim strSplit() As String = strLine.Split("=")
                            If strSplit.Length > 1 Then
                                pDic.Add(strSection & "." & strSplit(0), strSplit(1))
                            End If
                        End If
                    Loop


                    pSR.Close()
                    pFS.Close()
                Else
                    strReturn = "" ' strFile
                End If
            Catch ex As Exception

            End Try

            pSR = Nothing
            pFS = Nothing

            Return pDic
        End Function


        Public Shared Function OnUtf8(ByVal byts() As Byte) As Boolean
            Try
                Dim i As Int32, AscN As Int32
                Do While i <= UBound(byts)
                    If byts(i) < 128 Then
                        i += 1       'ascii字符   
                        AscN += 1
                    ElseIf (byts(i) And &HE0) = &HC0 And (byts(i + 1) And &HC0) = &H80 Then
                        i += 2      '2个字节的utf8   
                    ElseIf (byts(i) And &HF0) = &HE0 And (byts(i + 1) And &HC0) = &H80 And (byts(i + 2) And &HC0) = &H80 Then
                        i += 3       '3个字节的utf8   
                    Else
                        Return False
                    End If
                Loop

                '断可以不要, 当全部是ascii字符时, 被划分到哪种编码
                If AscN = byts.Length Then
                    Return False
                Else
                    Return True
                End If
            Catch ex As Exception
                'MsgBox(ex.Message)   
            End Try

            Return True
        End Function



    End Class

End Namespace