
Imports B_String.Funny

Imports System
Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.Threading
Imports System.Xml
Imports B_Data.Funny



Namespace FunnyWeber

    Public Class C_Request
        Public Class C_WebServer_Variable
            Public Key As String
            Public Value As String
        End Class

        'zzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz
        Public RawUrl As String '原始的URL
        'zzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz

        Public context As HttpListenerContext
        Public Host As String
        Public HostURL As String
        Public HostPost As Int32

        Public ByteFrom As Int32
        Public ByteTo As Int32

        Public QueryString As New C_Treap_Funny(Of C_WebServer_Variable)
        Public Session As New C_Treap_Funny(Of C_WebServer_Variable)

        Public RndID As Int32 'Debug 用的一个ID
        Public intLength As Int32 '内容长度
        Public bContinue As Boolean = False

        Public strHead As String ' POST GET
        Public sDirName, sRequestedFile As String
        Public sLocalDir As String
        Public strPostData As String
        Public strSessionID As String

        Public ReadByte() As Byte   '

        Public pSession As C_Session

        Public ClientIP As IPEndPoint


        Public Sub GetHostURL()
            Dim Index As Long
            Index = InStr(Me.Host, ":")

            If Index > 0 Then
                Me.HostURL = Left(Me.Host, Index - 1)
                Me.HostPost = Val(Right(Me.Host, Len(Host) - Index))
            Else
                Me.HostURL = Me.Host
                Me.HostPost = 80
            End If
        End Sub

        Public Sub setValue(ByVal strKey As String, ByVal strValue As String)
            Dim pVariable As New C_WebServer_Variable

            pVariable.Key = strKey
            pVariable.Value = S_Strings.URLDecode(Trim(strValue))

            QueryString.insert(New C_K_Str(pVariable.Key), pVariable)
        End Sub


        Public Function GetValue(ByVal strKey As String) As String
            Dim strValue1 As String = ""

            Dim pVariable As C_WebServer_Variable
            pVariable = QueryString.find(New C_K_Str(strKey))
            If Not pVariable Is Nothing Then
                strValue1 = pVariable.Value
            End If

            Return strValue1
        End Function


        ' ==============================================
        '   Fills the QueryString collection
        ' ==============================================
        Public Sub ExtractHttpGetParams( _
            ByVal strParams As String, _
            Optional ByVal strSplit As String = "&")

            Dim A(), v() As String
            Dim e As Object

            Dim pVariable As C_WebServer_Variable

            A = Split(strParams, strSplit)
            For Each e In A
                v = Split(e, "=")

                pVariable = Me.QueryString.find(New C_K_Str(Trim(v(0))))
                If pVariable Is Nothing Then
                    pVariable = New C_WebServer_Variable

                    pVariable.Key = Trim(v(0))
                    If UBound(v) >= 1 Then
                        pVariable.Value = S_Strings.URLDecode(Trim(v(1)))
                    End If

                    Me.QueryString.insert(New C_K_Str(pVariable.Key), pVariable)
                Else
                    If UBound(v) >= 1 Then
                        pVariable.Value &= "," & Trim(v(1))
                    End If
                End If

                If pVariable.Key = "ASP.NET_SessionId" Then
                    Me.strSessionID = pVariable.Value
                End If
            Next


        End Sub
    End Class
End Namespace