<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmProxy
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.txtProxyPort = New System.Windows.Forms.TextBox
        Me.lblProxyPort = New System.Windows.Forms.Label
        Me.txtDomain = New System.Windows.Forms.TextBox
        Me.lblDomain = New System.Windows.Forms.Label
        Me.lblPassword = New System.Windows.Forms.Label
        Me.lblUserName = New System.Windows.Forms.Label
        Me.txtPassword = New System.Windows.Forms.TextBox
        Me.txtUserName = New System.Windows.Forms.TextBox
        Me.txtProxyAddress = New System.Windows.Forms.TextBox
        Me.chkUseProxy = New System.Windows.Forms.CheckBox
        Me.lblProxy = New System.Windows.Forms.Label
        Me.B_Save = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'txtProxyPort
        '
        Me.txtProxyPort.Location = New System.Drawing.Point(285, 52)
        Me.txtProxyPort.Name = "txtProxyPort"
        Me.txtProxyPort.Size = New System.Drawing.Size(48, 20)
        Me.txtProxyPort.TabIndex = 37
        '
        'lblProxyPort
        '
        Me.lblProxyPort.AutoSize = True
        Me.lblProxyPort.Location = New System.Drawing.Point(236, 56)
        Me.lblProxyPort.Name = "lblProxyPort"
        Me.lblProxyPort.Size = New System.Drawing.Size(43, 13)
        Me.lblProxyPort.TabIndex = 36
        Me.lblProxyPort.Text = "端口："
        '
        'txtDomain
        '
        Me.txtDomain.Location = New System.Drawing.Point(93, 118)
        Me.txtDomain.Name = "txtDomain"
        Me.txtDomain.Size = New System.Drawing.Size(136, 20)
        Me.txtDomain.TabIndex = 35
        '
        'lblDomain
        '
        Me.lblDomain.AutoSize = True
        Me.lblDomain.Location = New System.Drawing.Point(13, 122)
        Me.lblDomain.Name = "lblDomain"
        Me.lblDomain.Size = New System.Drawing.Size(22, 13)
        Me.lblDomain.TabIndex = 34
        Me.lblDomain.Text = "域:"
        '
        'lblPassword
        '
        Me.lblPassword.AutoSize = True
        Me.lblPassword.Location = New System.Drawing.Point(13, 100)
        Me.lblPassword.Name = "lblPassword"
        Me.lblPassword.Size = New System.Drawing.Size(34, 13)
        Me.lblPassword.TabIndex = 33
        Me.lblPassword.Text = "密码:"
        '
        'lblUserName
        '
        Me.lblUserName.AutoSize = True
        Me.lblUserName.Location = New System.Drawing.Point(13, 78)
        Me.lblUserName.Name = "lblUserName"
        Me.lblUserName.Size = New System.Drawing.Size(46, 13)
        Me.lblUserName.TabIndex = 32
        Me.lblUserName.Text = "用户名:"
        '
        'txtPassword
        '
        Me.txtPassword.Location = New System.Drawing.Point(93, 96)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPassword.Size = New System.Drawing.Size(136, 20)
        Me.txtPassword.TabIndex = 31
        '
        'txtUserName
        '
        Me.txtUserName.Location = New System.Drawing.Point(93, 74)
        Me.txtUserName.Name = "txtUserName"
        Me.txtUserName.Size = New System.Drawing.Size(136, 20)
        Me.txtUserName.TabIndex = 30
        '
        'txtProxyAddress
        '
        Me.txtProxyAddress.Location = New System.Drawing.Point(93, 52)
        Me.txtProxyAddress.Name = "txtProxyAddress"
        Me.txtProxyAddress.Size = New System.Drawing.Size(136, 20)
        Me.txtProxyAddress.TabIndex = 29
        '
        'chkUseProxy
        '
        Me.chkUseProxy.Location = New System.Drawing.Point(93, 21)
        Me.chkUseProxy.Name = "chkUseProxy"
        Me.chkUseProxy.Size = New System.Drawing.Size(58, 16)
        Me.chkUseProxy.TabIndex = 28
        Me.chkUseProxy.Text = "代理"
        '
        'lblProxy
        '
        Me.lblProxy.AutoSize = True
        Me.lblProxy.Location = New System.Drawing.Point(13, 56)
        Me.lblProxy.Name = "lblProxy"
        Me.lblProxy.Size = New System.Drawing.Size(65, 13)
        Me.lblProxy.TabIndex = 27
        Me.lblProxy.Text = "代理URL："
        '
        'B_Save
        '
        Me.B_Save.Location = New System.Drawing.Point(239, 174)
        Me.B_Save.Name = "B_Save"
        Me.B_Save.Size = New System.Drawing.Size(94, 23)
        Me.B_Save.TabIndex = 38
        Me.B_Save.Text = "Save"
        Me.B_Save.UseVisualStyleBackColor = True
        '
        'FrmProxy
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(363, 231)
        Me.Controls.Add(Me.B_Save)
        Me.Controls.Add(Me.txtProxyPort)
        Me.Controls.Add(Me.lblProxyPort)
        Me.Controls.Add(Me.txtDomain)
        Me.Controls.Add(Me.lblDomain)
        Me.Controls.Add(Me.lblPassword)
        Me.Controls.Add(Me.lblUserName)
        Me.Controls.Add(Me.txtPassword)
        Me.Controls.Add(Me.txtUserName)
        Me.Controls.Add(Me.txtProxyAddress)
        Me.Controls.Add(Me.chkUseProxy)
        Me.Controls.Add(Me.lblProxy)
        Me.Name = "FrmProxy"
        Me.Text = "FrmProxy"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtProxyPort As System.Windows.Forms.TextBox
    Friend WithEvents lblProxyPort As System.Windows.Forms.Label
    Friend WithEvents txtDomain As System.Windows.Forms.TextBox
    Friend WithEvents lblDomain As System.Windows.Forms.Label
    Friend WithEvents lblPassword As System.Windows.Forms.Label
    Friend WithEvents lblUserName As System.Windows.Forms.Label
    Friend WithEvents txtPassword As System.Windows.Forms.TextBox
    Friend WithEvents txtUserName As System.Windows.Forms.TextBox
    Friend WithEvents txtProxyAddress As System.Windows.Forms.TextBox
    Friend WithEvents chkUseProxy As System.Windows.Forms.CheckBox
    Friend WithEvents lblProxy As System.Windows.Forms.Label
    Friend WithEvents B_Save As System.Windows.Forms.Button
End Class
