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
        Public URL As String  '资源的实际地址
        Public bContinue As Boolean = False '是否还需要继续

        Public Sub New()
            strBody = New StringBuilder
        End Sub

        Public Sub Write(ByVal strContent As String)
            strBody.Append(strContent)
        End Sub

    End Class
End Namespace