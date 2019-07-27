Imports System.IO
Imports System.Text
Imports Tamir.SharpSsh.jsch
Imports B_Debug.Funny
Imports Tamir.SharpSsh

Namespace Funny

    Public Class ShellHelp



        Public Shared Function Upload_File(
            ByRef pDebug As C_Debug,
            ByVal strHost As String,
            ByVal iPort As Int32,
            ByVal strUser As String,
            ByVal strUpload_Password As String,
            ByVal strFile_Private As String,
            ByVal strUpload_File As String,
            ByVal strUpload_Path As String) As String
            Dim monitor As MyProgressMonitor = New MyProgressMonitor
            monitor.pDebug = pDebug
            monitor.strAction = "上传"

            Dim pUser As MyUserInfo = New MyUserInfo(pDebug)
            pUser.setUserName(strUser)
            pUser.setPassword(strUpload_Password)


            Return Upload(strHost, iPort, pUser, strUpload_File, strUpload_Path, monitor, strFile_Private)
        End Function


        Public Shared Sub Download_File(
            ByRef pDebug As C_Debug,
            ByVal strHost As String,
            ByVal iPort As Int32,
            ByVal strUser As String,
            ByVal strUpload_Password As String,
            ByVal strFile_Private As String,
            ByVal FileRemote As String,
            ByVal LocalPath As String)

            Dim monitor As MyProgressMonitor = New MyProgressMonitor
            monitor.pDebug = pDebug
            monitor.strAction = "下载"

            Dim pUser As New MyUserInfo(pDebug)
            pUser.setUserName(strUser)
            pUser.setPassword(strUpload_Password)

            Download(strHost, iPort, pUser, FileRemote, LocalPath, monitor, strFile_Private)
        End Sub

        Public Shared Function Upload(ByVal host As String, ByVal port As Integer,
                                      ByRef ui As UserInfo_Extend,
                                      ByVal FileLocal As String, ByVal RemotePath As String,
                                      ByRef monitor As SftpProgressMonitor,
                                      ByVal strFile_Private As String) As String
            Try
                Dim jsch As New Tamir.SharpSsh.jsch.JSch()
                If strFile_Private <> "" Then
                    jsch.addIdentity(strFile_Private)
                End If

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

        Public Shared Function Download(ByVal host As String,
                                        ByVal port As Integer,
                                        ByRef ui As UserInfo_Extend,
                                        ByVal FileRemote As [String],
                                        ByVal LocalPath As [String],
                                        ByRef monitor As SftpProgressMonitor,
                                        ByVal strFile_Private As String) As String
            Try
                Dim jsch As New Tamir.SharpSsh.jsch.JSch()
                jsch.addIdentity(strFile_Private)
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


        Public Shared Function Run_Shell(ByVal host As String,
                                  ByVal iPort As Int32,
                                  ByVal username As String,
                                  ByVal pwd As String,
                                  ByVal file_private As String,
                                  ByVal strCommand As String) As String

            Try
                Dim exec As SshExec = New SshExec(host, username, pwd)

                If file_private <> "" Then
                    exec.AddIdentityFile(file_private)
                End If


                Console.Write("Connecting...")
                exec.Connect(iPort)

                Console.WriteLine("OK")
                Dim output As String = exec.RunCommand(strCommand)
                Console.WriteLine(output)
                Console.Write("Disconnecting...")
                exec.Close()
                Console.WriteLine("OK")
                Return output
            Catch e As Exception
                Console.WriteLine(e.Message)
                Return e.Message
            End Try

        End Function

        Private outputstream As MemoryStream = New MemoryStream()
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
        Public Function GetAllString(ByVal pEncode As Encoding) As String
            'Encoding.UTF8
            Dim outinfo As String = pEncode.GetString(outputstream.ToArray())
            '等待命令结束字符
            Dim iCount As Int32 = 0
            Do While True
                If outinfo.Trim().EndsWith(waitMark) Or outinfo.Trim().EndsWith(waitMark2) Then
                    Exit Do
                End If
                iCount += 1
                If (iCount > 10) Then
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