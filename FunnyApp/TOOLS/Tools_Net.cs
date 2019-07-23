using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FunnyApp {
    public partial class Tools {



        public string callback_status = "";
        public void Net_Upload(
            string hosts, string user,
            string password, string port,
            string local_file, string remotefile,
            string callback_status) {

            this.callback_status = callback_status;
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
                //txMsg.Text = ex.ToString();
                //Runtime.MessageCollector.AddExceptionStackTrace(Language.strSSHTransferFailed, ex);
                st?.Disconnect();
                st?.Dispose();
            }
        }



        private void AsyncCallback(IAsyncResult ar) {
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

                //Runtime.MessageCollector.AddMessage(MessageClass.InformationMsg,
                //                                    $"Transfer of {Path.GetFileName(st.SrcFile)} completed.", true);
                st.Disconnect();
                st.Dispose();
            } catch (Exception ex) {
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
    }
}
