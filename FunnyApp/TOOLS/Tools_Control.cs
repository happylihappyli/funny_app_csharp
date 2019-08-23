
using B_File.Funny;
using B_Math;
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
using System.Threading.Tasks;
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
                double dbMaximum = Double.Parse(strMax);
                pControl.Maximum = (int)dbMaximum;
                double dbValue = Double.Parse(strValue);
                pControl.Value = (int)dbValue; ;
            }
        }

        public void Menu_Init(
            string name,
            int x, int y,
            int width, int height) {

            Point p = new Point(x, y);//定义一个具体的位置  
            MenuStrip pControl = new MenuStrip();//实例化一个button  
            pControl.Name = name;
            pControl.Location = p;
            pControl.Size = new Size(width, height);
            pFrmApp.Controls.Add(pControl);//向具体的控件中添加
        }

        public void Menu_Add(
            string control_name,
            string item,
            string text) {
            MenuStrip pControl = (MenuStrip)pFrmApp.Controls[control_name];
            if (pControl != null) {
                ToolStripMenuItem pControl2 = new ToolStripMenuItem();//实例化一个button  
                pControl2.Name = item;
                pControl2.Text = text;
                pControl.Items.Add(pControl2);//向具体的控件中添加
            }
        }

        public void Menu_Item_Add(
            string menu_name,
            string control_name,
            string item,
            string text,
            string menu_event,
            string menu_data) {
            MenuStrip pMenu = (MenuStrip)pFrmApp.Controls[menu_name];
            ToolStripMenuItem pControl = (ToolStripMenuItem)pMenu.Items[control_name];
            if (pControl != null) {
                ToolStripMenuItem pControl2 = new ToolStripMenuItem();//实例化一个button  
                pControl2.Name = item;
                pControl2.Text = text;
                pControl2.Tag = new Function_Callback(menu_event,menu_data);
                pControl2.Click += new System.EventHandler(MyMenuItem_Click);

                pControl.DropDownItems.Add(pControl2);//向具体的控件中添加
            }
        }

        private void MyMenuItem_Click(object sender, EventArgs e) {
            ToolStripMenuItem pControl2 = (ToolStripMenuItem)sender;
            Function_Callback p = (Function_Callback)pControl2.Tag;
            pFrmApp.pJS.jint.Invoke(p.Name, p.Data);

        }

        public void ListBox_Init(string name,
            int x, int y,
            int width, int height) {
            Point p = new Point(x, y);//定义一个具体的位置  
            ListBox pControl = new ListBox();//实例化一个button  
            pControl.Name = name;
            //pControl.Text = text;
            //tag是控件留给用户自己定义的一个数据项,
            pControl.Location = p;
            pControl.Size = new Size(width, height);
            pFrmApp.Controls.Add(pControl);//向具体的控件中添加

        }
        

        public void ListBox_Clear(string control_name) {
            ListBox pControl = (ListBox)pFrmApp.Controls[control_name];
            if (pControl != null) {
                pControl.Items.Clear();
            }
        }

        public void ListBox_Add(string control_name, string text) {
            ListBox pControl = (ListBox)pFrmApp.Controls[control_name];
            if (pControl != null) {
                pControl.Items.Add(text);
            }
        }


        public void ListBox_Add_Bat(string control_name, string strTexts) {
            ListBox pControl = (ListBox)pFrmApp.Controls[control_name];
            if (pControl != null) {
                strTexts = strTexts.Replace("\r\n", "\n");
                strTexts = strTexts.Replace("\r", "\n");
                string[] strSplit = strTexts.Split('\n');
                for (int i = 0; i < strSplit.Length; i++) {
                    pControl.Items.Add(strSplit[i]);
                }
            }
        }

        /// <summary>
        /// 从文件读取信息到ListBox
        /// </summary>
        /// <param name="control_name"></param>
        /// <param name="file"></param>
        public void ListBox_From_File(string control_name, string file) {
            ListBox pControl = (ListBox)pFrmApp.Controls[control_name];
            if (pControl != null) {
                StreamReader pFile=S_File_Text.Read_Begin(file);
                string strLine=S_File_Text.Read_Line(ref pFile);
                while (strLine != null) {
                    pControl.Items.Add(strLine);
                    strLine = S_File_Text.Read_Line(ref pFile);
                }
                S_File_Text.Read_End(ref pFile);
            }
        }

        public int ListBox_Item_Size(string control_name) {
            ListBox pControl = (ListBox)pFrmApp.Controls[control_name];
            if (pControl != null) {
                return pControl.Items.Count;
            }
            return 0;
        }

        
        public void ListBox_Selected(string control_name, string strIndex) {
            ListBox pControl = (ListBox)pFrmApp.Controls[control_name];
            if (pControl != null) {
                int index = Int32.Parse(strIndex);
                pControl.SelectedIndex = index;//[index].
            }
        }

        public string ListBox_Text(string control_name) {
            ListBox pControl = (ListBox)pFrmApp.Controls[control_name];
            if (pControl != null && pControl.SelectedItem!=null) {
                return pControl.SelectedItem.ToString();
            }
            return "";
        }


        public int ListBox_Index(string control_name) {
            ListBox pControl = (ListBox)pFrmApp.Controls[control_name];
            if (pControl != null && pControl.SelectedItem != null) {
                return pControl.SelectedIndex;
            }
            return -1;
        }

        public string ListBox_Item(string control_name, string strIndex) {
            ListBox pControl = (ListBox)pFrmApp.Controls[control_name];
            if (pControl != null) {
                int index = Int32.Parse(strIndex);
                return pControl.Items[index].ToString();
            }
            return "";
        }



        public void Button_Init(string name, string text,
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



        public void Button_Backgound(string control_name, string file) {
            Button pControl = (Button)pFrmApp.Controls[control_name];
            if (pControl != null) {
                if (file.StartsWith("@")) {
                    //"recording"
                    string key = file.Substring(1);
                    pControl.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject(key);
                } else {
                    Image myimage = new Bitmap(file);
                    pControl.BackgroundImage = myimage;
                }
                pControl.BackgroundImageLayout=ImageLayout.Center;
            }
        }



        public void Button_Enable(string control_name, string strEnable) {
            Button pControl = (Button)pFrmApp.Controls[control_name];
            if (pControl != null) {
                if (strEnable=="1") {
                    //"recording"
                    pControl.Enabled = true;
                } else {
                    pControl.Enabled = false;
                }
            }
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



        public void TextBox_Init(string name, string text,
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


        public void Text_KeyDown(
            string control_name,
            string function_name) {
            TextBox pControl = (TextBox)pFrmApp.Controls[control_name];
            if (pControl != null) {
                pControl.Tag =new Function_Callback(function_name,"");
                pControl.KeyDown += new KeyEventHandler(this.my_textBox_KeyDown);
            }
        }

        public void Acception_Button(string control_name) {
            Button pControl = (Button)pFrmApp.Controls[control_name];
            pFrmApp.AcceptButton = pControl;
        }

        private void my_textBox_KeyDown(object sender, KeyEventArgs e) {
            TextBox textbox1 = (TextBox)sender;
            Function_Callback p = (Function_Callback)textbox1.Tag;
            pFrmApp.pJS.jint.Invoke(p.Name, e.KeyCode);
            e.Handled = true;
            e.SuppressKeyPress = true;
        }

        public void Label_Init(string name, string text,
            int x, int y) {

            Point p = new Point(x, y);//定义一个具体的位置  
            Label pControl = new Label();//实例化一个button  
            pControl.Name = name;
            pControl.Text = text;
            pControl.Location = p;
            pControl.AutoSize = true;
            pFrmApp.Controls.Add(pControl);//向具体的控件中添加button  
        }


        public void PictureBox_Init(string name,
            int x, int y,
            int width, int height) {

            Point p = new Point(x, y);//定义一个具体的位置  
            PictureBox pControl = new PictureBox();//实例化一个button  
            pControl.Name = name;
            pControl.Location = p;
            pControl.Size = new Size(width, height);
            pFrmApp.Controls.Add(pControl);//向具体的控件中添加

        }

        public void PictureBox_Draw_Ellipse(
                string control_name,
                int x,int y,int width,int height,
                string strColor,
                int iPen_Size) {
            PictureBox pControl = (PictureBox)pFrmApp.Controls[control_name];
            
            if (pControl != null) {
                pControl.BorderStyle = BorderStyle.FixedSingle;
                Graphics g2;
                Image b;
                if (pControl.Image != null) {
                    b = pControl.Image;
                } else {
                    b= new Bitmap(pControl.Width, pControl.Height);
                }
                g2 = Graphics.FromImage(b);
                Brush pBrush = new SolidBrush(Color.FromName(strColor));
                g2.FillEllipse(pBrush, x, y, width, height);
                //g2.FillEllipse()
                //g2.DrawEllipse
                pControl.Image = b;
            }
        }


        public void PictureBox_Event(
                string control_name,
                string mouse_up_event) {
            PictureBox pControl = (PictureBox)pFrmApp.Controls[control_name];

            pControl.Tag = mouse_up_event;
            if (pControl != null) {

                pControl.MouseUp += new MouseEventHandler(my_picturebox_mouse_up);//使用事件函数句柄指向一个具体的函数  

            }
        }

        private void my_picturebox_mouse_up(object sender, MouseEventArgs e) {
            PictureBox pBox = (PictureBox)sender;
            pFrmApp.pJS.jint.Invoke(pBox.Tag.ToString(), new[] { e.X,e.Y});
        }



        public void Add_Combox(string name, string text,
            int x, int y,
            int width, int height) {

            Point p = new Point(x, y);//定义一个具体的位置  
            ComboBox pControl = new ComboBox();//实例化一个button  
            pControl.Name = name;
            pControl.Text = text;
            pControl.Location = p;
            pControl.Size = new Size(width, height);
            pFrmApp.Controls.Add(pControl);//向具体的控件中添加

        }


        public void Combox_Add(string control_name, string text) {
            ComboBox pControl = (ComboBox)pFrmApp.Controls[control_name];
            if (pControl != null) {
                pControl.Items.Add(text);
            }
        }


        public void Combox_Clear(string control_name) {
            ComboBox pControl = (ComboBox)pFrmApp.Controls[control_name];
            if (pControl != null) {
                pControl.Items.Clear();
            }
        }

        public string Combox_Text(string control_name) {
            ComboBox pControl = (ComboBox)pFrmApp.Controls[control_name];
            if (pControl != null) {
                return pControl.Text;
            }
            return "";
        }


        public int Combox_Index(string control_name) {
            ComboBox pControl = (ComboBox)pFrmApp.Controls[control_name];
            if (pControl != null) {
                return pControl.SelectedIndex;
            }
            return -1;
        }


        public void Combox_Select(string control_name,int index) {
            ComboBox pControl = (ComboBox)pFrmApp.Controls[control_name];
            if (pControl != null) {
                pControl.SelectedIndex = index;
            }
        }


        public void Set_Text(string control_name, string text) {
            Show_Text(control_name, text);
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
            pFrmApp.pJS.jint.Invoke(p.Name, p.Data);
        }

        public void Show_Form(int width, int height) {
            pFrmApp.Width = width;
            pFrmApp.Height = height;
            pFrmApp.Left = (Screen.PrimaryScreen.WorkingArea.Width - width) / 2;
            pFrmApp.Top = (Screen.PrimaryScreen.WorkingArea.Height - height) / 2;
            pFrmApp.Show();
            pFrmApp.WindowState = FormWindowState.Normal;
        }


        public void Socket_Init(
            string url,
            string callback_Connect,
            string callback_DisConnect,
            string callback_chat_event,
            string callback_system_event) {

            pFrmApp.Init(url, callback_Connect, callback_DisConnect, callback_chat_event, callback_system_event);
        }

        public void Socket_Connect() {

            Task.Run(async () => {
                await pFrmApp.client.ConnectAsync();
            });
        }

        public void Notification(string title, string message) {
            pFrmApp.Call_Notifiction(title, message);
        }


        public void ShowInTask(string value) {
            pFrmApp.ShowInTaskbar = (value == "1");
        }

    }
}
