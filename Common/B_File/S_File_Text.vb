Imports System.IO

Namespace Funny
    Public Class S_File_Text

        Public Shared Function Read(
             ByVal strFile As String) As String
            Return Read(strFile, False, Nothing)
        End Function
        Public Shared Function Read( _
             ByVal strFile As String, _
             Optional ByRef bFileExist As Boolean = False, _
             Optional ByRef pEncoding As System.Text.Encoding = Nothing) As String

            '从Txt文件中读取内容
            Dim strReturn As String = ""
            If pEncoding Is Nothing Then
                pEncoding = System.Text.Encoding.UTF8    '.GetEncoding(936)
            End If
            Dim pFS As FileStream
            Dim pSR As StreamReader
            Try
                bFileExist = File.Exists(strFile)
                If bFileExist = True Then
                    pFS = New FileStream(strFile, FileMode.OpenOrCreate, FileAccess.Read)
                    pSR = New StreamReader(pFS, pEncoding)

                    pSR.BaseStream.Seek(0, SeekOrigin.Begin)
                    strReturn = pSR.ReadToEnd()

                    pSR.Close()
                    pFS.Close()
                Else
                    strReturn = "" ' strFile
                End If
            Catch ex As Exception

            End Try

            pSR = Nothing
            pFS = Nothing

            Return strReturn
        End Function


        Public Shared Function Read_Begin(
             ByVal strFile As String,
             Optional ByRef pEncoding As System.Text.Encoding = Nothing) As StreamReader

            '从Txt文件中读取内容
            Dim strReturn As String = ""
            If pEncoding Is Nothing Then
                pEncoding = System.Text.Encoding.UTF8    '.GetEncoding(936)
            End If
            Dim pFS As FileStream
            Dim pSR As StreamReader = Nothing
            Try
                Dim bFileExist As Boolean = File.Exists(strFile)
                If bFileExist = True Then
                    pFS = New FileStream(strFile, FileMode.OpenOrCreate, FileAccess.Read)
                    pSR = New StreamReader(pFS, pEncoding)

                    pSR.BaseStream.Seek(0, SeekOrigin.Begin)
                Else
                    strReturn = "" ' strFile
                End If
            Catch ex As Exception
                Debug.Print(ex.ToString)
            End Try

            Return pSR
        End Function

        Public Shared Function Read_ifEnd(pSR As StreamReader) As Boolean
            Return pSR.Peek = -1
        End Function

        Public Shared Function Read_Line(ByRef pSR As StreamReader) As String
            Return pSR.ReadLine()
        End Function

        Public Shared Sub Read_End(ByRef pSR As StreamReader)
            pSR.Close()
        End Sub

        Public Shared Function WriteContent( _
            ByVal strFile As String, _
            ByVal strContent As String, _
            ByVal bAppend As Boolean, _
            Optional ByRef pEncoding As System.Text.Encoding = Nothing) As String

            Dim pSW As StreamWriter
            Dim pFS As FileStream, strReturn As String = ""


            Try
                If strFile = "" Then Return ""
                '不要File.Delete,直接覆盖即可,
                '否则可能保存的时候失败会没有静态文件!()

                S_SYS.InitDir(strFile)    '创建目录等

                If bAppend Then
                    pFS = New FileStream(strFile, FileMode.Append, FileAccess.Write)
                Else
                    pFS = New FileStream(strFile, FileMode.Create, FileAccess.Write)
                End If

                pSW = New StreamWriter(pFS, System.Text.Encoding.UTF8)

                '附代码页表:
                'Code Page Character Set 语种 
                '936:			gb2312(简体中文(GB2312))
                '52936 hz-gb-2312 简体中文 (HZ) 
                '950:			big5(繁体中文(Big5))
                '50932:		  _autodetect(日语(自动选择))
                '949 ks_c_5601-1987 朝鲜语 

                pSW.WriteLine(strContent)

                pSW.Close()
                pFS.Close()
            Catch ex As Exception
                strReturn = ex.Message
            End Try
            pSW = Nothing
            pFS = Nothing

            Return strReturn
        End Function

        Public Shared Function Write( _
            ByVal strFile As String, _
            ByVal strContent As String, _
            ByVal bAppend As Boolean, _
            Optional ByVal WithBOM As Boolean = True, _
            Optional ByRef pEncoding As System.Text.Encoding = Nothing) As String

            Dim pSW As StreamWriter
            Dim pFS As FileStream, strReturn As String = ""


            Try
                If strFile = "" Then Return ""
                '不要File.Delete,直接覆盖即可,
                '否则可能保存的时候失败会没有静态文件!()

                S_SYS.InitDir(strFile)    '创建目录等

                If bAppend Then
                    pFS = New FileStream(strFile, FileMode.Append, FileAccess.Write)
                Else
                    pFS = New FileStream(strFile, FileMode.Create, FileAccess.Write)
                End If

                'Dim utf8WithoutBom As New System.Text.UTF8Encoding(WithBOM)
                'pSW = New StreamWriter(pFS, utf8WithoutBom)

                If pEncoding Is Nothing Then
                    pEncoding = System.Text.Encoding.UTF8
                End If

                If pEncoding Is System.Text.Encoding.UTF8 Then
                    If WithBOM = False Then
                        Dim utf8WithoutBom As New System.Text.UTF8Encoding(WithBOM)
                        pSW = New StreamWriter(pFS, utf8WithoutBom)
                    Else
                        pSW = New StreamWriter(pFS, System.Text.Encoding.UTF8)
                    End If
                Else
                    pSW = New StreamWriter(pFS, pEncoding)
                End If

                '附代码页表:
                'Code Page Character Set 语种 
                '936:			gb2312(简体中文(GB2312))
                '52936 hz-gb-2312 简体中文 (HZ) 
                '950:			big5(繁体中文(Big5))
                '50932:		  _autodetect(日语(自动选择))
                '949 ks_c_5601-1987 朝鲜语 

                pSW.Write(strContent)

                pSW.Close()
                pFS.Close()
                Dim strSplit() As String = strFile.Split("\")
                If strSplit.Length > 0 Then
                    strReturn = "Write成功," & strSplit(strSplit.Length - 1)
                Else
                    strReturn = "Write成功."
                End If
            Catch ex As Exception
                strReturn = ex.Message
            End Try
            pSW = Nothing
            pFS = Nothing

            Return strReturn
        End Function




        Public Shared Function Write_Begin( _
            ByVal strFile As String, _
            ByVal bAppend As Boolean, _
            Optional ByVal WithBOM As Boolean = True, _
            Optional ByRef pEncoding As System.Text.Encoding = Nothing) As StreamWriter

            Dim pSW As StreamWriter
            Dim pFS As FileStream, strReturn As String = ""


            Try
                If strFile = "" Then Return Nothing
                S_SYS.InitDir(strFile)    '创建目录等

                If bAppend Then
                    pFS = New FileStream(strFile, FileMode.Append, FileAccess.Write)
                Else
                    pFS = New FileStream(strFile, FileMode.Create, FileAccess.Write)
                End If
                If pEncoding Is Nothing Then
                    pEncoding = System.Text.Encoding.UTF8
                End If

                If WithBOM AndAlso (pEncoding Is Nothing OrElse pEncoding Is System.Text.Encoding.UTF8) Then
                    Dim utf8WithoutBom As New System.Text.UTF8Encoding(WithBOM)
                    pSW = New StreamWriter(pFS, utf8WithoutBom)
                Else
                    pSW = New StreamWriter(pFS, pEncoding)
                End If
                Return pSW
            Catch ex As Exception
                strReturn = ex.Message
            End Try
            pSW = Nothing

            Return Nothing
        End Function

        Public Shared Sub Write_Line(ByRef pSW As StreamWriter, ByVal strLine As String)
            pSW.WriteLine(strLine)
        End Sub

        Public Shared Sub Write_End(ByRef pSW As StreamWriter)
            pSW.Close()
            pSW = Nothing
        End Sub
    End Class
End Namespace

