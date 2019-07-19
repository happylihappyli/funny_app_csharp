Imports System.IO


Namespace Funny
    Public Module S_SYS

        Public Function ExecuteCmd(ByVal cmd As String) As String

            Dim startInfo As New ProcessStartInfo("cmd.exe")
            With startInfo
                '.Arguments = "/C " '+ cmd
                .RedirectStandardInput = True
                .RedirectStandardError = True
                .RedirectStandardOutput = True
                .UseShellExecute = False
                .CreateNoWindow = True
                .WindowStyle = ProcessWindowStyle.Hidden
            End With
            Dim strSplit() As String = Split(cmd, vbCrLf)
            Dim p As Process = Process.Start(startInfo)
            For i As Int32 = 0 To strSplit.Length - 1
                p.StandardInput.WriteLine(strSplit(i))
            Next
            p.StandardInput.WriteLine("exit")

            Dim strOutput As String = p.StandardOutput.ReadToEnd()
            Dim strError As String = p.StandardError.ReadToEnd()
            p.WaitForExit()
            If (strOutput.Length <> 0) Then
                Return strOutput
            ElseIf (strError.Length <> 0) Then
                Return strError
            End If
            Return ""
        End Function

        Public Function InitDir(ByVal strFile As String) As String
            Dim StrDir As String, Index As Long
            strFile = Replace(strFile, "/", "\")
            Index = InStrRev(strFile, "\")
            StrDir = Left(strFile, Index)
            Try
                If Directory.Exists(StrDir) = False Then
                    Directory.CreateDirectory(StrDir)
                End If
                Return True
            Catch ex As Exception
                Return ex.Message
            End Try

        End Function

        Public Function RunFile(ByVal strFile As String, Optional ByVal strParam As String = "")
            'Dim strFile As String
            Try
                System.Diagnostics.Process.Start(strFile, strParam)
                Return ""
            Catch ex As Exception
                Return ex.ToString()
            End Try
        End Function

    End Module
End Namespace