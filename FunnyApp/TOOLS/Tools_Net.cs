using B_Net.Funny;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FunnyApp {
    public partial class Tools {


        public enum MessageClass {
            DebugMsg = 0,
            InformationMsg = 1,
            WarningMsg = 2,
            ErrorMsg = 3,
        }

        public string callback_status = "";
        public string callback_error = "";
        public void Net_Upload(
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

                // Connect creates the protocol objects and makes the initial connection.
                st.Connect();

                switch (Protocol) {
                    //case SecureTransfer.SSHTransferProtocol.SCP:
                    //    st.ScpClt.Uploading += ScpClt_Uploading;
                    //    break;
                    case SecureTransfer.SSHTransferProtocol.SFTP:
                        st.asyncCallback = AsyncCallback;
                        break;
                }

                var t = new Thread(StartTransferBG);
                t.SetApartmentState(ApartmentState.STA);
                t.IsBackground = true;
                t.Start();
            } catch (Exception ex) {
                pFrmApp.JS_Function(this.callback_error, ex.ToString());
                //txMsg.Text = ex.ToString();
                //Runtime.MessageCollector.AddExceptionStackTrace(Language.strSSHTransferFailed, ex);
                st?.Disconnect();
                st?.Dispose();
            }
        }



        private void AsyncCallback(IAsyncResult ar) {
            
            pFrmApp.JS_Function(this.callback_error, ar.ToString());
            //Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg, $"SFTP AsyncCallback completed.", true);
        }



        private void StartTransferBG() {
            try {
                //DisableButtons();
                //Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                //                                    $"Transfer of {Path.GetFileName(st.SrcFile)} started.", true);
                st.Upload();

                // SftpClient is Asynchronous, so we need to wait here after the upload and handle the status directly since no status events are raised.
                if (st.Protocol == SecureTransfer.SSHTransferProtocol.SFTP) {
                    var fi = new FileInfo(st.SrcFile);
                    while (!st.asyncResult.IsCompleted) {
                        var max = fi.Length > int.MaxValue
                            ? Convert.ToInt32(fi.Length / 1024)
                            : Convert.ToInt32(fi.Length);

                        var cur = fi.Length > int.MaxValue
                            ? Convert.ToInt32(st.asyncResult.UploadedBytes / 1024)
                            : Convert.ToInt32(st.asyncResult.UploadedBytes);
                        SshTransfer_Progress(cur, max);
                        Thread.Sleep(50);
                    }
                }

                pFrmApp.JS_Function(this.callback_error,"传输完毕");
                //Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                //                                    $"Transfer of {Path.GetFileName(st.SrcFile)} completed.", true);
                st.Disconnect();
                st.Dispose();
            } catch (Exception ex) {
                pFrmApp.JS_Function(this.callback_error, ex.ToString());
                //Runtime.MessageCollector.AddExceptionStackTrace(Language.strSSHStartTransferBG, ex,
                //                                                MessageClass.ErrorMsg, false);
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
            //JS.jint.Invoke(this.callback_status, 100*curVal/maxVal);
            
        }


        private void SshTransfer_Progress(int transferredBytes, int totalBytes) {
            maxVal = totalBytes;
            curVal = transferredBytes;

            SetStatus();
        }



        public void Send_Msg(string Type,string strFrom,string strTo,string data) {

            JObject jObject = JObject.Parse(JsonConvert.SerializeObject(new {
                from = strFrom,
                to = strTo,
                message = data,
                user = "0",
                msg_id = "0",
            }));
            //"chat_event"
            pFrmApp.socket.Emit(Type, jObject);
        }


        public string Net_Http_GET(string url) {
            return S_Net.Http(url,"GET","","utf-8");
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
