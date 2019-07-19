using B_File.Funny;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quobject.SocketIoClientDotNet.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tulpep.NotificationWindow;

namespace FunnyApp
{
    public partial class FrmApp : Form
    {

        public string File = "";
        public Socket socket = null;
        public FrmApp()
        {
            InitializeComponent();
        }

        private void FrmApp_Load(object sender, EventArgs e)
        {
            if ("".Equals(File)) {
                MessageBox.Show("没有设置启动参数");
            } else { 
                String strCode = S_File_Text.Read(File);
                JS.Run_Code(this,strCode);
            }
        }

        delegate void d_Call_Event(string str1, string str2);//创建一个代理

        public void Call_Event(string str1, string str2) {

            if (!this.InvokeRequired){
                try { 
                    JS.jint.Invoke(str1, str2);
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




        private void Notification(string title,string message) {
            var popupNotifier = new PopupNotifier();
            popupNotifier.TitleText = title;
            popupNotifier.ContentText = message;
            popupNotifier.IsRightToLeft = false;
            popupNotifier.Popup();
        }
        public void Init(string url,
            string callback_Connect,
            string callback_chat_event)
        {

            socket = IO.Socket(url);
            socket.On(Socket.EVENT_CONNECT, () =>
            {
                Console.Write("test");
                Call_Event(callback_Connect,"");

            });

            socket.On("chat_event", (data) =>
            {
                Call_Event(callback_chat_event,data.ToString());//, data.ToString());

            });
        }
        private void FrmApp_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            JS.jint.Invoke("event_connected");
        }
    }
}
