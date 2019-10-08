using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FunnyApp.Function {
    public class C_TCP {

        Socket socket;
        FrmApp pFrmApp = null;
        public string user_name = "";
        public string call_back_msg = "";

        public C_TCP(FrmApp pApp) {
            this.pFrmApp = pApp;
        }

        public void connect(string ip,int port,
            string user_name,string call_back_msg) {

            this.user_name = user_name;
            this.call_back_msg = call_back_msg;

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Connects to host using IPEndPoint.
            try {
                socket.Connect(ip, port);// "robot6.funnyai.com", 6000);

            } catch (Exception ex) {
                AddMessage(ex.ToString());
                return;
            }

            if (!socket.Connected) {
                //MessageBox.Show("Unable to connect to host");
                //strRetPage = "Unable to connect to host";
            }

            byte[] outStream = Encoding.UTF8.GetBytes(user_name+ " is joining\r\n");
            socket.Send(outStream, outStream.Length, 0);

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


        private void AddMessage(string msg) {
            pFrmApp.Call_Event(this.call_back_msg, msg);
            //Call_Show(msg);
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
    }
}
