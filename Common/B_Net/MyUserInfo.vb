Imports Tamir.SharpSsh.jsch.examples '//Tamir.SharpSsh.jsch.examples
Imports Tamir.SharpSsh.jsch

Imports System.Windows.Forms
Imports B_Debug.Funny

Public Class MyUserInfo
    Implements UserInfo_Extend



    Public pDebug As C_Debug

    Private passwd As String
    Private passwordField As New InputForm()
    Private userName As String

    Public Sub New(ByRef pDebug As C_Debug)
        Me.pDebug = pDebug
    End Sub

    Public Function getPassword() As String Implements Tamir.SharpSsh.jsch.UserInfo.getPassword
        Return passwd
    End Function

    Public Sub setPassword(ByVal p As String) Implements Tamir.SharpSsh.jsch.UserInfo.setPassword
        passwd = p
    End Sub

    Public Function getPassphrase() As [String] Implements Tamir.SharpSsh.jsch.UserInfo.getPassphrase
        Return Nothing
    End Function


    Public Sub showMessage(ByVal message As [String]) Implements Tamir.SharpSsh.jsch.UserInfo.showMessage
        pDebug.WriteLine(message)

        'MessageBox.Show(message, "SharpSSH", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
    End Sub

    Public Function promptPassphrase(ByVal message As String) As Boolean Implements Tamir.SharpSsh.jsch.UserInfo.promptPassphrase
        Return True
    End Function

    Public Function promptPassword(ByVal message As String) As Boolean Implements Tamir.SharpSsh.jsch.UserInfo.promptPassword
        Return True

        'Dim inForm As New InputForm()
        'inForm.Text = message
        'inForm.PasswordField = True

        'If inForm.PromptForInput() Then
        '    passwd = inForm.getText()
        '    Return True
        'Else
        '    Return False
        'End If
    End Function

    Public Function promptYesNo(ByVal message As String) As Boolean Implements Tamir.SharpSsh.jsch.UserInfo.promptYesNo
        Dim returnVal As DialogResult = MessageBox.Show(message, "SharpSSH", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
        Return (returnVal = DialogResult.Yes)
    End Function

    Public Function getUserName() As String Implements Tamir.SharpSsh.jsch.UserInfo_Extend.getUserName
        Return Me.userName
    End Function

    Public Sub setUserName(ByVal user As String) Implements Tamir.SharpSsh.jsch.UserInfo_Extend.setUserName
        Me.userName = user
    End Sub
End Class
