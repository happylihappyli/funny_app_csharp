
Imports System
Imports System.IO
Imports System.Configuration
Imports System.Data
Imports System.Collections
Imports System.Xml
Imports System.Net
Imports System.Web
Imports System.Text
Imports System.Text.RegularExpressions
Imports B_Data.Funny



Namespace Funny
    Public Class C_Email
        Inherits ThreadBase

        Public strWeb_URL As String 'Web 地址
        Public strWeb_Name As String 'Web_名称

        Dim ESM As C_ESmtpMail = New C_ESmtpMail() 'Funny_SMTP.

        Protected Overrides Sub Run()
            ESM.Send()
        End Sub

        Public Function setEmail( _
            ByVal strEmailTo As String, _
            ByVal strSubject As String, _
            ByVal strBody As String, _
            ByVal strURLProxy As String, _
            ByVal strEmailProxy As String, _
            ByVal strPasswordProxy As String, _
            Optional ByVal CharSet As String = "utf-8") As String
            '//一个重要注意事项：许多SMTP服务器不只需要登录验证，还对From作了验证
            '//（比如263)，如果和登录用的用户名不符也不能发信，这时的错误返回信息是；
            '//553 邮箱名不可用
            '//碰到这样的情况您可以将From属性设为您所用SMTP服务器上的邮件地址。
            '//然后将FromName和ReplyTo设为您想让收信人回复的地址就可以了

            If InStr(strEmailTo, "@") <= 0 Then
                Return "非法Email:" & strEmailTo
            End If

            If strEmailTo = "" Then
                Return "Email地址为空"
            End If

            ESM.RecipientName = strEmailTo     '//设定收件人姓名
            ESM.RecipientMaxNum_Write = 1

            'ESM.AddAttachment(pSession.pServer.MapPath("/Default.htm"))


            ESM.AddRecipient(strEmailTo)    '//设定收件人地址（必须填写）。
            '//ESM.AddRecipient(tbCc.Text);//再次使用AddRecipient就是抄送了，也可以直接传入一个字符串数组，设定一组收件人。

            If strEmailProxy = "" Then
                Return "strEmailProxy 没有设置"
            End If

            ESM.From = strEmailProxy    '//设定发件人地址(必须填写)
            ESM.FromName = "admin"    '//设定发件人姓名

            ESM.Priority_Write = "Normal"     ';//设定优先级
            ESM.Html = True     '//设定正文是否HTML格式。
            ESM.Charset = CharSet
            ESM.Subject = strSubject
            ESM.Body = strBody

            If strURLProxy = "" Then
                Return "strURLProxy 没有设置"
            End If

            ESM.MailDomain = strURLProxy
            '//也可将将SMTP信息一次设置完成。写成
            '//ESM.MailDomain="esmuser:esmpass@smtp.163.com:25";
            ESM.MailServerUserName = strEmailProxy
            ';//设定SMTP验证的用户名

            ESM.MailServerPassWord = strPasswordProxy
            ';//设定SMTP验证的密码

            'Dim StrReturn As String
            ''//开始发送
            'If (ESM.Send()) Then
            '    StrReturn = "发送成功"
            'Else
            '    StrReturn = "URL:" & strURLProxy & ESM.ErrorMessage + vbCrLf
            'End If
            'ESM = Nothing

            Return "设置成功！"
        End Function

    End Class
End Namespace

