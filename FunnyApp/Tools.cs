
using B_Math;
using FunnyApp.Baidu;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace FunnyApp {
    public partial class Tools {
        FrmApp pFrmApp;

        public Tools(FrmApp FrmApp) {
            this.pFrmApp = FrmApp;
        }

        public delegate void Init_Delegate(string url,
            string callback_Connect,
            string callback_chat_event);


        private static void Call_Init(string url,
            string callback_Connect,
            string callback_chat_event, Init_Delegate pFunction) {
            pFunction(url, callback_Connect, callback_chat_event);
        }


        public void Connect_Socket(
            string url,
            string callback_Connect,
            string callback_chat_event) {
            Call_Init(url, callback_Connect, callback_chat_event, pFrmApp.Init);

        }

        public void Notification(string title,string message) {
            pFrmApp.Call_Notifiction(title, message);
        }

        public void Msg(string strLine) {
            MessageBox.Show(strLine);
        }

        public void TTS(string strLine) {
            baidu_tts.tts(strLine);
        }

        public string Get_Text(string strName) {
            TextBox pControl = (TextBox)pFrmApp.Controls[strName];
            return pControl.Text;
        }

        public void Send_Msg(string data) {

            JObject jObject = JObject.Parse(JsonConvert.SerializeObject(new {
                from = "csharp",
                to = "*",
                message = data,
                user = "0",
                msg_id = "0",
            }));
            pFrmApp.socket.Emit("chat_event", jObject);
        }

        public void Set_Title(string strLine) {
            pFrmApp.Text = strLine;
        }

        public void Add_Button(string name, string text,
            int x, int y,
            int width, int height,
            string Function ,String Function_data) {
            Point p = new Point(x, y);//定义一个具体的位置  
            Button pControl = new Button();//实例化一个button  
            pControl.Name = name;
            pControl.Text = text;
            pControl.Tag = new Function_Callback(Function, Function_data);
            //tag是控件留给用户自己定义的一个数据项,
            pControl.Location = p;
            pControl.Size = new Size(width, height);
            pFrmApp.Controls.Add(pControl);//向具体的控件中添加button  
            pControl.Click += new EventHandler(my_button_Click);//使用事件函数句柄指向一个具体的函数  

        }



        public void Add_Text(string name, string text,
            int x, int y,
            int width, int height,
            string call_back) {
            Point p = new Point(x, y);//定义一个具体的位置  
            TextBox pControl = new TextBox();//实例化一个button  
            pControl.Name = name;
            pControl.Text = text;
            pControl.Tag = call_back;//tag是控件留给用户自己定义的一个数据项,
            pControl.Location = p;
            pControl.Size = new Size(width, height);
            pFrmApp.Controls.Add(pControl);//向具体的控件中添加button  
        }

        public void Show_Text(string control_name, string text) {
            TextBox pControl = (TextBox)pFrmApp.Controls[control_name];
            pControl.Text = text;
        }


        private void my_button_Click(object sender, EventArgs e) {
            Button button = (Button)sender;
            Function_Callback p =(Function_Callback)button.Tag;
            JS.jint.Invoke(p.Name,p.Data);
        }

        public void Show_Form(int width,int height) {
            pFrmApp.Width = width;
            pFrmApp.Height = height;
            pFrmApp.Show();
            pFrmApp.WindowState = FormWindowState.Normal;
        }

        public double Math_Cal(string strLine) {
            C_Math pMath = new C_Math();
            return pMath.EvaluateExpression(strLine);
        }

        public void Form_Title(string title) {
            pFrmApp.Text = title;
        }
    }
}
