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
        private string ip="";
        private int port=0;
        public string user_name = "";
        private string call_back_connect = "";
        public string call_back_msg = "";
        private string call_back_error = "";
        public int keep_count = 0;
        public string data_remain = "";
        public bool b_msg_receive_loop_error = false;

        public C_TCP(){
        }


        public void hook_event(string call_back_msg) {
            this.call_back_msg = call_back_msg;
        }


        public void connect(
            string ip, int port,
            string user_name,
            string call_back_connect,
            string call_back_msg,
            string call_back_error) {

            this.ip = ip;
            this.port = port;
            this.user_name = user_name;
            this.call_back_connect = call_back_connect;
            this.call_back_msg = call_back_msg;
            this.call_back_error = call_back_error;

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try {
                socket.Connect(ip, port);

            } catch (Exception ex) {
                Process_Message(ex.ToString());
                return;
            }

            if (socket.Connected){
                tcp_sender.Raise_Event(call_back_connect, "");
            }


            b_msg_receive_loop_error = false;
            Task.Run(() => {
                while (b_msg_receive_loop_error == false) {
                    byte[] message = new byte[4096];
                    int bytes = 0;
                    do {
                        try {
                            bytes = socket.Receive(message, message.Length, 0);

                        } catch (Exception ex) {
                            b_msg_receive_loop_error = true;
                            //Process_Message(ex.ToString());
                            tcp_sender.Raise_Event(this.call_back_error, ex.ToString());
                            return ;
                        }
                    }
                    while (bytes == 0);

                    Process_Message(Encoding.UTF8.GetString(message, 0, bytes));
                    Thread.Sleep(500);
                }
            });
        }

        public void check_connect() {
            keep_count += 1;
            if (keep_count > 3) {
                if ("".Equals(this.ip) == false) {

                    S_SYS.beep(1000, 400, 5);
                    b_msg_receive_loop_error = true;
                    keep_count = 0;
                    this.connect(this.ip, this.port, this.user_name,
                        this.call_back_connect,
                        this.call_back_msg,
                        this.call_back_error);
                }
            }
        }

        private void Process_Message(string data) {
            data = data_remain + data;
            data_remain = "";

            while (data != null && "".Equals(data) == false) {
                int index1 = data.IndexOf(":<s>:");
                int index2 = data.IndexOf(":</s>");
                if (index2 > index1 && index1 > -1) {
                    string json = data.Substring(index1 + 5, index2-(index1 + 5));
                    JObject jObject = JObject.Parse(json);
                    if (jObject.ContainsKey("k")) {
                        keep_count = 0;
                    } else {
                        tcp_sender.Raise_Event(this.call_back_msg, json);
                    }
                    data = data.Substring(index2 + 5);
                } else {
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

}
