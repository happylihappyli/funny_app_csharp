
using B_File.Funny;
using B_Math;
using FunnyApp.Baidu;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace FunnyApp {
    public partial class Tools {

        FrmApp pFrmApp;

        public Tools(FrmApp FrmApp) {
            this.pFrmApp = FrmApp;
        }

        public void Set_Title(string strLine) {
            pFrmApp.Text = strLine;
        }

        public void Add_Progress(
            string name, 
            int x, int y,
            int width, int height) {
            Point p = new Point(x, y);//定义一个具体的位置  
            ProgressBar pControl = new ProgressBar();//实例化一个button  
            pControl.Name = name;
            //pControl.Text = text;
            //pControl.Tag = new Function_Callback(Function, Function_data);
            //tag是控件留给用户自己定义的一个数据项,
            pControl.Location = p;
            pControl.Size = new Size(width, height);
            pFrmApp.Controls.Add(pControl);//向具体的控件中添加button  

        }


        public void Show_ProgressBar(
            string control_name, string strMax, string strValue) {
            ProgressBar pControl = (ProgressBar)pFrmApp.Controls[control_name];
            if (pControl != null) {
                pControl.Maximum = Int32.Parse(strMax);
                pControl.Value = Int32.Parse(strValue); ;
            }
        }



        public void Add_Button(string name, string text,
            int x, int y,
            int width, int height,
            string Function, String Function_data) {
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


        public void Add_Password(string name, string text,
            int x, int y,
            int width, int height,
            string call_back) {
            Point p = new Point(x, y);//定义一个具体的位置  
            TextBox pControl = new TextBox();//实例化一个button 
            pControl.PasswordChar = '*';
            pControl.Name = name;
            pControl.Text = text;
            pControl.Tag = call_back;//tag是控件留给用户自己定义的一个数据项,
            pControl.Location = p;
            pControl.Multiline = false;
            pControl.ScrollBars = ScrollBars.Vertical;
            pControl.Size = new Size(width, height);
            pFrmApp.Controls.Add(pControl);//向具体的控件中添加button  
        }

        public void Add_Text(string name, string text,
            int x, int y,
            int width, int height) {
            Point p = new Point(x, y);//定义一个具体的位置  
            TextBox pControl = new TextBox();//实例化一个button  
            pControl.Name = name;
            pControl.Text = text;
            pControl.Location = p;
            pControl.Multiline = false;
            pControl.ScrollBars = ScrollBars.Vertical;
            pControl.Size = new Size(width, height);
            pFrmApp.Controls.Add(pControl);//向具体的控件中添加button  
        }



        public void Add_Text_Multi(string name, string text,
            int x, int y,
            int width, int height) {
            Point p = new Point(x, y);//定义一个具体的位置  
            TextBox pControl = new TextBox();//实例化一个button  
            pControl.Name = name;
            pControl.Text = text;
            pControl.Location = p;
            pControl.Multiline = true;
            pControl.ScrollBars = ScrollBars.Vertical;
            pControl.Size = new Size(width, height);
            pFrmApp.Controls.Add(pControl);//向具体的控件中添加button  
        }


        public void Add_Label(string name, string text,
            int x, int y) {

            Point p = new Point(x, y);//定义一个具体的位置  
            Label pControl = new Label();//实例化一个button  
            pControl.Name = name;
            pControl.Text = text;
            pControl.Location = p;
            pControl.AutoSize = true;
            pFrmApp.Controls.Add(pControl);//向具体的控件中添加button  
        }

        public void Show_Text(string control_name, string text) {
            TextBox pControl = (TextBox)pFrmApp.Controls[control_name];
            if (pControl != null) pControl.Text = text;
        }

        public string Get_Text(string control_name) {
            TextBox pControl = (TextBox)pFrmApp.Controls[control_name];
            if (pControl != null) {
                return pControl.Text;// = text;
            } else {
                return "";
            }
        }

        private void my_button_Click(object sender, EventArgs e) {
            Button button = (Button)sender;
            Function_Callback p = (Function_Callback)button.Tag;
            JS.jint.Invoke(p.Name, p.Data);
        }

        public void Show_Form(int width, int height) {
            pFrmApp.Width = width;
            pFrmApp.Height = height;
            pFrmApp.Left = (Screen.PrimaryScreen.WorkingArea.Width - width) / 2;
            pFrmApp.Top = (Screen.PrimaryScreen.WorkingArea.Height - height) / 2;
            pFrmApp.Show();
            pFrmApp.WindowState = FormWindowState.Normal;
        }


        public void Connect_Socket(
            string url,
            string callback_Connect,
            string callback_chat_event,
            string callback_system_event) {
            // 
            //Call_Init(url, callback_Connect, callback_chat_event, pFrmApp.Init);
            pFrmApp.Init(url, callback_Connect, callback_chat_event, callback_system_event);
        }

        public void Notification(string title, string message) {
            pFrmApp.Call_Notifiction(title, message);
        }

    }
}
