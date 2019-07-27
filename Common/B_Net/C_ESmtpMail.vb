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
            ErrCodeHT.Add("500", "�����ַ����")
            ErrCodeHT.Add("501", "������ʽ����")
            ErrCodeHT.Add("502", "�����ʵ��")
            ErrCodeHT.Add("503", "��������ҪSMTP��֤")
            ErrCodeHT.Add("504", "�����������ʵ��")
            ErrCodeHT.Add("421", "����δ�������رմ����ŵ�")
            ErrCodeHT.Add("450", "Ҫ����ʼ�����δ��ɣ����䲻���ã����磬����æ��")
            ErrCodeHT.Add("550", "Ҫ����ʼ�����δ��ɣ����䲻���ã����磬����δ�ҵ����򲻿ɷ��ʣ�")
            ErrCodeHT.Add("451", "����Ҫ��Ĳ�������������г���")
            ErrCodeHT.Add("551", "�û��Ǳ��أ��볢��<forward-path>")
            ErrCodeHT.Add("452", "ϵͳ�洢���㣬Ҫ��Ĳ���δִ��")
            ErrCodeHT.Add("552", "�����Ĵ洢���䣬Ҫ��Ĳ���δִ��")
            ErrCodeHT.Add("553", "�����������ã�Ҫ��Ĳ���δִ�У����������ʽ����")
            ErrCodeHT.Add("432", "��Ҫһ������ת��")
            ErrCodeHT.Add("534", "��֤���ƹ��ڼ�")
            ErrCodeHT.Add("538", "��ǰ�������֤������Ҫ����")
            ErrCodeHT.Add("454", "��ʱ��֤ʧ��")
            ErrCodeHT.Add("530", "��Ҫ��֤")
            RightCodeHT.Add("220", "�������")
            RightCodeHT.Add("250", "Ҫ����ʼ��������")
            RightCodeHT.Add("251", "�û��Ǳ��أ���ת����<forward-path>")
            RightCodeHT.Add("354", "��ʼ�ʼ����룬��<enter>.<enter>����")
            RightCodeHT.Add("221", "����رմ����ŵ�")
            RightCodeHT.Add("334", "��������Ӧ��֤Base64�ַ���")
            RightCodeHT.Add("235", "��֤�ɹ�")
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
                errmsg += "�ռ��˹���"
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
                errmsg = "�������Ӵ���"
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
                errmsg = "�������Ӵ���"
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
                    errmsg = "��������ʧ��"
                    Return False
                End If
            Catch ex As Exception
                errmsg = "��������ʧ��"
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
                If Not Dialog(SendBuffer, "SMTP��������֤ʧ�ܣ���˶��û��������롣") Then
                    Return False
                End If
            Else
                SendBufferstr = "HELO " + mailserver + enter
                If Not Dialog(SendBufferstr, "") Then
                    Return False
                End If
            End If
            SendBufferstr = "MAIL FROM:<" + From + ">" + enter
            If Not Dialog(SendBufferstr, "�����˵�ַ���󣬻���Ϊ��") Then
                Return False
            End If
            SendBuffer = New String(recipientmaxnum) {}
            Dim i As Integer = 0
            While i < Recipient.Count
                SendBuffer(i) = "RCPT TO:<" + Recipient(i).ToString + ">" + enter
                System.Math.Min(System.Threading.Interlocked.Increment(i), i - 1)
            End While
            If Not Dialog(SendBuffer, "�ռ��˵�ַ����") Then
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
            If Not Dialog(SendBufferstr, "�����ż���Ϣ") Then
                Return False
            End If
            SendBufferstr = "QUIT" + enter
            If Not Dialog(SendBufferstr, "�Ͽ�����ʱ����") Then
                Return False
            End If
            ns.Close()
            tc.Close()
            Return True
        End Function

        Public Function Send() As Boolean
            If Recipient.Count = 0 Then
                errmsg = "�ռ����б���Ϊ��"
                Return False
            End If
            If mailserver.Trim = "" Then
                errmsg = "����ָ��SMTP������"
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
