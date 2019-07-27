Imports Tamir.SharpSsh.jsch
Imports B_Debug.Funny

Public Class MyProgressMonitor
    Inherits SftpProgressMonitor

    Public pDebug As C_Debug

    'Public pFrmMain As FrmMain

    Public strAction As String = "上传"
    Private c As Long = 0
    Private max As Long = 0
    Private percent As Long = -1
    Private elapsed As Integer = -1

    Private timer As System.Timers.Timer

    Public Overrides Sub Init(ByVal op As Integer, ByVal src As [String], ByVal dest As [String], ByVal max As Long)
        'bar = New ConsoleProgressBar()
        Me.max = max
        elapsed = 0
        timer = New System.Timers.Timer(1000)
        timer.Start()
        AddHandler timer.Elapsed, New System.Timers.ElapsedEventHandler(AddressOf timer_Elapsed)
        'pFrmMain.ShowProgressBar(1)
        'pFrmMain.ShowMsg("开始" & strAction)

        If pDebug IsNot Nothing Then
            pDebug.WriteLine("开始" & strAction)
        End If
    End Sub

    Public Overrides Function Count(ByVal c As Long) As Boolean
        Me.c += c
        If percent >= Me.c * 100 \ max Then
            Return True
        End If
        percent = Me.c * 100 \ max

        'pFrmMain.ShowProgressBar(percent)
        If pDebug IsNot Nothing Then
            pDebug.WriteLine(percent)
        End If
        Console.Write(percent & ".")
        Return True
    End Function


    Private Sub timer_Elapsed(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs)
        Me.elapsed += 1
    End Sub

    Public Overrides Sub [End]()

        timer.[Stop]()
        timer.Dispose()

        If pDebug IsNot Nothing Then
            pDebug.WriteLine(strAction & "完毕！耗时" & elapsed & " 秒!")
        End If
    End Sub
End Class