
using B_File.Funny;
using B_Math;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

        [Obsolete("Add_Progress() is Obsolete,Use Progress_Init()")]
        public void Add_Progress(
            string name, 
            int x, int y,
            int width, int height) {
            Progress_Init(name, x,y, width, height);
        }

        public void Progress_Init(
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

        public void DataGrid_Add_Line(string control_name,string data,string sep) {
            DataGridView pControl = (DataGridView)pFrmApp.Controls[control_name];
            if (pControl != null) {


                string[] row0 = data.Split(sep[0]);
                pControl.Rows.Add(row0);
                int i = pControl.Rows.Count - 2;
                pControl.Rows[i].HeaderCell.Value = (i+1)+"" ;
            }
        }


        public void DataGrid_Add_Button(
            string control_name, 
            string btn_Name, 
            string btn_Text,
            string my_event) {
            DataGridView pControl = (DataGridView)pFrmApp.Controls[control_name];
            if (pControl != null) {

                //在datagridview中添加button按钮
                DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
                btn.Tag = new Function_Callback(my_event,"");
                btn.Name = btn_Name;
                btn.HeaderText = btn_Text;
                btn.DefaultCellStyle.NullValue = btn_Text;
                pControl.Columns.Add(btn);
            }
        }


        public int DataGrid_Rows(
            string control_name) {
            DataGridView pControl = (DataGridView)pFrmApp.Controls[control_name];
            if (pControl != null) {

                return pControl.Rows.Count;
            }
            return 0;
        }

        public string DataGrid_Read(
            string control_name,
            int index,
            int column_index) {
            DataGridView pControl = (DataGridView)pFrmApp.Controls[control_name];
            if (pControl != null) {

                return Convert.ToString(pControl.Rows[index].Cells[column_index].Value);
            }
            return "";
        }

        public void DataGrid_Set(
            string control_name,
            int index, int Col, string strText) {
            DataGridView pControl = (DataGridView)pFrmApp.Controls[control_name];
            if (pControl != null) {
                pControl.Rows[index].Cells[Col].Value = strText;
            }
        }

        public void DataGrid_Clear(
            string control_name) {
            DataGridView pControl = (DataGridView)pFrmApp.Controls[control_name];
            if (pControl != null) {
                pControl.Rows.Clear();
                pControl.Columns.Clear();
            }
        }



        /// <summary>
        /// 表格初始化
        /// </summary>
        /// <param name="name"></param>
        /// <param name="Column_Count"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void DataGrid_Init(string control_name,
            int x, int y,
            int width, int height) {


            Point p = new Point(x, y);//定义一个具体的位置  
            DataGridView pControl = new DataGridView();//实例化一个button  
            pControl.Name = control_name;
            pControl.Location = p;
            pControl.Size = new Size(width, height);
            pFrmApp.Controls.Add(pControl);//向具体的控件中添加

            pControl.CellContentClick += new DataGridViewCellEventHandler(dataGridView1_CellContentClick);
        }



        public void DataGrid_Init_Column(string control_name,
            int Column_Count,
            string strLines) {

            DataGridView pControl = (DataGridView)pFrmApp.Controls[control_name];
            if (pControl != null) {
                string[] strSplit = strLines.Split(',');
                for (int i = 0; i < Column_Count; i++) {
                    DataGridViewTextBoxColumn pColunm = new DataGridViewTextBoxColumn();
                    pColunm.HeaderText = strSplit[i];// Encoding.ASCII.GetString(new byte[] { (byte)("A"[0] + i) });
                    pControl.Columns.Add(pColunm);
                }
            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) {
            DataGridView dataGridView1 = (DataGridView)sender;
            //点击button按钮事件
            if ( e.RowIndex >= 0) {
                //说明点击的列是DataGridViewButtonColumn列
                DataGridViewColumn column = dataGridView1.Columns[e.ColumnIndex];
                if (column.Tag != null) {
                    Function_Callback p = (Function_Callback)column.Tag;
                    pFrmApp.Call_Event(p.Name, e.RowIndex+""); 
                }
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
                ToolStripMenuItem pControl2 = new ToolStripMenuItem();//实例化一个
                pControl2.Name = item;
                pControl2.Text = text;
                pControl2.Tag = new Function_Callback(menu_event,menu_data);
                pControl2.Click += new System.EventHandler(MyMenuItem_Click);

                pControl.DropDownItems.Add(pControl2);//向具体的控件中添加
                
            }
        }



        public void Menu_Item2_Add(
            string menu_name,
            string control_name,
            string item,
            string item2,
            string text,
            string menu_event,
            string menu_data) {
            MenuStrip pMenu = (MenuStrip)pFrmApp.Controls[menu_name];
            ToolStripMenuItem pControl = (ToolStripMenuItem)pMenu.Items[control_name];
            if (pControl != null) {

                ToolStripMenuItem pControl2 = (ToolStripMenuItem)pControl.DropDownItems[item];
                if (pControl2 != null) {
                    ToolStripMenuItem pControl3 = new ToolStripMenuItem();//实例化一个
                    pControl3.Name = item2;
                    pControl3.Text = text;
                    pControl3.Tag = new Function_Callback(menu_event, menu_data);
                    pControl3.Click += new System.EventHandler(MyMenuItem_Click);

                    pControl2.DropDownItems.Add(pControl3);//向具体的控件中添加
                }
                

            }
        }
        private void MyMenuItem_Click(object sender, EventArgs e) {
            ToolStripMenuItem pControl2 = (ToolStripMenuItem)sender;
            if (pControl2.Tag != null) {

                Function_Callback p = (Function_Callback)pControl2.Tag;
                pFrmApp.Call_Event(p.Name, p.Data); // .pJS.jint.Invoke
            }

        }


        public void Web_Init(string name,
            int x, int y,
            int width, int height) {
            Point p = new Point(x, y);//定义一个具体的位置  
            WebBrowser pControl = new WebBrowser();//实例化一个button  
            pControl.Name = name;
            //pControl.Text = text;
            //tag是控件留给用户自己定义的一个数据项,
            pControl.Location = p;
            pControl.Size = new Size(width, height);
            pFrmApp.Controls.Add(pControl);//向具体的控件中添加

        }



        public void Web_Content(string control_name,
            string content) {
            WebBrowser pControl = (WebBrowser)pFrmApp.Controls[control_name];
            if (pControl != null) {
                pControl.DocumentText = content;
            }
        }


        public void Web_New_Event(string control_name,
            string strEvent) {
            WebBrowser pControl = (WebBrowser)pFrmApp.Controls[control_name];
            if (pControl != null) {
                pControl.NewWindow += new CancelEventHandler(this.webBrowser1_NewWindow);
                pControl.Tag = new Function_Callback(strEvent,"");
            }
        }

        private void webBrowser1_NewWindow(object sender, CancelEventArgs e) {
            e.Cancel = true;
            WebBrowser web = (WebBrowser)sender;
            string url = web.Document.ActiveElement.GetAttribute("href");
            //webBrowser1.Navigate(url);

            Function_Callback p = (Function_Callback)web.Tag;
            pFrmApp.Call_Event(p.Name, url);
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


        public void Tree_Init(string name,
            int x, int y,
            int width, int height) {
            Point p = new Point(x, y);//定义一个具体的位置  
            TreeView pControl = new TreeView();//实例化一个button  
            pControl.Name = name;
            pControl.Location = p;
            pControl.Size = new Size(width, height);
            pFrmApp.Controls.Add(pControl);//向具体的控件中添加
        }



        public void Tree_Add_Node_Root(
            string control_name,
            string key,
            string text,
            string fun_event) {
            TreeView pControl = (TreeView)pFrmApp.Controls[control_name];
            if (pControl != null) {
                TreeNode pNode=pControl.Nodes.Add(key,text);
                pNode.Tag = new Function_Callback(fun_event, "");
            }
        }



        public void Tree_Add_Node(
            string control_name,
            string key,
            string key2,
            string text,
            string fun_event) {
            TreeView pControl = (TreeView)pFrmApp.Controls[control_name];
            if (pControl != null) {
                TreeNode pNode = pControl.Nodes[key].Nodes.Add(key2, text);
                
                pNode.Tag = new Function_Callback(fun_event, "");
            }
        }

        public string Tree_Disk() {
            string strReturn = "";

            //循环遍历计算机所有逻辑驱动器名称(盘符)
            foreach (string drive in Environment.GetLogicalDrives()) {
                //实例化DriveInfo对象 命名空间System.IO
                var dir = new DriveInfo(drive);
                switch (dir.DriveType)           //判断驱动器类型
                {
                    case DriveType.Fixed:        //仅取固定磁盘盘符 Removable-U盘 
                    {
                        strReturn += dir.Name + ",";
                    }
                    break;
                }
            }
            if (strReturn.EndsWith(",")) {
                strReturn = strReturn.Substring(0, strReturn.Length - 1);
            }
            return strReturn;
        }


        public void Tray_Show(string url) {
            if (url.StartsWith("@")) {
                url = url.Replace("@", App_Path());
            }
            pFrmApp.Tray_Show(url);
        }

        public int ListBox_Select(
            string control_name,
            string Name) {
            ListBox pControl = (ListBox)pFrmApp.Controls[control_name];
            if (pControl != null) {
                for (int i=0;i< pControl.Items.Count; i++) {
                    string strName = (string)pControl.Items[i];
                    if (strName == Name) {
                        pControl.SelectedIndex = i;
                        return i;
                    }
                }
            }
            return -1;
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

        public void Close() {
            pFrmApp.Close();
        }

        public void Password_Init(string name, string text,
            int x, int y,
            int width, int height) {
            Point p = new Point(x, y);//定义一个具体的位置  
            TextBox pControl = new TextBox();//实例化一个button 
            pControl.PasswordChar = '*';
            pControl.Name = name;
            pControl.Text = text;
            pControl.Location = p;
            pControl.Multiline = false;
            pControl.ScrollBars = ScrollBars.Vertical;
            pControl.Size = new Size(width, height);
            pFrmApp.Controls.Add(pControl);//向具体的控件中添加button  
        }

        public void Text_Init(string name, string text,
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



        public void Combox_Init(string name, string text,
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

        public void Combox_Text_Set(string control_name,string value) {
            ComboBox pControl = (ComboBox)pFrmApp.Controls[control_name];
            if (pControl != null) {
                pControl.Text= value;
            }
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

        [Obsolete("Set_Text() is Obsolete,Use Text_Set()")]
        public void Set_Text(string control_name, string text) {
            Text_Set(control_name, text);
        }

        public void Text_Set(string control_name, string text) {
            TextBox pControl = (TextBox)pFrmApp.Controls[control_name];
            if (pControl != null) pControl.Text = text;
        }



        [Obsolete("Show_Text() is Obsolete,Use Text_Set()")]
        public void Show_Text(string control_name, string text) {
            Text_Set(control_name, text);
        }


        [Obsolete("Text_Show() is Obsolete,Use Text_Set()")]
        public void Text_Show(string control_name, string text) {
            Text_Set(control_name, text);
        }


        public string Get_Text(string control_name) {
            return Text_Read(control_name);
        }
        public string Text_Read(string control_name) {
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
