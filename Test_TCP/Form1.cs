using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test_TCP {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        //TcpClient tcpClient = new TcpClient();
        //NetworkStream serverStream = default(NetworkStream);
        string readData = string.Empty;
        string msg = "Conected to Chat Server ...";
        Socket socket;

        private void button1_Click(object sender, EventArgs e) {
            timer1.Enabled = true;

            txtConversation.Text = "";
            AddPrompt();

            Connect();
        } 

        public void Connect() {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Connects to host using IPEndPoint.
            try {
                socket.Connect("robot6.funnyai.com", 6000);

            } catch (Exception ex) {
                AddMessage(ex.ToString());
                return;
            }

            if (!socket.Connected) {
                MessageBox.Show("Unable to connect to host");
                //strRetPage = "Unable to connect to host";
            }

            byte[] outStream = Encoding.UTF8.GetBytes(txtChatName.Text.Trim()
                                  + " is joining\r\n");
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
                AddMessage("test");
            });
        }


        private void AddMessage(string msg) {
            Call_Show(msg);
        }

        // Purpose:     Adds the " >> " prompt in the text box
        // End Result:  Shows prompt to user
        private void AddPrompt() {
            txtConversation.Text = txtConversation.Text +
                Environment.NewLine + " >> " + msg;
        }

        private void Form1_Load(object sender, EventArgs e) {

        }



        delegate void d_Call_Show(string msg);//创建一个代理
        public void Call_Show(string msg) {
            if (!this.InvokeRequired) {
                txtConversation.Text = txtConversation.Text +
                Environment.NewLine + " >> " + msg;
            } else {
                d_Call_Show a1 = new d_Call_Show(Call_Show);
                Invoke(a1, new object[] { msg });
            }
        }

        private void button2_Click(object sender, EventArgs e) {

            // Use the SelectWrite enumeration to obtain Socket status.
            if (socket.Poll(-1, SelectMode.SelectWrite)) {
                Console.WriteLine("This Socket is writable.");

                byte[] outStream = Encoding.UTF8.GetBytes(txMsg.Text + "\r\n");
                try {
                    socket.Send(outStream, outStream.Length, 0);
                }catch(Exception ex) {
                    Console.WriteLine("disconnected");
                }
            } else if (socket.Poll(-1, SelectMode.SelectRead)) {
                Console.WriteLine("This Socket is readable.");
            } else if (socket.Poll(-1, SelectMode.SelectError)) {
                Console.WriteLine("This Socket has an error.");
            }


        }

        private void button3_Click(object sender, EventArgs e) {
            txtConversation.Text = "";
        }


        private void timer1_Tick(object sender, EventArgs e) {
            if (Tools.IsConnected(socket)==false ||
                socket.Connected==false) {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                AddMessage("重新连接");
                Connect();
            }
        }
    }
}
