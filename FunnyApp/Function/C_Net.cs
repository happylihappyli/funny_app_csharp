using agsXMPP;
using agsXMPP.protocol.client;
using B_Net.Funny;
using Newtonsoft.Json.Linq;
using Renci.SshNet.Sftp;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FunnyApp {
    public class C_Net {

        SecureTransfer st = null;

        
        public FrmApp pFrmApp;

        public C_Net(FrmApp FrmApp) {
            this.pFrmApp = FrmApp;
        }

        
        public enum MessageClass {
            DebugMsg = 0,
            InformationMsg = 1,
            WarningMsg = 2,
            ErrorMsg = 3,
        }

        string hosts = "";
        string user = "";
        string password = "";
        string port = "";
        string dir = "";
        string callback = "";

        public void ftp_list(
            string hosts, 
            string user,string password, 
            string port,
            string dir,string callback) {
            this.hosts = hosts;
            this.user = user;
            this.password = password;
            this.port = port;
            this.dir = dir;
            this.callback = callback;

            Thread p = new Thread(ftp_list_sub);
            p.Start();
        }


        public void ftp_list_sub() { 
            string strReturn = "";
            SecureTransfer.SSHTransferProtocol Protocol = SecureTransfer.SSHTransferProtocol.SFTP;

            try {
                st = new SecureTransfer(hosts, user, password,
                            int.Parse(port), Protocol);

                st.Connect();

                switch (Protocol) {
                    case SecureTransfer.SSHTransferProtocol.SFTP:
                        st.asyncCallback = AsyncCallback;
                        break;
                }

                strReturn= st.List_File(dir); 
            } catch (Exception ex) {
                pFrmApp.JS_Function(this.callback_error, ex.ToString());
                st?.Disconnect();
                st?.Dispose();
            }

            pFrmApp.Call_Event(callback, strReturn);
        }


        XmppClientConnection xmpp ;

        public void xmpp_connect(string hosts,
            string user,string password,
            string event_login,string event_msg){

            xmpp = new XmppClientConnection(hosts);
            xmpp.Open(user,password);
            xmpp.OnLogin += delegate (object sender) {
                pFrmApp.Call_Event(event_login, "");
            };
            xmpp.OnMessage += delegate (object sender, Message msg) {
                pFrmApp.Call_Event(event_msg, "");
            };
        }

        public void xmpp_send(string user,string msg) {
            xmpp.Send(new Message(user,
                    MessageType.chat, msg));
        }


        public string http_post(string url,string data) {
            string strReturn="";
            strReturn=S_Net.http_post("", url, data, "POST", "utf-8", "");
            return strReturn;
        }

        public string callback_status = "";
        public string callback_error = "";
        public void ftp_upload(
            string hosts, string user,
            string password, string port,
            string local_file, string remotefile,
            string callback_status,
            string callback_error) {

            this.callback_status = callback_status;
            this.callback_error = callback_error;
            SecureTransfer.SSHTransferProtocol Protocol = SecureTransfer.SSHTransferProtocol.SFTP;

            try {
                st = new SecureTransfer(hosts, user, password,
                            int.Parse(port), Protocol,
                                        local_file, remotefile);

                st.Connect();

                switch (Protocol) {
                    case SecureTransfer.SSHTransferProtocol.SFTP:
                        st.asyncCallback = AsyncCallback;
                        break;
                }

                var t = new Thread(Start_Upload);
                t.SetApartmentState(ApartmentState.STA);
                t.IsBackground = true;
                t.Start();
            } catch (Exception ex) {
                pFrmApp.JS_Function(this.callback_error, ex.ToString());
                st?.Disconnect();
                st?.Dispose();
            }
        }


        public void ftp_download(
            string hosts, string user,
            string password, string port,
            string source_file, string dest_file,
            string callback_status,
            string callback_error) {

            this.callback_status = callback_status;
            this.callback_error = callback_error;
            SecureTransfer.SSHTransferProtocol Protocol = SecureTransfer.SSHTransferProtocol.SFTP;

            try {
                st = new SecureTransfer(hosts, user, password,
                            int.Parse(port), Protocol,
                            source_file, dest_file);

                st.Connect();

                switch (Protocol) {
                    case SecureTransfer.SSHTransferProtocol.SFTP:
                        st.asyncCallback = AsyncCallback;
                        break;
                }

                var t = new Thread(Start_Download);
                t.SetApartmentState(ApartmentState.STA);
                t.IsBackground = true;
                t.Start();
            } catch (Exception ex) {
                pFrmApp.JS_Function(this.callback_error, ex.ToString());
                st?.Disconnect();
                st?.Dispose();
            }
        }


        private void AsyncCallback(IAsyncResult ar) {
            if ("".Equals(callback_error) == false) {
                pFrmApp.JS_Function(this.callback_error, ar.ToString());
            }
        }


        private void Start_Download() {
            try {
                st.Download();

                if (st.Protocol == SecureTransfer.SSHTransferProtocol.SFTP) {
                    SftpFileAttributes att = st.SftpClt.GetAttributes(st.SrcFile);
                    var fileSize = att.Size;

                    while (!st.async_download_result.IsCompleted) {
                        var max = fileSize > int.MaxValue
                            ? Convert.ToInt32(fileSize / 1024)
                            : Convert.ToInt32(fileSize);

                        var cur = fileSize > int.MaxValue
                            ? Convert.ToInt32(st.async_download_result.DownloadedBytes / 1024)
                            : Convert.ToInt32(st.async_download_result.DownloadedBytes);
                        Show_Progress(cur, max);
                        Thread.Sleep(100);
                    }
                }
                st.End_Download();
                pFrmApp.JS_Function(this.callback_error, "传输完毕");
                st.Disconnect();
                st.Dispose();
            } catch (Exception ex) {
                pFrmApp.JS_Function(this.callback_error, ex.ToString());
                st?.Disconnect();
                st?.Dispose();
            }
        }

        private void Start_Upload() {
            try {st.Upload();

                // SftpClient is Asynchronous, so we need to wait here after the upload and handle the status directly since no status events are raised.
                if (st.Protocol == SecureTransfer.SSHTransferProtocol.SFTP) {
                    var fi = new FileInfo(st.SrcFile);
                    while (!st.async_upload_result.IsCompleted) {
                        var max = fi.Length > int.MaxValue
                            ? Convert.ToInt32(fi.Length / 1024)
                            : Convert.ToInt32(fi.Length);

                        var cur = fi.Length > int.MaxValue
                            ? Convert.ToInt32(st.async_upload_result.UploadedBytes / 1024)
                            : Convert.ToInt32(st.async_upload_result.UploadedBytes);
                        Show_Progress(cur, max);
                        Thread.Sleep(50);
                    }
                }

                st.End_Upload();
                
                pFrmApp.JS_Function(this.callback_error,"传输完毕");
                st.Disconnect();
                st.Dispose();
            } catch (Exception ex) {
                pFrmApp.JS_Function(this.callback_error, ex.ToString());
                st?.Disconnect();
                st?.Dispose();
            }
        }


        private int maxVal;
        private int curVal;
        private delegate void SetStatusCB();

        private void SetStatus() {
            int a = 100 * curVal / maxVal;
            pFrmApp.JS_Function(this.callback_status, curVal + ","+ maxVal);
        }


        private void Show_Progress(int transferredBytes, int totalBytes) {
            maxVal = totalBytes;
            curVal = transferredBytes;

            SetStatus();
        }



        public void Send_Msg(string Type, string strJSON) {

            JObject jObject = JObject.Parse(strJSON);
            Task.Run(async () => {
                await pFrmApp.client.EmitAsync(Type, jObject);
            });
        }

        public void Send_Msg(string Type,string strFrom,string strTo,string data) {

            var pObj = new {
                type = "",
                from = strFrom,
                to = strTo,
                message = data
            };
            Task.Run(async () => {
                await pFrmApp.client.EmitAsync("chat_event", pObj);
            });


        }


        public void Socket_Init(
            string url,
            string callback_Connect,
            string callback_DisConnect,
            string callback_chat_event,
            string callback_system_event) {

            pFrmApp.Init(url, callback_Connect, callback_DisConnect, callback_chat_event, callback_system_event);
        }

        public void Socket_Connect() {
            if (pFrmApp.client.State != SocketIOClient.SocketIOState.Connected) {

                Task.Run(async () => {
                    await pFrmApp.client.ConnectAsync();
                });
            }
        }

        public bool Socket_Connected() {
            if (pFrmApp.client.State == SocketIOClient.SocketIOState.Closed) {
                return false;
            } else if (pFrmApp.client.State == SocketIOClient.SocketIOState.Connected) {
                
                return true;
            } else {
                return true;
            }
        }

        public string http_get(string url,string encode="utf-8") {
            return S_Net.Http(url,"GET","", encode);
        }


        public string http_get_ref(string url, string encode,string reference) {
            return S_Net.Http(url, "GET", "", encode, reference);
        }


        public string Set_Proxy(string ip, string port) {
            if (Proxies.SetProxy(ip + ":" + port)) {
                return "已设置代理：" + ip + ":" + port;
            } else {
                return "设置代理失败. 原因：无效IP和端口.";
            }
        }


        public string UnSet_Proxy() {
            if (Proxies.UnsetProxy()) {
                return "已取消代理.";
            } else {
                return "取消代理失败.";
            }
        }
    }
}
