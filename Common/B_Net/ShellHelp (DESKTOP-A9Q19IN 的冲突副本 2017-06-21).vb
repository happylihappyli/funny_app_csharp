Imports System.IO
Imports System.Text
Imports Tamir.SharpSsh.jsch
Imports B_Debug.Funny

Namespace Funny

    Public Class ShellHelp



        Public Shared Function Upload_File( _
            ByRef pDebug As C_Debug, _
            ByVal strHost As String, _
            ByVal iPort As Int32, _
            ByVal strUser As String, _
            ByVal strUpload_Password As String, _
            ByVal strUpload_File As String, _
            ByVal strUpload_Path As String) As String
            Dim monitor As MyProgressMonitor = New MyProgressMonitor
            monitor.pDebug = pDebug
            monitor.strAction = "上传"

            Dim pUser As MyUserInfo = New MyUserInfo(pDebug)
            pUser.setUserName(strUser)
            pUser.setPassword(strUpload_Password)

            Return Upload(strHost, iPort, pUser, strUpload_File, strUpload_Path, monitor)
        End Function


        Public Shared Sub Download_File( _
            ByRef pDebug As C_Debug, _
            ByVal strHost As String, _
            ByVal iPort As Int32, _
            ByVal strUser As String, _
            ByVal strUpload_Password As String, _
            ByVal FileRemote As String, ByVal LocalPath As String)

            Dim monitor As MyProgressMonitor = New MyProgressMonitor
            monitor.pDebug = pDebug
            monitor.strAction = "下载"

            Dim pUser As New MyUserInfo(pDebug)
            pUser.setUserName(strUser)
            pUser.setPassword(strUpload_Password)

            Download(strHost, iPort, pUser, FileRemote, LocalPath, monitor)
        End Sub

        Public Shared Function Upload(ByVal host As String, ByVal port As Integer, _
                                      ByRef ui As UserInfo_Extend, _
                                      ByVal FileLocal As String, ByVal RemotePath As String, _
                                      ByRef monitor As SftpProgressMonitor) As String
            Try
                Dim jsch As New Tamir.SharpSsh.jsch.JSch()

                'String user = host.Substring(0, host.IndexOf('@'));
                'host = host.Substring(host.IndexOf('@') + 1);

                Dim session As Tamir.SharpSsh.jsch.Session = jsch.getSession(ui.getUserName(), host, port)
                session.setUserInfo(ui)

                session.connect()

                Dim channel As Tamir.SharpSsh.jsch.Channel = session.openChannel("sftp")
                channel.connect()
                Dim c As ChannelSftp = DirectCast(channel, ChannelSftp)

                'Stream ins = Console.OpenStandardInput();
                'TextWriter outs = Console.Out;

                Dim cmds As New ArrayList()
                Dim buf As Byte() = New Byte(1024) {}

                c.cd(RemotePath)

                If True Then
                    'String p1 = FileLocal;// (String)cmds[1];
                    Dim p2 As [String] = "."
                    c.put(FileLocal, p2, monitor, ChannelSftp.OVERWRITE)

                End If
                session.disconnect()
                Return ""
            Catch e As Exception
                Return e.ToString()
            End Try
        End Function

        Public Shared Function Download(ByVal host As String, _
                                        ByVal port As Integer, _
                                        ByRef ui As UserInfo_Extend, _
                                        ByVal FileRemote As [String], _
                                        ByVal LocalPath As [String], _
                                        ByRef monitor As SftpProgressMonitor) As String
            Try
                Dim jsch As New Tamir.SharpSsh.jsch.JSch()

                Dim session As Session = jsch.getSession(ui.getUserName(), host, port)
                session.setUserInfo(ui)

                session.connect()

                Dim channel As Channel = session.openChannel("sftp")
                channel.connect()
                Dim c As ChannelSftp = DirectCast(channel, ChannelSftp)

                'Stream ins = Console.OpenStandardInput();
                'TextWriter outs = Console.Out;

                Dim cmds As New ArrayList()
                Dim buf As Byte() = New Byte(1023) {}
                'int i;
                'String str;
                'int level = 0;

                'c.cd(RemotePath);

                If True Then
                    'String p1 = FileLocal;// (String)cmds[1];
                    'String p2 = ".";
                    c.[get](FileRemote, LocalPath, monitor)
                End If
                session.disconnect()
                Return ""
            Catch e As Exception
                Return e.ToString()
            End Try
        End Function


        Private outputstream As System.IO.MemoryStream = New MemoryStream()
        Private inputstream As Tamir.SharpSsh.SshStream = Nothing
        'Channel channel = null;
        'Session session = null;
        ''' <summary>
        ''' 命令等待标识
        ''' </summary>
        Private waitMark As String = "]#"
        Private waitMark2 As String = "[K"
        ''' <summary>
        ''' 打开连接
        ''' </summary>
        ''' <param name="host"></param>
        ''' <param name="username"></param>
        ''' <param name="pwd"></param>
        Public Function OpenShell(ByVal host As String, _
                                  ByVal iPort As Int32, _
                                  ByVal username As String, ByVal pwd As String) As Boolean
            Try
                '/Redirect standard I/O to the SSH channel
                inputstream = New Tamir.SharpSsh.SshStream(host, iPort, username, pwd)

                '我手动加进去的方法。。为了读取输出信息
                inputstream.set_OutputStream(outputstream)

                Return inputstream IsNot Nothing
            Catch
                Throw
            End Try
        End Function
        ''' <summary>
        ''' 执行命令
        ''' </summary>
        ''' <param name="cmd"></param>
        Public Function Shell(ByVal cmd As String, ByVal pEncoding As Encoding) As Boolean
            If inputstream Is Nothing Then
                Return False
            End If

            Dim initinfo As String = GetAllString(pEncoding)

            inputstream.Write(cmd)
            inputstream.Flush()

            Dim currentinfo As String = GetAllString(pEncoding)
            While currentinfo = initinfo
                System.Threading.Thread.Sleep(100)
                currentinfo = GetAllString(pEncoding)
            End While

            Return True
        End Function

        ''' <summary>
        ''' 获取输出信息
        ''' </summary>
        ''' <returns></returns>
        Public Function GetAllString(ByVal pEncode As Encoding, Optional bWait_Count As Int32 = 30) As String
            'Encoding.UTF8
            Dim outinfo As String = pEncode.GetString(outputstream.ToArray())
            '等待命令结束字符
            Dim iCount As Int32 = 0
            Do While Not outinfo.Trim().EndsWith(waitMark) _
                AndAlso Not outinfo.Trim().EndsWith(waitMark2) _
                AndAlso Not outinfo.Trim().EndsWith("]$")
                iCount += 1
                If (iCount > bWait_Count) Then
                    Exit Do
                End If
                System.Threading.Thread.Sleep(200)
                outinfo = pEncode.GetString(outputstream.ToArray())
            Loop
            outputstream.Flush()
            Dim strReturn As String = outinfo.ToString()
            strReturn = RegularExpressions.Regex.Replace(strReturn, "\033\[\d+m", "")
            strReturn = RegularExpressions.Regex.Replace(strReturn, "\033\[\d+;\d+m", "")
            strReturn = RegularExpressions.Regex.Replace(strReturn, "\033\[m", "")

            Return strReturn
        End Function

        ''' <summary>
        ''' 关闭连接
        ''' </summary>
        Public Sub Close()
            If inputstream IsNot Nothing Then
                inputstream.Close()
            End If
        End Sub
    End Class

End Namespace