using FunnyApp.Function.TCP;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FunnyApp.Function {
    public class C_TCP {
        public static TCP_Msg_Sender tcp_sender = new TCP_Msg_Sender();

        Socket socket;
        //FrmApp pFrmApp = null;
        public string user_name = "";
        public string call_back_msg = "";

        public C_TCP(){
        }


        public void hook_event(string call_back_msg) {
            this.call_back_msg = call_back_msg;
        }


        public void connect(string ip, int port,
            string user_name,
            string call_back_connect,
            string call_back_msg) {

            this.user_name = user_name;
            this.call_back_msg = call_back_msg;

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try {
                socket.Connect(ip, port);

            } catch (Exception ex) {
                AddMessage(ex.ToString());
                return;
            }

            if (!socket.Connected) {
            } else {
                tcp_sender.Raise_Event(call_back_connect, "");
                //pFrmApp.Call_Event(call_back_connect, "");
            }


            bool bError = false;
            // upload as javascript blob
            Task.Run(() => {
                while (bError == false) {
                    byte[] message = new byte[4096];
                    int bytes = 0;
                    do {
                        try {
                            bytes = socket.Receive(message, message.Length, 0);

                        } catch (Exception ex) {
                            bError = true;
                            AddMessage(ex.ToString());
                            return;
                        }
                    }
                    while (bytes == 0);

                    AddMessage(Encoding.UTF8.GetString(message, 0, bytes));
                    Thread.Sleep(500);
                }
            });
        }

        public int keep_count = 0;
        public string data_remain = "";
        private void AddMessage(string data) {
            data = data_remain + data;
            data_remain = "";

            while (data != null && "".Equals(data) == false) {
                if (data.StartsWith("m:<s>:")) {
                    keep_count = 0;
                    int index1 = data.IndexOf(":<s>:");
                    int index2 = data.IndexOf(":</s>");
                    if (index2 > index1 && index1 > 0) {
                        string json = data.Substring(index1 + 5, index2-(index1 + 5));
                        JObject jObject = JObject.Parse(json);
                        if (jObject.ContainsKey("k")) {

                        } else {
                            tcp_sender.Raise_Event(this.call_back_msg, json);
                        }

                        data = data.Substring(index2 + 5);
                        int index = data.IndexOf("\n");
                        if (index >= 0) data = data.Substring(index + 1);
                    } else {
                        data_remain = data;
                        break;
                    }
                } else {
                    Console.WriteLine("error=" + data);
                    data_remain = data;
                    break;
                }
            }


        }

        public void send(string msg) {

            // Use the SelectWrite enumeration to obtain Socket status.
            if (socket.Poll(-1, SelectMode.SelectWrite)) {
                Console.WriteLine("This Socket is writable.");

                byte[] outStream = Encoding.UTF8.GetBytes(msg + "\r\n");
                try {
                    socket.Send(outStream, outStream.Length, 0);
                } catch (Exception ex) {
                    Console.WriteLine("disconnected");
                }
            } else if (socket.Poll(-1, SelectMode.SelectRead)) {
                Console.WriteLine("This Socket is readable.");
            } else if (socket.Poll(-1, SelectMode.SelectError)) {
                Console.WriteLine("This Socket has an error.");
            }

        }

        public void close() {
            if (socket != null) { 
                socket.Close();
            }
        }
    }


    public class TCP_Msg_EventArgs : EventArgs {
        private string msg;
        private string strEvent;
        public TCP_Msg_EventArgs(string strEvent,string msg)
            : base() {
            this.strEvent = strEvent;
            this.msg = msg;
        }

        public string Event {
            get {
                return strEvent;
            }
        }

        public string Msg {
            get {
                return msg;
            }
        }
    }


    
    
    //public class MainEntryPoint {
    //    public static void Main(string[] args) {
    //        // 实例化一个事件发送器
    //        TCP_Msg_Sender tcp_sender = new TCP_Msg_Sender();
    //        // 实例化一个事件接收器
    //        TCP_Msg_Receiver eventReceiver = new TCP_Msg_Receiver(tcp_sender);
    //        // 运行
    //        tcp_sender.Run();
    //    }
    //}
}
