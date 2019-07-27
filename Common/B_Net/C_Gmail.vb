
Imports B_File.Funny
Imports B_Data.Funny
Imports B_Debug.Funny

Imports System.Net.Mail
Imports System.Windows.Forms


Namespace Funny

    Public Class C_Gmail
        Inherits ThreadBase

        Dim message As New MailMessage
        Dim strMailFrom As String
        Dim strPassword As String
        Public strReturn As String

        Public Sub addEmail(ByVal strEmail As String)
            message.To.Add(New MailAddress(strEmail)) ' // the email you want 
        End Sub

        Public Sub New( _
                ByVal strMailFrom As String, _
                ByVal strPassword As String, _
                ByVal strMailTO As String, _
                ByVal strSubject As String, _
                ByVal strBody As String)



            If strMailTO.IndexOf("@") = -1 Then Exit Sub
            If strMailFrom.IndexOf("@") = -1 Then Exit Sub

            Me.strMailFrom = strMailFrom
            Me.strPassword = strPassword

            message.From = New MailAddress(strMailFrom) ' 发信人
            message.To.Add(New MailAddress(strMailTO)) 'Email收信人
            '//to send email to 
            message.Subject = strSubject.Replace(vbCr, "").Replace(vbLf, "")

            message.IsBodyHtml = True
            message.BodyEncoding = System.Text.Encoding.UTF8
            message.Body = strBody ' "this is just a simple test!<br> Jack"
            message.Priority = MailPriority.High
        End Sub

        Protected Overrides Sub Run()
            Dim client As New SmtpClient("smtp.gmail.com", 587)
            '//Gmail使用的端口 
            client.Credentials = _
            New System.Net.NetworkCredential(strMailFrom, strPassword) '// Your user 
            '//name & password 
            client.EnableSsl = True '; //经过ssl加密 
            Dim userState As Object = message

            Try
                client.Send(message)
                strReturn = "邮件发送到" + message.To.ToString()
            Catch ex As Exception
                strReturn = ex.ToString
            End Try

            S_Debug.LogError(Application.StartupPath & "\Log\Log.txt", strReturn)

            Debug.Print(strReturn)
        End Sub
    End Class

End Namespace