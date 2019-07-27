Imports B_Net.Funny
Imports B_File.Funny
Imports B_XML.Funny


Public Class FrmProxy

    Private Sub FrmProxy_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim pSet As C_XML_Setting = New C_XML_Setting

        pSet.setFile(Environment.CurrentDirectory & "\Set.xml")

        txtProxyAddress.Text = pSet.Read_Setting("Proxy", "URL")
        txtProxyPort.Text = pSet.Read_Setting("Proxy", "Port")
        txtUserName.Text = pSet.Read_Setting("Proxy", "UserName")
        txtPassword.Text = pSet.Read_Setting("Proxy", "Password")
        txtDomain.Text = pSet.Read_Setting("Proxy", "Domain")
        chkUseProxy.Checked = (pSet.Read_Setting("Proxy", "Used") = "1")

        If pSet.Read_Setting("Proxy", "Used") = "1" Then
            S_Net.bProxy = True
        End If

        S_Net.strProxyURL = pSet.Read_Setting("Proxy", "URL")
        S_Net.iProxyPort = Val(pSet.Read_Setting("Proxy", "Port"))
        S_Net.strProxyName = pSet.Read_Setting("Proxy", "UserName")
        S_Net.strProxyPassword = pSet.Read_Setting("Proxy", "Password")
        S_Net.strProxyDomain = pSet.Read_Setting("Proxy", "Domain")

        pSet = Nothing
    End Sub

    Private Sub B_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles B_Save.Click
        Dim pSet As C_XML_Setting = New C_XML_Setting
        pSet.setFile(Environment.CurrentDirectory & "\Set.xml")

        pSet.Save_Setting("Proxy", "Used", IIf(chkUseProxy.Checked, "1", "0"))
        pSet.Save_Setting("Proxy", "URL", txtProxyAddress.Text)
        pSet.Save_Setting("Proxy", "Port", txtProxyPort.Text)
        pSet.Save_Setting("Proxy", "UserName", txtUserName.Text)
        pSet.Save_Setting("Proxy", "Password", txtPassword.Text)
        pSet.Save_Setting("Proxy", "Domain", txtDomain.Text)

        S_Net.bProxy = chkUseProxy.Checked
        S_Net.strProxyDomain = txtDomain.Text
        S_Net.strProxyName = txtUserName.Text
        S_Net.strProxyPassword = txtPassword.Text
        S_Net.strProxyURL = txtProxyAddress.Text
        S_Net.iProxyPort = Val(txtProxyPort.Text)
        pSet = Nothing
        Me.Close()
    End Sub

    Private Sub chkUseProxy_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkUseProxy.CheckedChanged

        Dim Section As String = ""
        Dim oRegKey As Microsoft.Win32.RegistryKey
        Dim sValue As String = ""
        Dim sProxy() As String
        Dim i As Byte

        'SetupEnabled()

        If chkUseProxy.Checked AndAlso (txtProxyAddress.Text = "") Then
            Section = "Software\Microsoft\Windows\CurrentVersion\Internet Settings"
            oRegKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(Section, False)
            sValue = oRegKey.GetValue("ProxyServer")

            If sValue <> "" Then
                sProxy = Split(sValue, ";")
                If UBound(sProxy) = 0 Then 'If all proxy addresses are the same only one entry is returned ( fw.solar.dk:80 )
                    sValue = sProxy(0)
                ElseIf UBound(sProxy) > 0 Then 'Proxyinfo must be found. ( ftp=fw.solar.dk:80;gopher=fw.solar.dk:80;http=fw.solar.dk:80;https=fw.solar.dk:80;socks=fw.solar.dk:80 )
                    For i = 0 To UBound(sProxy)
                        'Searches and strips unneeded info
                        If (UCase(Mid(sProxy(i), 1, 5)) = ("HTTP=")) Then sValue = Mid(sProxy(i), 6, Len(sProxy(i)))
                    Next i
                Else
                    MsgBox("Proxy server not detected!", vbExclamation, "Not found!")
                    Exit Sub
                End If

                sProxy = Split(sValue, ":") 'Split address and port
                txtProxyAddress.Text = sProxy(0)
                txtProxyPort.Text = sProxy(1)
            Else
                MsgBox("Proxy server not detected!", vbExclamation, "Not found!")
            End If

        End If
    End Sub
End Class