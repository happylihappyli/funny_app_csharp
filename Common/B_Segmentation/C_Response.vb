Imports System
Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.Threading
Imports System.Xml





Namespace FunnyWeber

    Public Class C_Response
        Public ContentType As String
        Public strBody As StringBuilder
        Public ContentEncoding As System.Text.Encoding
        Public strSetCookie As String
        Public statusCode As Int32 = 200
        Public URL As String  '��Դ��ʵ�ʵ�ַ
        Public bContinue As Boolean = False '�Ƿ���Ҫ����

        Public Sub New()
            strBody = New StringBuilder
        End Sub

        Public Sub Write(ByVal strContent As String)
            strBody.Append(strContent)
        End Sub

    End Class
End Namespace