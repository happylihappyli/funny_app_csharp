Imports B_File.Funny

Imports System
Imports System.Text
Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System.Collections
Imports System.Windows.Forms

Imports B_Debug.Funny


Namespace Funny

    Public Class C_ESmtpMail
        Private enter As String = "" & vbCrLf
        Public Charset As String = "GB2312"
        Public From As String = ""
        Public FromName As String = ""
        Public RecipientName As String = ""
        Public StrBody As String = ""
        Private Recipient As Hashtable = New Hashtable
        Private mailserver As String = ""
        Private mailserverport As Integer = 25
        Private username As String = ""
        Private password As String = ""
        Private ESmtp As Boolean = False
        Public Html As Boolean = False
        Private Attachments As System.Collections.ArrayList
        Private priority As String = "Normal"
        Public Subject As String = ""
        Public Body As String = ""
        Private RecipientNum As Integer = 0
        Private recipientmaxnum As Integer = 1
        Private errmsg As String
        Private tc As TcpClient
        Private ns As NetworkStream
        Private ErrCodeHT As Hashtable = New Hashtable
        Private RightCodeHT As Hashtable = New Hashtable

        Private strLogPath As String

        Public Sub New()
            strLogPath = Application.StartupPath & "\Log\"
            S_SYS.InitDir(strLogPath)
            Attachments = New System.Collections.ArrayList
        End Sub

        Public WriteOnly Property MailDomain() As String
            Set(ByVal Value As String)
                Dim maidomain As String = Value.Trim
                Dim tempint As Integer
                If Not (maidomain = "") Then
                    tempint = maidomain.IndexOf("@")
                    If Not (tempint = -1) Then
                        Dim str As String = maidomain.Substring(0, tempint)
                        MailServerUserName = str.Substring(0, str.IndexOf(":"))
                        MailServerPassWord = str.Substring(str.IndexOf(":") + 1, str.Length - str.IndexOf(":") - 1)
                        maidomain = maidomain.Substring(tempint + 1, maidomain.Length - tempint - 1)
                    End If
                    tempint = maidomain.IndexOf(":")
                    If Not (tempint = -1) Then
                        mailserver = maidomain.Substring(0, tempint)
                        mailserverport = System.Convert.ToInt32(maidomain.Substring(tempint + 1, maidomain.Length - tempint - 1))
                    Else
                        mailserver = maidomain
                    End If
                End If
            End Set
        End Property

        Public WriteOnly Property MailDomainPort() As Integer
            Set(ByVal Value As Integer)
                mailserverport = Value
            End Set
        End Property

        Public WriteOnly Property MailServerUserName() As String
            Set(ByVal Value As String)
                If Not (Value.Trim = "") Then
                    username = Value.Trim
                    ESmtp = True
                Else
                    username = ""
                    ESmtp = False
                End If
            End Set
        End Property

        Public WriteOnly Property MailServerPassWord() As String
            Set(ByVal Value As String)
                password = Value
            End Set
        End Property

        Public WriteOnly Property Priority_Write() As String
            Set(ByVal Value As String)
                Select Case Value.ToLower
                    Case "high"
                        priority = "High"
                        ' break 
                    Case "1"
                        priority = "High"
                        ' break 
                    Case "normal"
                        priority = "Normal"
                        ' break 
                    Case "3"
                        priority = "Normal"
                        ' break 
                    Case "low"
                        priority = "Low"
                        ' break 
                    Case "5"
                        priority = "Low"
                        ' break 
                    Case Else
                        priority = "Normal"
                        ' break 
                End Select
            End Set
        End Property

        Public ReadOnly Property ErrorMessage() As String
            Get
                Return errmsg
            End Get
        End Property
        Private logs As String = ""

        Public ReadOnly Property Logs_Read() As String
            Get
                Return logs
            End Get
        End Property

        Private Sub SMTPCodeAdd()
            ErrCodeHT.Add("500", "邮箱地址错误")
            ErrCodeHT.Add("501", "参数格式错误")
            ErrCodeHT.Add("502", "命令不可实现")
            ErrCodeHT.Add("503", "服务器需要SMTP验证")
            ErrCodeHT.Add("504", "命令参数不可实现")
            ErrCodeHT.Add("421", "服务未就绪，关闭传输信道")
            ErrCodeHT.Add("450", "要求的邮件操作未完成，邮箱不可用（例如，邮箱忙）")
            ErrCodeHT.Add("550", "要求的邮件操作未完成，邮箱不可用（例如，邮箱未找到，或不可访问）")
            ErrCodeHT.Add("451", "放弃要求的操作；处理过程中出错")
            ErrCodeHT.Add("551", "用户非本地，请尝试<forward-path>")
            ErrCodeHT.Add("452", "系统存储不足，要求的操作未执行")
            ErrCodeHT.Add("552", "过量的存储分配，要求的操作未执行")
            ErrCodeHT.Add("553", "邮箱名不可用，要求的操作未执行（例如邮箱格式错误）")
            ErrCodeHT.Add("432", "需要一个密码转换")
            ErrCodeHT.Add("534", "认证机制过于简单")
            ErrCodeHT.Add("538", "当前请求的认证机制需要加密")
            ErrCodeHT.Add("454", "临时认证失败")
            ErrCodeHT.Add("530", "需要认证")
            RightCodeHT.Add("220", "服务就绪")
            RightCodeHT.Add("250", "要求的邮件操作完成")
            RightCodeHT.Add("251", "用户非本地，将转发向<forward-path>")
            RightCodeHT.Add("354", "开始邮件输入，以<enter>.<enter>结束")
            RightCodeHT.Add("221", "服务关闭传输信道")
            RightCodeHT.Add("334", "服务器响应验证Base64字符串")
            RightCodeHT.Add("235", "验证成功")
        End Sub

        Private Function Base64Encode(ByVal Code As String, ByVal str As String) As String
            Dim barray As Byte()
            Select Case Code.ToLower
                Case "gb2312"
                    barray = Encoding.Default.GetBytes(str)
                    ' break 
                Case Else
                    barray = Encoding.UTF8.GetBytes(str)
                    ' break 
            End Select
            Return Convert.ToBase64String(barray)
        End Function

        Private Function Base64Decode(ByVal Code As String, ByVal str As String) As String
            Dim barray As Byte()
            barray = Convert.FromBase64String(str)
            Return Encoding.Default.GetString(barray)
        End Function

        Private Function GetStream(ByVal FilePath As String) As String
            Dim FileStr As System.IO.FileStream = New System.IO.FileStream(FilePath, System.IO.FileMode.Open)
            Dim by(0 To Convert.ToInt32(FileStr.Length) - 1) As Byte
            FileStr.Read(by, 0, by.Length)
            FileStr.Close()
            Return (System.Convert.ToBase64String(by))
        End Function

        Public Sub AddAttachment(ByVal path As String)
            Attachments.Add(path)
        End Sub

        Public Function AddRecipient(ByVal str As String) As Boolean
            str = str.Trim
            If str Is Nothing OrElse str = "" OrElse str.IndexOf("@") = -1 Then
                Return True
            End If
            If RecipientNum < recipientmaxnum Then
                Recipient.Add(RecipientNum, str)
                System.Math.Min(System.Threading.Interlocked.Increment(RecipientNum), RecipientNum - 1)
                Return True
            Else
                errmsg += "收件人过多"
                Return False
            End If
        End Function

        Public WriteOnly Property RecipientMaxNum_Write() As Integer
            Set(ByVal Value As Integer)
                recipientmaxnum = Value
            End Set
        End Property

        Public Function AddRecipient(ByVal str As String()) As Boolean
            Dim i As Integer = 0
            While i < str.Length
                If Not AddRecipient(str(i)) Then
                    Return False
                End If
                System.Math.Min(System.Threading.Interlocked.Increment(i), i - 1)
            End While
            Return True
        End Function

        Private Function SendCommand(ByVal str As String) As Boolean
            Dim WriteBuffer As Byte()
            If str Is Nothing OrElse str.Trim = "" Then
                Return True
            End If
            logs += str
            WriteBuffer = Encoding.Default.GetBytes(str)
            Try
                ns.Write(WriteBuffer, 0, WriteBuffer.Length)
            Catch
                errmsg = "网络连接错误"
                Return False
            End Try
            Return True
        End Function

        Private Function RecvResponse() As String
            Dim StreamSize As Integer
            Dim Returnvalue As String = ""
            Dim ReadBuffer(0 To 1024 - 1) As Byte
            Try
                StreamSize = ns.Read(ReadBuffer, 0, ReadBuffer.Length)
            Catch
                errmsg = "网络连接错误"
                Return "false"
            End Try
            If StreamSize = 0 Then
                Return Returnvalue
            Else
                Returnvalue = Encoding.Default.GetString(ReadBuffer).Substring(0, StreamSize)
                logs += Returnvalue
                Return Returnvalue
            End If
        End Function

        Private Function Dialog( _
            ByVal str As String, ByVal errstr As String) As Boolean
            If str Is Nothing OrElse str.Trim = "" Then
                Return True
            End If
            If SendCommand(str) Then
                Dim RR As String = RecvResponse()
                If RR = "false" Then
                    Return False
                End If
                Dim RRCode As String = ""
                Try
                    RRCode = RR.Substring(0, 3)
                Catch
                    S_Debug.LogError(strLogPath, "RRCode:" & RRCode)
                    Return False
                End Try

                If Not (RightCodeHT(RRCode) Is Nothing) Then
                    Return True
                Else
                    If Not (ErrCodeHT(RRCode) Is Nothing) Then
                        errmsg += (RRCode + ErrCodeHT(RRCode).ToString)
                        errmsg += enter
                    Else
                        errmsg += RR
                    End If
                    errmsg += errstr
                    Return False
                End If
            Else
                Return False
            End If
        End Function

        Private Function Dialog( _
            ByVal str As String(), ByVal errstr As String) As Boolean
            Dim i As Integer = 0
            While i < str.Length
                If Not Dialog(str(i), "") Then
                    errmsg += enter
                    errmsg += errstr
                    Return False
                End If
                System.Math.Min(System.Threading.Interlocked.Increment(i), i - 1)
            End While
            Return True
        End Function

        Private Function SendEmail() As Boolean
            Try
                tc = New TcpClient(mailserver, mailserverport)
            Catch e As Exception
                errmsg = e.ToString
                Return False
            End Try
            ns = tc.GetStream
            SMTPCodeAdd()

            Try
                If RightCodeHT(RecvResponse.Substring(0, 3)) Is Nothing Then
                    errmsg = "网络连接失败"
                    Return False
                End If
            Catch ex As Exception
                errmsg = "网络连接失败"
                Return False
            End Try

            Dim SendBuffer As String()
            Dim SendBufferstr As String
            If ESmtp Then
                SendBuffer = New String(4) {}
                SendBuffer(0) = "EHLO " + mailserver + enter
                SendBuffer(1) = "AUTH LOGIN" + enter
                SendBuffer(2) = Base64Encode(Charset, username) + enter
                SendBuffer(3) = Base64Encode(Charset, password) + enter
                If Not Dialog(SendBuffer, "SMTP服务器验证失败，请核对用户名和密码。") Then
                    Return False
                End If
            Else
                SendBufferstr = "HELO " + mailserver + enter
                If Not Dialog(SendBufferstr, "") Then
                    Return False
                End If
            End If
            SendBufferstr = "MAIL FROM:<" + From + ">" + enter
            If Not Dialog(SendBufferstr, "发件人地址错误，或不能为空") Then
                Return False
            End If
            SendBuffer = New String(recipientmaxnum) {}
            Dim i As Integer = 0
            While i < Recipient.Count
                SendBuffer(i) = "RCPT TO:<" + Recipient(i).ToString + ">" + enter
                System.Math.Min(System.Threading.Interlocked.Increment(i), i - 1)
            End While
            If Not Dialog(SendBuffer, "收件人地址有误") Then
                Return False
            End If
            SendBufferstr = "DATA" + enter
            If Not Dialog(SendBufferstr, "") Then
                Return False
            End If
            SendBufferstr = "From:" + FromName + "<" + From + ">" + enter
            SendBufferstr += "To:=?" + Charset.ToUpper + "?B?" + Base64Encode(Charset, RecipientName) + "?=" + "<" + Recipient(0) + ">" + enter
            SendBufferstr += "CC:"
            i = 0
            While i < Recipient.Count
                SendBufferstr += Recipient(i).ToString + "<" + Recipient(i).ToString + ">,"
                System.Math.Min(System.Threading.Interlocked.Increment(i), i - 1)
            End While

            SendBufferstr += enter
            If Charset = "" Then
                SendBufferstr += "Subject:" + Subject + enter
            Else
                SendBufferstr += "Subject:" + "=?" + Charset.ToUpper + "?B?" + Base64Encode(Charset, Subject) + "?=" + enter
            End If
            SendBufferstr += "MIME-Version: 1.0" + enter
            SendBufferstr += "Content-Type: multipart/mixed;" + enter
            SendBufferstr += "" & Microsoft.VisualBasic.Chr(9) & "boundary=""----=_NextPart_000_00D6_01C29593.AAB31770""" + enter
            SendBufferstr += "X-Priority:" + priority + enter
            SendBufferstr += "X-MSMail-Priority:" + priority + enter
            SendBufferstr += "Importance:" + priority + enter
            SendBufferstr += "X-Mailer: FunnyWeber.Com" + enter
            SendBufferstr += enter
            SendBufferstr += "This is a multi-part message in MIME format." + enter
            SendBufferstr += enter
            SendBufferstr += "------=_NextPart_000_00D6_01C29593.AAB31770" + enter
            If Html Then
                SendBufferstr += "Content-Type: text/html;" + enter
            Else
                SendBufferstr += "Content-Type: text/plain;" + enter
            End If
            If Charset = "" Then
                SendBufferstr &= "" & Microsoft.VisualBasic.Chr(9) & "charset=""iso-8859-1""" & enter
            Else
                SendBufferstr &= "" & Microsoft.VisualBasic.Chr(9) & "charset=""" & Charset.ToLower & """" & enter
            End If
            SendBufferstr += "Content-Transfer-Encoding: base64" + enter + enter
            SendBufferstr += Base64Encode(Charset, Body) + enter
            If Not (Attachments.Count = 0) Then
                For Each filepath As String In Attachments
                    SendBufferstr += "------=_NextPart_000_00D6_01C29593.AAB31770" + enter
                    SendBufferstr += "Content-Type: application/octet-stream" + enter
                    SendBufferstr += "" & Microsoft.VisualBasic.Chr(9) & "name=""=?" + Charset.ToUpper + "?B?" + Base64Encode(Charset, filepath.Substring(filepath.LastIndexOf("\") + 1)) + "?=""" + enter
                    SendBufferstr += "Content-Transfer-Encoding: base64" + enter
                    SendBufferstr += "Content-Disposition: attachment;" + enter
                    SendBufferstr += "" & Microsoft.VisualBasic.Chr(9) & "filename=""=?" + Charset.ToUpper + "?B?" + Base64Encode(Charset, filepath.Substring(filepath.LastIndexOf("\") + 1)) + "?=""" + enter + enter
                    SendBufferstr += GetStream(filepath) + enter + enter
                Next
            End If
            SendBufferstr += "------=_NextPart_000_00D6_01C29593.AAB31770--" + enter + enter
            SendBufferstr += enter + "." + enter
            If Not Dialog(SendBufferstr, "错误信件信息") Then
                Return False
            End If
            SendBufferstr = "QUIT" + enter
            If Not Dialog(SendBufferstr, "断开连接时错误") Then
                Return False
            End If
            ns.Close()
            tc.Close()
            Return True
        End Function

        Public Function Send() As Boolean
            If Recipient.Count = 0 Then
                errmsg = "收件人列表不能为空"
                Return False
            End If
            If mailserver.Trim = "" Then
                errmsg = "必须指定SMTP服务器"
                Return False
            End If
            Return SendEmail()
        End Function

        'Public Function Send(ByVal smtpserver As String) As Boolean
        '    MailDomain = smtpserver
        '    Return Send
        'End Function

        'Public Function Send(ByVal smtpserver As String, ByVal from As String, ByVal fromname As String, ByVal strTo As String, ByVal toname As String, ByVal html As Boolean, ByVal subject As String, ByVal body As String) As Boolean
        '    MailDomain = smtpserver
        '    from = from
        '    fromname = fromname
        '    AddRecipient(strTo)
        '    RecipientName = toname
        '    html = html
        '    subject = subject
        '    body = body
        '    Return Send
        'End Function
    End Class
End Namespace
