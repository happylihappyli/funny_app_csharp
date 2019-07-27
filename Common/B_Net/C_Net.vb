Imports B_Net.Funny
Imports B_String.Funny

Namespace Funny
    Public Class C_Net

        Public Delegate Sub ReadFromURL_CallBack(ByVal bError As Boolean, ByVal strType As String, ByVal strReturn As String)

        Public UserName As String
        Public UserMD5 As String

        Public strURL As String
        Public strType As String
        Public strData As String
        Public pCallBack As C_Net.ReadFromURL_CallBack


        Public Sub ReadFromURL(ByVal strURL As String, _
                               ByVal strType As String, _
                               ByVal strData As String, _
                               ByVal bSendUserInfo As Boolean, _
                               ByRef pCallBack As C_Net.ReadFromURL_CallBack)
            Dim pNet As C_Net = New C_Net
            If bSendUserInfo Then
                pNet.UserName = UserName
                pNet.UserMD5 = UserMD5
            End If
            pNet.strType = strType
            pNet.strData = strData
            pNet.strURL = strURL
            pNet.pCallBack = pCallBack
            Dim p As Threading.Thread = New Threading.Thread(AddressOf pNet.PostToWeb)
            p.Start()
        End Sub

        Public Sub PostToWeb()
            Dim strReturn As String = ""
            Try
                strData &= "&UserName=" & S_Strings.UrlEncode(UserName) & "&UserMD5=" & S_Strings.UrlEncode(UserMD5)

                strReturn = S_Net.http_post(UserName, strURL, strData, "POST", "utf-8", "")


                Dim index As Long = strReturn.IndexOf("<")
                If index > -1 Then strReturn = strReturn.Substring(index)


                pCallBack.Invoke(False, strType, strReturn)
            Catch ex As Exception
                'pCallBack.Invoke(True, strType, ex.ToString)
                MsgBox(ex.ToString)
            End Try

        End Sub

        ''' <summary>
        ''' 获取文件类型后缀，如 gif,png,....
        ''' </summary>
        ''' <param name="strURL"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetFileType(ByVal strURL As String) As String
            Dim strSplit() As String = Split(strURL, ".")
            Dim strFileType As String = strSplit(strSplit.Length - 1)
            Return strFileType
        End Function
    End Class
End Namespace
