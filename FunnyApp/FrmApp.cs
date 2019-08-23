using AutoIt;
using B_Data.Funny;
using B_File.Funny;
using B_IniFile;
using CommonTreapVB.TreapVB;
using FunnyApp.TOOLS.Audio;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SocketIOClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tulpep.NotificationWindow;
//using Chromely.Core;
//using Chromely.Core.Host;
//using Chromely.Core.Infrastructure;

namespace FunnyApp
{
    public partial class FrmApp : Form
    {
        public static Treap<FrmApp> pTreapFrmApp = new Treap<FrmApp>();

        public FrmApp pParent = null;
        public string strFile = "";
        //public Socket socket = null;
        //public SocketIO socket = null;
        public JS pJS = new JS();
        public Tools tools = null;

        public FrmApp()
        {
            InitializeComponent();
            tools = new Tools(this);
        }

        private void FrmApp_Load(object sender, EventArgs e)
        {

            if ("".Equals(strFile)) {
                MessageBox.Show("没有设置启动参数");
            } else {
                string strCode = S_File_Text.Read(strFile);
                pJS.Run_Code(this, strCode);
            }
        }

        delegate void d_Call_Event(string str1, string str2);//创建一个代理

        public void Call_Event(string str1, string str2) {

            if (!this.InvokeRequired){
                try { 
                    pJS.jint.Invoke(str1, str2);
                }
                catch(Exception ex) {
                    Debug.Print(ex.ToString());
                }
            } else{
                d_Call_Event a1 = new d_Call_Event(Call_Event);
                Invoke(a1, new object[] { str1, str2 });
            }

        }



        delegate void d_Call_Notifiction(string title, string message);//创建一个代理
        public void Call_Notifiction(string title, string message) {
            if (!this.InvokeRequired) {
                Notification(title, message);

            } else {
                d_Call_Notifiction a1 = new d_Call_Notifiction(Call_Notifiction);
                Invoke(a1, new object[] { title, message });
            }
        }

        delegate void d_Call_Au3_Run(string program, string dir);//创建一个代理
        public void Au3_Run(string program, string dir){
            if (!this.InvokeRequired) {
                AutoItX.Run(program, dir);
            } else {
                d_Call_Au3_Run a1 = new d_Call_Au3_Run(Call_Notifiction);
                Invoke(a1, new object[] { program, dir });
            }
        }




        delegate void d_Call_JS_Function(string function, string data);//创建一个代理
        public void JS_Function(string function, string data) {
            if (!this.InvokeRequired) {
                pJS.jint.Invoke(function, data);
            } else {
                d_Call_JS_Function a1 = new d_Call_JS_Function(JS_Function);
                Invoke(a1, new object[] { function, data });
            }
        }

        private void Notification(string title,string message) {
            var popupNotifier = new PopupNotifier();
            popupNotifier.TitleText = title;
            popupNotifier.ContentText = message;
            popupNotifier.IsRightToLeft = false;
            popupNotifier.Click += new System.EventHandler(Notifer_Click);
            popupNotifier.Popup();
        }

        private void Notifer_Click(object sender, EventArgs e) {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        public string callback_Connect = "";
        public string callback_DisConnect = "";
        public SocketIO client = null;
        public void Init(string url,
            string callback_Connect,
            string callback_DisConnect,
            string callback_chat_event,
            string callback_system_event)
        {
            this.callback_Connect = callback_Connect;
            this.callback_DisConnect = callback_DisConnect;

            //url = "ws://robot6.funnyai.com:8000/socket.io/?EIO=2&transport=websocket";
            client = new SocketIO(url); // url to nodejs 

            client.OnConnected += Client_OnConnectedAsync;
            client.OnClosed += Client_OnClosedAsync;
            //socket.Message += SocketMessage;
            //socket.SocketConnectionClosed += SocketConnectionClosed;
            //socket.Error += SocketError;

            //socket.op
            // register for 'connect' event with io server
            client.On("connect", (fn) =>
            {
                Console.WriteLine("\r\nConnected event...\r\n");
                Console.WriteLine("Emit Part object");

                Console.Write("test");
                Call_Event(callback_Connect, "");

                //// emit Json Serializable object, anonymous types, or strings
                //Part newPart = new Part() { PartNumber = "K4P2G324EC", Code = "DDR2", Level = 1 };
                //socket.Emit("partInfo", newPart);
            });

            // register for 'update' events - message is a json 'Part' object
            client.On("chat_event", (data) =>
            {
                Call_Event(callback_chat_event, data.Text);

            });

            client.On("sys_event", (data) => {
                 Call_Event(callback_system_event, data.Text);//, data.ToString());

            });

            // make the socket.io connection
            Task.Run(async () => {
                await client.ConnectAsync();
            });

        }

        private void Client_OnClosedAsync(ServerCloseReason obj) {

            Console.WriteLine("Disconnect to server");

            Call_Event(this.callback_DisConnect, "");

        }

        public void Client_OnConnectedAsync() {
            Console.WriteLine("Connected to server");
            Call_Event(callback_Connect, "");

            //Task.Run(async () => {
            //    await client.EmitAsync("chat_event",
            //        new { from = "a",type="", to = "", message = "test .net" });

            //});
        }


        private void FrmApp_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        public string time_function = "";
        public string[] args;

        private void timer1_Tick(object sender, EventArgs e) {
            if ("".Equals(time_function)==false){
                JS_Function(time_function, "");
            }
        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e) {

        }
    }
}
