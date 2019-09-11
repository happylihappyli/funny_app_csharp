Imports System.Text.RegularExpressions
Imports System.Net
Imports System.Net.Sockets
Imports System.IO
Imports System.Text
Imports System.Web
Imports System.Xml


Imports B_TreapVB.TreapVB
Imports System.Text.Encoding
Imports System.IO.Compression

Imports B_File.Funny
Imports B_Data.Funny
Imports B_XML.Funny
Imports System.Net.Security
Imports System.Security.Cryptography.X509Certificates

Namespace Funny
    Public Module S_Net
        Public bProxy As Boolean = False
        Public strProxyURL As String
        Public iProxyPort As Integer
        Public strProxyName As String
        Public strProxyPassword As String
        Public strProxyDomain As String

        Public pTreapCookie As New Treap(Of CookieContainer)
        Public pReturnValue As New Treap(Of CookieContainer)

        Private bInit As Boolean = False

        Public Sub init()
            Dim pSet As C_XML_Setting = New C_XML_Setting
            pSet.setFile(Environment.CurrentDirectory & "\Set.xml")
            If pSet.Read_Setting("Proxy", "Used") = "1" Then
                S_Net.bProxy = True
            End If

            S_Net.strProxyURL = pSet.Read_Setting("Proxy", "URL")
            S_Net.iProxyPort = Val(pSet.Read_Setting("Proxy", "Port"))
            S_Net.strProxyName = pSet.Read_Setting("Proxy", "UserName")
            S_Net.strProxyPassword = pSet.Read_Setting("Proxy", "Password")
            S_Net.strProxyDomain = pSet.Read_Setting("Proxy", "Domain")
            pSet = Nothing
            bInit = True
        End Sub


        Public Sub Download(ByVal url As String, ByVal strFile As String)
            Try
                Dim dFile As New WebClient

                Console.WriteLine(url)
                Console.WriteLine(strFile)

                dFile.DownloadFile(url, strFile)
            Catch ex As Exception
                Console.WriteLine(ex.ToString())
            End Try
        End Sub

        '=Net.Http(url,"get","postdata","utf-8",referenceurl,"0")
        '最后一个参数是1，就是GZIp解压缩
        '抓取web内容

        Public Function Http( _
                ByVal strURL As String, _
                ByVal strMethod As String, _
                ByVal HttpData As String, _
                ByVal strEncode As String, _
                Optional ByVal strRefererURL As String = "", _
                Optional ByVal strGzip As String = "0") As String

            Dim bGzip As Boolean = (strGzip = "1")

            If S_Net.bInit = False Then
                S_Net.init()
            End If
            If strEncode = "" Then strEncode = "UTF-8"
            Dim pEncoding As Text.Encoding = Text.Encoding.GetEncoding(strEncode)

            Dim strContentType As String = "application/x-www-form-urlencoded"
            'System.Net.ServicePointManager.Expect100Continue = False
            Dim req As HttpWebRequest

            If pEncoding Is Nothing Then
                pEncoding = Encoding.UTF8
            End If
            req = CType(WebRequest.Create(strURL), HttpWebRequest)
            req.Method = strMethod
            req.ContentType = strContentType

            req.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; zh-CN; rv:1.9.2.3) Gecko/20100401 Firefox/3.6.3 GTB6"
            req.Headers.Add("Accept-Charset", "GB2312,utf-8;q=0.7,*;q=0.7")
            If strRefererURL <> "" Then
                req.Referer = strRefererURL
            End If

            req.Timeout = 40000 '设置超时 
            req.ReadWriteTimeout = 100000

            Dim strEmail As String = Rnd()
            Dim pCookieCurrent As CookieContainer = pTreapCookie.find(New C_K_Str(strEmail))
            If pCookieCurrent Is Nothing Then
                pCookieCurrent = New CookieContainer()
                pTreapCookie.insert(New C_K_Str(strEmail), pCookieCurrent)
            End If
            req.CookieContainer = pCookieCurrent
            req.ServicePoint.Expect100Continue = False

            If bProxy Then
                Dim oProxy As New WebProxy(strProxyURL, iProxyPort)
                If strProxyDomain <> "" Then
                    oProxy.Credentials = New NetworkCredential(strProxyName, strProxyPassword, strProxyDomain)
                Else
                    oProxy.Credentials = New NetworkCredential(strProxyName, strProxyPassword)
                End If
                req.Proxy = oProxy
            End If

            Dim respHTML As String = ""
            Try
                Dim postStream As Stream = Nothing
                If UCase(strMethod) = "POST" Then
                    req.AllowAutoRedirect = True
                    req.Method = "POST"
                    Dim bytesData() As Byte = pEncoding.GetBytes(HttpData)
                    req.ContentLength = bytesData.Length
                    postStream = req.GetRequestStream()
                    postStream.Write(bytesData, 0, bytesData.Length)   '以上向服务器post信息。 
                Else
                    'req.GetResponse()
                End If
                'req.ProtocolVersion = HttpVersion.Version10 ' reference from 
                'req.AllowAutoRedirect = False
                'req.AllowWriteStreamBuffering = False
                'req.KeepAlive = False


                Dim rep As HttpWebResponse = CType(req.GetResponse(), HttpWebResponse)
                Dim pStream As Stream = rep.GetResponseStream

                'If bBinary Then
                '    '如果是二进制
                '    Dim inBuf As Byte() = New Byte(0 To 1024 * 1024 * 20 - 1) {}
                '    Dim bytesToRead As Integer = CInt(inBuf.Length)
                '    Dim bytesRead As Integer = 0
                '    Dim Pos As Integer = 0

                '    Dim fstr As New FileStream(strFile, FileMode.OpenOrCreate, FileAccess.Write)
                '    Dim bFinished As Boolean = False
                '    Do
                '        Do While bytesToRead > 0
                '            Dim n As Integer = pStream.Read(inBuf, bytesRead, bytesToRead)
                '            If n = 0 Then
                '                bFinished = True
                '                Exit Do
                '            End If
                '            bytesRead += n
                '            bytesToRead -= n
                '        Loop
                '        fstr.Write(inBuf, Pos, bytesRead)
                '        Pos += bytesRead
                '        bytesRead = 0
                '    Loop Until bFinished

                '    fstr.Close()
                'Else
                If bGzip Then
                    Dim gzip As New GZipStream(pStream, CompressionMode.Decompress)
                    Dim reader2 As New StreamReader(gzip, pEncoding)
                    respHTML = reader2.ReadToEnd()
                    reader2.Close()
                Else
                    Dim reader As StreamReader = New StreamReader(pStream, pEncoding) ' System.Text.Encoding.UTF8)
                    respHTML = reader.ReadToEnd
                    reader.Close()
                End If
                'If strFile <> "" Then
                '    S_File_Text.WriteContent(strFile, respHTML, False)
                'End If
                'End If

                If UCase(strMethod) = "POST" Then
                    If Not postStream Is Nothing Then
                        postStream.Close()
                    End If
                End If

                pStream.Close()
                rep.Close()
                req.Abort()

            Catch ex As Exception
                'If bReturnError Then
                '    respHTML = "网络连接错误：请重试! " & _
                '        ex.Message & "<br>" & _
                '        ex.Source & "<br>" & _
                '        ex.StackTrace
                'Else
                respHTML = ""
                'End If
            End Try

            Return respHTML '这就是向网络服务器post后返回的信息
        End Function

        ''' <summary>
        ''' 返回XML字符串
        ''' </summary>
        ''' <param name="strEmail"></param>
        ''' <param name="strURL"></param>
        ''' <param name="HttpData"></param>
        ''' <param name="strMethod"></param>
        ''' <param name="strEncode"></param>
        ''' <param name="strRefererURL"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function postData_return_XML( _
                ByVal strEmail As String, _
                ByVal strURL As String, _
                ByVal HttpData As String, _
                ByVal strMethod As String, _
                ByVal strEncode As String, _
                ByVal strRefererURL As String) As String
            Dim strXML As String = S_Net.http_post(
                strEmail, strURL,
                HttpData, strMethod, strEncode, strRefererURL)
            Dim index As Long = strXML.IndexOf("<")
            If index > -1 Then
                strXML = strXML.Substring(index)
            End If
            Return strXML
        End Function


        Public Function postData_return_JSON( _
                ByVal strEmail As String, _
                ByVal strURL As String, _
                ByVal HttpData As String, _
                ByVal strMethod As String, _
                ByVal strEncode As String, _
                ByVal strRefererURL As String) As String
            Dim strReturn As String = S_Net.http_post(
                strEmail, strURL,
                HttpData, strMethod, strEncode, strRefererURL)
            Dim index As Long = strReturn.IndexOf("{")
            If index > -1 Then
                strReturn = strReturn.Substring(index)
            Else
                Return ""
            End If
            Return strReturn
        End Function


        ' 忽略证书认证错误处理的函数
        Private Function OnRemoteCertificateValidationCallback(
                  ByVal sender As Object,
                  ByVal certificate As X509Certificate,
                  ByVal chain As X509Chain,
                  ByVal sslPolicyErrors As SslPolicyErrors
                ) As Boolean
            Return True  ' 认证正常，没有错误
        End Function



        Public Function WebResponseGet(webRequest As HttpWebRequest, pEncoding As Text.Encoding) As String

            Dim responseReader As StreamReader = Nothing
            Dim responseData As String = ""
            Try
                responseReader = New StreamReader(webRequest.GetResponse().GetResponseStream(), pEncoding)
                responseData = responseReader.ReadToEnd()
            Catch
                Return "连接错误"

            Finally
                'webRequest.GetResponse().GetResponseStream().Close()
                responseReader.Close()
                responseReader = Nothing
            End Try
            Return responseData
        End Function


        Public Function https_get(
                ByVal strEmail As String,
                ByVal strURL As String,
                ByVal HttpData As String,
                ByVal strMethod As String,
                ByVal strEncode As String,
                ByVal strRefererURL As String,
                Optional ByVal bBinary As Boolean = False,
                Optional ByVal strFile As String = "",
                Optional ByVal bReturnError As Boolean = True,
                Optional ByVal bGzip As Boolean = False) As String

            'ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls  'SecurityProtocolType.Tls Or SecurityProtocolType.Ssl3
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 ';
            ServicePointManager.ServerCertificateValidationCallback =
                New RemoteCertificateValidationCallback(AddressOf OnRemoteCertificateValidationCallback)


            Dim req As HttpWebRequest = WebRequest.CreateDefault(New Uri(strURL))
            req.Method = strMethod '"GET"
            Dim strContentType As String = "application/x-www-form-urlencoded"
            req.ContentType = strContentType
            req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.96 Safari/537.36"


            req.Timeout = 40000 '设置超时 
            req.ReadWriteTimeout = 100000

            Dim Str As String = ""
            Try
                If strEncode = "" Then strEncode = "UTF-8"
                Dim pEncoding As Text.Encoding = Text.Encoding.GetEncoding(strEncode)
                Str = WebResponseGet(req, pEncoding)
            Catch ex As Exception

            End Try

            Return Str

        End Function



        Public Function http_post_with_head(
                ByVal strEmail As String,
                ByVal strURL As String,
                ByVal HttpData As String,
                ByVal strMethod As String,
                ByVal strEncode As String,
                ByVal strRefererURL As String,
                ByVal strHead_Key As String,
                ByVal strHead_Value As String,
                Optional ByVal bRaw As Boolean = False,
                Optional ByVal bBinary As Boolean = False,
                Optional ByVal strFile As String = "",
                Optional ByVal bReturnError As Boolean = True,
                Optional ByVal bGzip As Boolean = False) As String


            If S_Net.bInit = False Then
                S_Net.init()
            End If
            If strEncode = "" Then strEncode = "UTF-8"
            Dim pEncoding As Text.Encoding = Text.Encoding.GetEncoding(strEncode)

            Dim strContentType As String = "application/x-www-form-urlencoded"
            'System.Net.ServicePointManager.Expect100Continue = False
            Dim req As HttpWebRequest

            If pEncoding Is Nothing Then
                pEncoding = Encoding.UTF8
            End If
            Try
                req = CType(WebRequest.Create(strURL), HttpWebRequest)
            Catch ex As Exception
                Return ex.ToString
            End Try
            req.Method = strMethod
            If bRaw = False Then
                req.ContentType = strContentType
            End If


            req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.96 Safari/537.36"

            req.Headers.Add(strHead_Key, strHead_Value) ' "Accept-Charset", "GB2312,utf-8;q=0.7,*;q=0.7")
            If strRefererURL <> "" Then
                req.Referer = strRefererURL
            End If

            req.Timeout = 40000 '设置超时 
            req.ReadWriteTimeout = 100000

            Dim pCookieCurrent As CookieContainer = pTreapCookie.find(New C_K_Str(strEmail))
            If pCookieCurrent Is Nothing Then
                pCookieCurrent = New CookieContainer()
                pTreapCookie.insert(New C_K_Str(strEmail), pCookieCurrent)
            End If
            req.CookieContainer = pCookieCurrent
            req.ServicePoint.Expect100Continue = False

            If bProxy Then
                Dim oProxy As New WebProxy(strProxyURL, iProxyPort)
                If strProxyDomain <> "" Then
                    oProxy.Credentials = New NetworkCredential(strProxyName, strProxyPassword, strProxyDomain)
                Else
                    oProxy.Credentials = New NetworkCredential(strProxyName, strProxyPassword)
                End If
                req.Proxy = oProxy
            End If

            Dim respHTML As String = ""
            Try
                Dim postStream As Stream = Nothing
                If UCase(strMethod) = "POST" Then
                    req.AllowAutoRedirect = True
                    req.Method = "POST"
                    Dim bytesData() As Byte = pEncoding.GetBytes(HttpData)
                    req.ContentLength = bytesData.Length
                    postStream = req.GetRequestStream()
                    postStream.Write(bytesData, 0, bytesData.Length)   '以上向服务器post信息。 
                Else
                    'req.GetResponse()
                End If
                'req.ProtocolVersion = HttpVersion.Version10 ' reference from 
                'req.AllowAutoRedirect = False
                'req.AllowWriteStreamBuffering = False
                'req.KeepAlive = False


                Dim rep As HttpWebResponse = CType(req.GetResponse(), HttpWebResponse)
                Dim pStream As Stream = rep.GetResponseStream

                If bBinary Then
                    '如果是二进制
                    Dim inBuf As Byte() = New Byte(0 To 1024 * 1024 * 20 - 1) {}
                    Dim bytesToRead As Integer = CInt(inBuf.Length)
                    Dim bytesRead As Integer = 0
                    Dim Pos As Integer = 0

                    Dim fstr As New FileStream(strFile, FileMode.OpenOrCreate, FileAccess.Write)
                    Dim bFinished As Boolean = False
                    Do
                        Do While bytesToRead > 0
                            Dim n As Integer = pStream.Read(inBuf, bytesRead, bytesToRead)
                            If n = 0 Then
                                bFinished = True
                                Exit Do
                            End If
                            bytesRead += n
                            bytesToRead -= n
                        Loop
                        fstr.Write(inBuf, Pos, bytesRead)
                        Pos += bytesRead
                        bytesRead = 0
                    Loop Until bFinished

                    fstr.Close()
                Else
                    If bGzip Then
                        Dim gzip As New GZipStream(pStream, CompressionMode.Decompress)
                        Dim reader2 As New StreamReader(gzip, pEncoding)
                        respHTML = reader2.ReadToEnd()
                        reader2.Close()
                    Else
                        Dim reader As StreamReader = New StreamReader(pStream, pEncoding) ' System.Text.Encoding.UTF8)
                        respHTML = reader.ReadToEnd
                        reader.Close()
                    End If
                    If strFile <> "" Then
                        S_File_Text.WriteContent(strFile, respHTML, False)
                    End If
                End If

                If UCase(strMethod) = "POST" Then
                    If Not postStream Is Nothing Then
                        postStream.Close()
                    End If
                End If

                pStream.Close()
                rep.Close()
                req.Abort()

            Catch ex As Exception
                If bReturnError Then
                    respHTML = "网络连接错误：请重试! " &
                        ex.Message & "<br>" &
                        ex.Source & "<br>" &
                        ex.StackTrace
                Else
                    respHTML = ""
                End If
            End Try

            Return respHTML '这就是向网络服务器post后返回的信息
        End Function


        Public Function http_post(
                ByVal strEmail As String,
                ByVal strURL As String,
                ByVal HttpData As String,
                ByVal strMethod As String,
                ByVal strEncode As String,
                ByVal strRefererURL As String,
                Optional ByVal bRaw As Boolean = False,
                Optional ByVal bBinary As Boolean = False,
                Optional ByVal strFile As String = "",
                Optional ByVal bReturnError As Boolean = True,
                Optional ByVal bGzip As Boolean = False) As String


            If S_Net.bInit = False Then
                S_Net.init()
            End If
            If strEncode = "" Then strEncode = "UTF-8"
            Dim pEncoding As Text.Encoding = Text.Encoding.GetEncoding(strEncode)

            Dim strContentType As String = "application/x-www-form-urlencoded"
            'System.Net.ServicePointManager.Expect100Continue = False
            Dim req As HttpWebRequest

            If pEncoding Is Nothing Then
                pEncoding = Encoding.UTF8
            End If
            Try
                req = CType(WebRequest.Create(strURL), HttpWebRequest)
            Catch ex As Exception
                Return ex.ToString
            End Try
            req.Method = strMethod
            If bRaw = False Then
                req.ContentType = strContentType
            End If


            req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.96 Safari/537.36"

            'req.Headers.Add("Accept-Charset", "GB2312,utf-8;q=0.7,*;q=0.7")
            If strRefererURL <> "" Then
                req.Referer = strRefererURL
            End If

            req.Timeout = 40000 '设置超时 
            req.ReadWriteTimeout = 100000

            Dim pCookieCurrent As CookieContainer = pTreapCookie.find(New C_K_Str(strEmail))
            If pCookieCurrent Is Nothing Then
                pCookieCurrent = New CookieContainer()
                pTreapCookie.insert(New C_K_Str(strEmail), pCookieCurrent)
            End If
            req.CookieContainer = pCookieCurrent
            req.ServicePoint.Expect100Continue = False

            If bProxy Then
                Dim oProxy As New WebProxy(strProxyURL, iProxyPort)
                If strProxyDomain <> "" Then
                    oProxy.Credentials = New NetworkCredential(strProxyName, strProxyPassword, strProxyDomain)
                Else
                    oProxy.Credentials = New NetworkCredential(strProxyName, strProxyPassword)
                End If
                req.Proxy = oProxy
            End If

            Dim respHTML As String = ""
            Try
                Dim postStream As Stream = Nothing
                If UCase(strMethod) = "POST" Then
                    req.AllowAutoRedirect = True
                    req.Method = "POST"
                    Dim bytesData() As Byte = pEncoding.GetBytes(HttpData)
                    req.ContentLength = bytesData.Length
                    postStream = req.GetRequestStream()
                    postStream.Write(bytesData, 0, bytesData.Length)   '以上向服务器post信息。 
                Else
                    'req.GetResponse()
                End If
                'req.ProtocolVersion = HttpVersion.Version10 ' reference from 
                'req.AllowAutoRedirect = False
                'req.AllowWriteStreamBuffering = False
                'req.KeepAlive = False


                Dim rep As HttpWebResponse = CType(req.GetResponse(), HttpWebResponse)
                Dim pStream As Stream = rep.GetResponseStream

                If bBinary Then
                    '如果是二进制
                    Dim inBuf As Byte() = New Byte(0 To 1024 * 1024 * 20 - 1) {}
                    Dim bytesToRead As Integer = CInt(inBuf.Length)
                    Dim bytesRead As Integer = 0
                    Dim Pos As Integer = 0

                    Dim fstr As New FileStream(strFile, FileMode.OpenOrCreate, FileAccess.Write)
                    Dim bFinished As Boolean = False
                    Do
                        Do While bytesToRead > 0
                            Dim n As Integer = pStream.Read(inBuf, bytesRead, bytesToRead)
                            If n = 0 Then
                                bFinished = True
                                Exit Do
                            End If
                            bytesRead += n
                            bytesToRead -= n
                        Loop
                        fstr.Write(inBuf, Pos, bytesRead)
                        Pos += bytesRead
                        bytesRead = 0
                    Loop Until bFinished

                    fstr.Close()
                Else
                    If bGzip Then
                        Dim gzip As New GZipStream(pStream, CompressionMode.Decompress)
                        Dim reader2 As New StreamReader(gzip, pEncoding)
                        respHTML = reader2.ReadToEnd()
                        reader2.Close()
                    Else
                        Dim reader As StreamReader = New StreamReader(pStream, pEncoding) ' System.Text.Encoding.UTF8)
                        respHTML = reader.ReadToEnd
                        reader.Close()
                    End If
                    If strFile <> "" Then
                        S_File_Text.WriteContent(strFile, respHTML, False)
                    End If
                End If

                If UCase(strMethod) = "POST" Then
                    If Not postStream Is Nothing Then
                        postStream.Close()
                    End If
                End If

                pStream.Close()
                rep.Close()
                req.Abort()

            Catch ex As Exception
                If bReturnError Then
                    respHTML = "网络连接错误：请重试! " &
                        ex.Message & "<br>" &
                        ex.Source & "<br>" &
                        ex.StackTrace
                Else
                    respHTML = ""
                End If
            End Try

            Return respHTML '这就是向网络服务器post后返回的信息
        End Function

        Public Function Get_URL_XML(ByVal strURL As String) As String

            Dim strReturn As String = ""
            Dim pXML As XmlDocument = New XmlDocument
            Try
                pXML.Load(strURL)
                strReturn = pXML.OuterXml
            Catch ex As Exception

            End Try

            Return strReturn
        End Function
    End Module
End Namespace