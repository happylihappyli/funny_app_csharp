
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

        Public strWeb_URL As String 'Web ��ַ
        Public strWeb_Name As String 'Web_����

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
            '//һ����Ҫע��������SMTP��������ֻ��Ҫ��¼��֤������From������֤
            '//������263)������͵�¼�õ��û�������Ҳ���ܷ��ţ���ʱ�Ĵ��󷵻���Ϣ�ǣ�
            '//553 ������������
            '//������������������Խ�From������Ϊ������SMTP�������ϵ��ʼ���ַ��
            '//Ȼ��FromName��ReplyTo��Ϊ�����������˻ظ��ĵ�ַ�Ϳ�����

            If InStr(strEmailTo, "@") <= 0 Then
                Return "�Ƿ�Email:" & strEmailTo
            End If

            If strEmailTo = "" Then
                Return "Email��ַΪ��"
            End If

            ESM.RecipientName = strEmailTo     '//�趨�ռ�������
            ESM.RecipientMaxNum_Write = 1

            'ESM.AddAttachment(pSession.pServer.MapPath("/Default.htm"))


            ESM.AddRecipient(strEmailTo)    '//�趨�ռ��˵�ַ��������д����
            '//ESM.AddRecipient(tbCc.Text);//�ٴ�ʹ��AddRecipient���ǳ����ˣ�Ҳ����ֱ�Ӵ���һ���ַ������飬�趨һ���ռ��ˡ�

            If strEmailProxy = "" Then
                Return "strEmailProxy û������"
            End If

            ESM.From = strEmailProxy    '//�趨�����˵�ַ(������д)
            ESM.FromName = "admin"    '//�趨����������

            ESM.Priority_Write = "Normal"     ';//�趨���ȼ�
            ESM.Html = True     '//�趨�����Ƿ�HTML��ʽ��
            ESM.Charset = CharSet
            ESM.Subject = strSubject
            ESM.Body = strBody

            If strURLProxy = "" Then
                Return "strURLProxy û������"
            End If

            ESM.MailDomain = strURLProxy
            '//Ҳ�ɽ���SMTP��Ϣһ��������ɡ�д��
            '//ESM.MailDomain="esmuser:esmpass@smtp.163.com:25";
            ESM.MailServerUserName = strEmailProxy
            ';//�趨SMTP��֤���û���

            ESM.MailServerPassWord = strPasswordProxy
            ';//�趨SMTP��֤������

            'Dim StrReturn As String
            ''//��ʼ����
            'If (ESM.Send()) Then
            '    StrReturn = "���ͳɹ�"
            'Else
            '    StrReturn = "URL:" & strURLProxy & ESM.ErrorMessage + vbCrLf
            'End If
            'ESM = Nothing

            Return "���óɹ���"
        End Function

    End Class
End Namespace

