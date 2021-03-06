﻿
using AForge.Imaging;
using B_Data.Funny;
using B_File.Funny;
using B_Math;
using Gma.System.MouseKeyHook;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FunnyApp {
    public class C_UI {

        FrmApp pFrmApp;
        public Hashtable Ctrls = new Hashtable();


        public C_UI(FrmApp pFrmApp) {
            pFrmApp.pUI = this;
            this.pFrmApp = pFrmApp;
        }

        public void Set_Title(string strLine) {
            pFrmApp.Text = strLine;
        }


        /// <summary>
        /// 运行CMD命令
        /// </summary>
        /// <param name="cmd">命令</param>
        /// <returns></returns>
        public string Run_Cmd(string cmds) {
            try {
                // 创建进程
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardInput = true;
                //process.StartInfo.RedirectStandardOutput = true;
                //process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = false;
                process.Start();
                //process.StandardInput.AutoFlush = true;
                string[] cmd = cmds.Split('\n');
                for (int i = 0; i < cmd.Length; i++) {
                    process.StandardInput.WriteLine(cmd[i]);
                }
                Thread.Sleep(3000);
                process.StandardInput.WriteLine("exit");
                // 执行进程
                //string standardOutput = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                process.Close();
                return "";// standardOutput;
            } catch (Exception ex) {
                return ex.ToString();
            }
        }


        public string Run_Shell(string cmds) {
            try {
                // 创建进程
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo.FileName = "PowerShell.exe";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardInput = true;
                //process.StartInfo.RedirectStandardOutput = true;
                //process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = false;
                process.Start();
                //process.StandardInput.AutoFlush = true;
                string[] cmd = cmds.Split('\n');
                for (int i = 0; i < cmd.Length; i++) {
                    process.StandardInput.WriteLine(cmd[i]);
                }
                Thread.Sleep(3000);
                process.StandardInput.WriteLine("exit");
                // 执行进程
                //string standardOutput = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                process.Close();
                return "";// standardOutput;
            } catch (Exception ex) {
                return ex.ToString();
            }


            //Process p = new Process();
            //p.StartInfo.FileName = "PowerShell.exe";
            //p.StartInfo.UseShellExecute = false;
            //p.StartInfo.RedirectStandardInput = true;
            //p.StartInfo.RedirectStandardOutput = true;
            //p.StartInfo.Verb = "runas";
            //p.StartInfo.CreateNoWindow = false;
            //p.Start();
            //p.StandardInput.WriteLine("set-ExecutionPolicy RemoteSigned");
            //p.StandardInput.WriteLine("y");
            //p.StandardInput.WriteLine("cd SVC_Tool_1.0.0.0_Master_Test");
            //p.StandardInput.WriteLine(".\\Add-AppDevPackage.ps1");
            //p.StandardInput.AutoFlush = true;
            //p.WaitForExit();
            //p.Close();
        }



        public void progress_init(
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
            if (Ctrls.Contains(name) == false) {
                Ctrls.Add(name, pControl);
            } else {
                this.msg("控件已经存在:" + name);
            }
        }


        public void progress_show(
            string control_name, string strMax, string strValue) {
            ProgressBar pControl = (ProgressBar)Ctrls[control_name];
            if (pControl != null) {
                double dbMaximum = Double.Parse(strMax);
                pControl.Maximum = (int)dbMaximum;
                double dbValue = Double.Parse(strValue);
                pControl.Value = (int)dbValue; ;
            }
        }

        public void datagrid_add_line(string control_name,string data,string sep) {
            DataGridView pControl = (DataGridView)Ctrls[control_name];
            if (pControl != null) {


                string[] row0 = data.Split(sep[0]);
                pControl.Rows.Add(row0);
                int i = pControl.Rows.Count - 2;
                pControl.Rows[i].HeaderCell.Value = (i+1)+"" ;
            }
        }


        public void datagrid_add_checkbox(
            string control_name,
            string btn_Name,
            string btn_Text,
            string my_event) {
            DataGridView pControl = (DataGridView)Ctrls[control_name];
            if (pControl != null) {

                //在datagridview中添加button按钮
                DataGridViewCheckBoxColumn btn = new DataGridViewCheckBoxColumn();
                btn.Tag = new Function_Callback(my_event, "");
                btn.Name = btn_Name;
                btn.HeaderText = btn_Text;
                btn.FalseValue = "0";
                btn.TrueValue = "1";
                pControl.Columns.Add(btn);
            }
        }


        public void datagrid_add_button(
            string control_name, 
            string btn_Name, 
            string btn_Text,
            string my_event) {
            DataGridView pControl = (DataGridView)Ctrls[control_name];
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


        public int datagrid_rows(
            string control_name) {
            DataGridView pControl = (DataGridView)Ctrls[control_name];
            if (pControl != null) {

                return pControl.Rows.Count;
            }
            return 0;
        }



        public string datagrid_read(
            string control_name,
            int index,
            int column_index) {
            DataGridView pControl = (DataGridView)Ctrls[control_name];
            if (pControl != null) {
                if (pControl.Rows[index].Cells[column_index].OwningColumn.GetType().Name
                    == "DataGridViewCheckBoxColumn") {
                    DataGridViewCheckBoxCell p = (DataGridViewCheckBoxCell)pControl.Rows[index].Cells[column_index];
                    if (p.EditingCellFormattedValue.Equals(true)) { //p.Value == p.TrueValue){// 
                        return "1";
                    } else {
                        return "0";
                    }
                } else {
                    return Convert.ToString(pControl.Rows[index].Cells[column_index].Value);
                }

            }
            return "";
        }


        public void datagrid_set(
            string control_name,
            int index, int col, string strText) {
            DataGridView pControl = (DataGridView)Ctrls[control_name];
            if (pControl != null) {
                pControl.Rows[index].Cells[col].Value = strText;
            }
        }


        public void datagrid_set_checkbox(
            string control_name,
            int index, int col, string value) {
            DataGridView pControl = (DataGridView)Ctrls[control_name];
            if (pControl != null) {
                DataGridViewCheckBoxCell p = (DataGridViewCheckBoxCell)pControl.Rows[index].Cells[col];

                if ("1".Equals(value)) {
                    p.Value = p.TrueValue;
                } else {
                    p.Value = p.FalseValue;
                }
            }
        }

        public void datagrid_clear(
            string control_name) {
            DataGridView pControl = (DataGridView)Ctrls[control_name];
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
        public void datagrid_init(string name,
            int x, int y,
            int width, int height) {


            Point p = new Point(x, y);//定义一个具体的位置  
            DataGridView pControl = new DataGridView();//实例化一个button  
            pControl.Name = name;
            pControl.Location = p;
            pControl.Size = new Size(width, height);
            pFrmApp.Controls.Add(pControl);//向具体的控件中添加
            if (Ctrls.Contains(name) == false) {
                Ctrls.Add(name, pControl);
            } else {
                this.msg("控件已经存在:" + name);
            }
            // CellContentClick 
            pControl.CellContentClick += new DataGridViewCellEventHandler(dataGridView1_CellContentClick);
        }



        public void datagrid_init_column(string control_name,
            int Column_Count,
            string strLines) {

            DataGridView pControl = (DataGridView)Ctrls[control_name];
            if (pControl != null) {
                string[] strSplit = strLines.Split(',');
                for (int i = 0; i < Column_Count; i++) {
                    DataGridViewTextBoxColumn pColunm = new DataGridViewTextBoxColumn();
                    pColunm.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
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

        public void menu2_init(string name) {
            ContextMenuStrip pControl = new ContextMenuStrip();
            //pFrmApp.Controls.Add(pControl);//向具体的控件中添加
            if (Ctrls.Contains(name) == false) {
                Ctrls.Add(name, pControl);
            } else {
                this.msg("控件已经存在:" + name);
            }
        }


        public void menu2_add(
            string control_name,
            string item,
            string text,
            string menu_event,
            string menu_data) {
            ContextMenuStrip pControl = (ContextMenuStrip)Ctrls[control_name];
            if (pControl != null) {
                ToolStripMenuItem pControl2 = new ToolStripMenuItem();//实例化一个button  
                pControl2.Name = item;
                pControl2.Text = text;
                pControl2.Tag = new Function_Callback(menu_event, menu_data);

                pControl2.Click += new EventHandler(MyMenuItem_Click);

                pControl.Items.Add(pControl2);//向具体的控件中添加
            }
        }



        public void menu2_container(
            string control_name,
            string name) {
            ContextMenuStrip pControl = (ContextMenuStrip)Ctrls[control_name];
            Control pContainer = (Control)Ctrls[name];
            if (pControl != null) {
                pContainer.ContextMenuStrip = pControl;
            }
        }



        public void menu_init(string name) {
            MenuStrip pControl = new MenuStrip();//实例化
            pFrmApp.Controls.Add(pControl);//向具体的控件中添加
            if (Ctrls.Contains(name) == false) {
                Ctrls.Add(name, pControl);
            } else {
                this.msg("控件已经存在:" + name);
            }
        }

        public void menu_add(
            string control_name,
            string item,
            string text) {
            MenuStrip pControl = (MenuStrip)Ctrls[control_name];
            if (pControl != null) {
                ToolStripMenuItem pControl2 = new ToolStripMenuItem();//实例化一个button  
                pControl2.Name = item;
                pControl2.Text = text;
                pControl.Items.Add(pControl2);//向具体的控件中添加
            }
        }

        public void menu_item_add(
            string menu_name,
            string control_name,
            string item,
            string text,
            string menu_event,
            string menu_data) {
            MenuStrip pMenu = (MenuStrip)Ctrls[menu_name];
            ToolStripMenuItem pControl = (ToolStripMenuItem)pMenu.Items[control_name];
            if (pControl != null) {
                ToolStripMenuItem pControl2 = new ToolStripMenuItem();//实例化一个
                pControl2.Name = item;
                pControl2.Text = text;
                pControl2.Tag = new Function_Callback(menu_event,menu_data);
                pControl2.Click += new EventHandler(MyMenuItem_Click);

                pControl.DropDownItems.Add(pControl2);//向具体的控件中添加
                
            }
        }



        public void menu_item2_add(
            string menu_name,
            string control_name,
            string item,
            string item2,
            string text,
            string menu_event,
            string menu_data) {
            MenuStrip pMenu = (MenuStrip)Ctrls[menu_name];
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
            if (Ctrls.Contains(name) == false) {
                Ctrls.Add(name, pControl);
            } else {
                this.msg("控件已经存在:" + name);
            }
        }



        public void Web_Content(string control_name,
            string content) {
            WebBrowser pControl = (WebBrowser)Ctrls[control_name];
            if (pControl != null) {
                pControl.DocumentText = content;
            }
        }


        public void Web_New_Event(string control_name,
            string strEvent) {
            WebBrowser pControl = (WebBrowser)Ctrls[control_name];
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


        public void Tree_Init(string name,
            int x, int y,
            int width, int height) {
            Point p = new Point(x, y);//定义一个具体的位置  
            TreeView pControl = new TreeView();//实例化一个button  
            pControl.Name = name;
            pControl.Location = p;
            pControl.Size = new Size(width, height);
            pControl.NodeMouseClick += new TreeNodeMouseClickEventHandler(mytreeView_NodeMouseClick);

            pFrmApp.Controls.Add(pControl);//向具体的控件中添加
            if (Ctrls.Contains(name) == false) {
                Ctrls.Add(name, pControl);
            } else {
                this.msg("控件已经存在:" + name);
            }
        }

        private void mytreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e) {
            TreeView pControl = (TreeView)sender;
            TreeNode pNode = e.Node;
            Function_Callback pFun = (Function_Callback)pNode.Tag;
            pFrmApp.Call_Event(pFun.Name, pNode.FullPath);
        }

        public void Tree_Add_Node_Root(
            string control_name,
            string key,
            string text,
            string fun_event) {
            TreeView pControl = (TreeView)Ctrls[control_name];
            if (pControl != null) {
                TreeNode pNode=pControl.Nodes.Add(key,text);
                pNode.Tag = new Function_Callback(fun_event, "");
            }
        }



        public void Tree_Add_Node(
            string control_name,
            string keys,
            string key2,
            string text,
            string fun_event) {
            TreeView pControl = (TreeView)Ctrls[control_name];
            if (pControl != null) {
                string[] strSplit = keys.Split('\\');
                TreeNode pNode = pControl.Nodes[strSplit[0]];
                for (int i = 1; i < strSplit.Length; i++) {
                    pNode = pNode.Nodes[strSplit[i]];//.Nodes.Add(key2, text);
                }
                TreeNode pNode2= pNode.Nodes.Add(key2, text);
                pNode2.Tag = new Function_Callback(fun_event, "");
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

        public void Form_Title(string title) {
            pFrmApp.Text = title;
        }

        public void Tray_Show(string url) {
            if (url.StartsWith("@")) {
                url = url.Replace("@", pFrmApp.pSYS.path_app());
            }
            pFrmApp.Tray_Show(url);
        }

        public void msg(string strLine) {
            MessageBox.Show(strLine);
        }


        public int listbox_select(
            string control_name,
            string Name) {
            ListBox pControl = (ListBox)Ctrls[control_name];
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



        public void checkbox_init(string name,
            string text,
            int x, int y,
            int width, int height) {
            Point p = new Point(x, y);//定义一个具体的位置  
            CheckBox pControl = new CheckBox();//实例化一个button  
            pControl.Name = name;
            pControl.Text = text;
            //tag是控件留给用户自己定义的一个数据项,
            pControl.Location = p;
            pControl.Size = new Size(width, height);
            pFrmApp.Controls.Add(pControl);//向具体的控件中添加
            if (Ctrls.Contains(name) == false) {
                Ctrls.Add(name, pControl);
            } else {
                this.msg("控件已经存在:" + name);
            }
        }


        public bool checkbox_checked(string control_name) {
            CheckBox pControl = (CheckBox)Ctrls[control_name];
            if (pControl != null) {
                return pControl.Checked;
            }
            return false;
        }

        public void listbox_clear(string control_name) {
            ListBox pControl = (ListBox)Ctrls[control_name];
            if (pControl != null) {
                pControl.Items.Clear();
            }
        }
        public void listbox_init(string name,
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
            if (Ctrls.Contains(name) == false) {
                Ctrls.Add(name, pControl);
            } else {
                this.msg("控件已经存在:" + name);
            }
        }

        public void listbox_init_event(
            string control_name,
            string event_change) {

            ListBox pControl = (ListBox)Ctrls[control_name];
            if (pControl != null) {
                pControl.SelectedIndexChanged += new EventHandler(mylistbox_DoubleClick);
                pControl.Tag = new Function_Callback(event_change, "");
            }
        }

        private void mylistbox_DoubleClick(object sender, EventArgs e) {
            ListBox pControl = (ListBox)sender;
            Function_Callback pFun = (Function_Callback)pControl.Tag;
            pFrmApp.Call_Event(pFun.Name, pFun.Data);
        }

        public void listbox_add(string control_name, string text) {
            ListBox pControl = (ListBox)Ctrls[control_name];
            if (pControl != null) {
                pControl.Items.Add(text);
            }
        }


        public void listbox_add_bat(string control_name, string strTexts) {
            ListBox pControl = (ListBox)Ctrls[control_name];
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
        public void listbox_from_file(string control_name, string file) {
            ListBox pControl = (ListBox)Ctrls[control_name];
            if (pControl != null) {
                StreamReader pFile=S_File_Text.Read_Begin(file);
                string strLine=S_File_Text.Read_Line(pFile);
                while (strLine != null) {
                    pControl.Items.Add(strLine);
                    strLine = S_File_Text.Read_Line(pFile);
                }
                S_File_Text.Read_End(pFile);
            }
        }

        public int listbox_item_size(string control_name) {
            ListBox pControl = (ListBox)Ctrls[control_name];
            if (pControl != null) {
                return pControl.Items.Count;
            }
            return 0;
        }



        
        public void listbox_selected(string control_name, string strIndex) {
            ListBox pControl = (ListBox)Ctrls[control_name];
            if (pControl != null) {
                int index = Int32.Parse(strIndex);
                pControl.SelectedIndex = index;//[index].
            }
        }

        public string listbox_text(string control_name) {
            ListBox pControl = (ListBox)Ctrls[control_name];
            if (pControl != null && pControl.SelectedItem!=null) {
                return pControl.SelectedItem.ToString();
            }
            return "";
        }


        public int listbox_index(string control_name) {
            ListBox pControl = (ListBox)Ctrls[control_name];
            if (pControl != null && pControl.SelectedItem != null) {
                return pControl.SelectedIndex;
            }
            return -1;
        }

        public string listbox_item(string control_name, int index) {
            ListBox pControl = (ListBox)Ctrls[control_name];
            if (pControl != null) {
                return pControl.Items[index].ToString();
            }
            return "";
        }

        public void listbox_remove(string control_name, int index){// strIndex) {
            ListBox pControl = (ListBox)Ctrls[control_name];
            if (pControl != null) {
                pControl.Items.RemoveAt(index);
            }
        }


        public void button_init(string name, string text,
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
            if (Ctrls.Contains(name) == false) {
                Ctrls.Add(name, pControl);
            } else {
                this.msg("控件已经存在:" + name);
            }
            pControl.Click += new EventHandler(my_button_Click);//使用事件函数句柄指向一个具体的函数  

        }



        public void button_backgound(string control_name, string file) {
            Button pControl = (Button)Ctrls[control_name];
            if (pControl != null) {
                if (file.StartsWith("@")) {
                    //"recording"
                    string key = file.Substring(1);
                    pControl.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject(key);
                } else {
                    System.Drawing.Image myimage = new Bitmap(file);
                    pControl.BackgroundImage = myimage;
                }
                pControl.BackgroundImageLayout=ImageLayout.Center;
            }
        }



        public void button_enable(string control_name, string iEnable) {
            Button pControl = (Button)Ctrls[control_name];
            if (pControl != null) {
                if (iEnable == "1") {
                    //"recording"
                    pControl.Enabled = true;
                } else {
                    pControl.Enabled = false;
                }
            }
        }

        
        public void close() {
            pFrmApp.Close();
        }

        public void password_init(string name, string text,
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
            if (Ctrls.Contains(name) == false) {
                Ctrls.Add(name, pControl);
            } else {
                this.msg("控件已经存在:" + name);
            }
        }

        public void text_init(string name, string text,
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
            if (Ctrls.Contains(name) == false) {
                Ctrls.Add(name, pControl);
            } else {
                this.msg("控件已经存在:" + name);
            }
        }



        public void textbox_init(string name, string text,
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
            if (Ctrls.Contains(name) == false) {
                Ctrls.Add(name, pControl);
            } else {
                this.msg("控件已经存在:" + name);
            }
        }


        public void text_keydown(
            string control_name,
            string function_name) {
            TextBox pControl = (TextBox)Ctrls[control_name];
            if (pControl != null) {
                pControl.Tag =new Function_Callback(function_name,"");
                pControl.KeyDown += new KeyEventHandler(this.my_textBox_KeyDown);
            }
        }



        public void button_default(string control_name) {
            Button pControl = (Button)Ctrls[control_name];
            pFrmApp.AcceptButton = pControl;
        }

        private void my_textBox_KeyDown(object sender, KeyEventArgs e) {
            TextBox textbox1 = (TextBox)sender;
            Function_Callback p = (Function_Callback)textbox1.Tag;
            pFrmApp.pJS.p_Engine.Invoke(p.Name, e.KeyCode);
            e.Handled = true;
            e.SuppressKeyPress = true;
        }

        public void label_init(string name, string text,int x, int y) {

            Point p = new Point(x, y);//定义一个具体的位置  
            Label pControl = new Label();//实例化一个button  
            pControl.Name = name;
            pControl.Text = text;
            pControl.Location = p;
            pControl.AutoSize = true;
            pFrmApp.Controls.Add(pControl);//向具体的控件中添加
            if (Ctrls.Contains(name) == false) {
                Ctrls.Add(name, pControl);
            } else {
                this.msg("控件已经存在:"+name);
            }
        }


        public void picturebox_init(string name,
            int x, int y,
            int width, int height) {

            Point p = new Point(x, y);//定义一个具体的位置  
            PictureBox pControl = new PictureBox();//实例化一个button  
            pControl.Name = name;
            pControl.BorderStyle = BorderStyle.None;
            pControl.Location = p;
            pControl.Size = new Size(width, height);
            pFrmApp.Controls.Add(pControl);//向具体的控件中添加
            if (Ctrls.Contains(name) == false) {
                Ctrls.Add(name, pControl);
            } else {
                this.msg("控件已经存在:" + name);
            }
        }

        public void picturebox_draw_ellipse(
                string control_name,
                int x,int y,int width,int height,
                string strColor,
                int iPen_Size) {
            PictureBox pControl = (PictureBox)Ctrls[control_name];
            
            if (pControl != null) {
                pControl.BorderStyle = BorderStyle.FixedSingle;
                Graphics g2;
                System.Drawing.Image b;
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



        public void picturebox_draw_line(
                string control_name,
                int x, int y, int x2, int y2,
                string strColor,
                int iPen_Size) {
            PictureBox pControl = (PictureBox)Ctrls[control_name];

            if (pControl != null) {
                pControl.BorderStyle = BorderStyle.FixedSingle;
                Graphics g2;
                System.Drawing.Image b;
                if (pControl.Image != null) {
                    b = pControl.Image;
                } else {
                    b = new Bitmap(pControl.Width, pControl.Height);
                }
                g2 = Graphics.FromImage(b);
                Pen pPen = new Pen(Color.FromName(strColor), iPen_Size);
                g2.DrawLine(pPen, x, y, x2, y2);
                //g2.FillEllipse()
                //g2.DrawEllipse
                pControl.Image = b;
            }
        }


        private Bitmap get_pictemp(String strFile) {
            strFile = strFile.Replace(".\\", Application.StartupPath + "\\");
            Bitmap rtt = new Bitmap(strFile);
            return rtt;
        }


        private Bitmap ImageConvert(Bitmap bmOld) {
            int iwidth = bmOld.Width;
            int iHeight = bmOld.Height;

            Bitmap bmNew = new Bitmap(iwidth, iHeight, PixelFormat.Format24bppRgb);
            Graphics g = Graphics.FromImage(bmNew);
            g.DrawImage(bmOld, new Point(0, 0));
            g.Dispose();
            return bmNew;
        }


        public void screen_save(string strFile) {
            Bitmap bit = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);// 1920, 1080);

            Graphics g = Graphics.FromImage(bit);
            g.CopyFromScreen(new Point(0, 0), new Point(0, 0), new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height));
            g.Dispose();

            bit.Save(strFile);
        }


        public void top_most(int iValue) {
            if (iValue == 1) {
                pFrmApp.TopMost = true;
            } else {
                pFrmApp.TopMost = false;
            }
        }


        public void win_pos(int x,int y) {
            pFrmApp.Left = x;
            pFrmApp.Top = y;
        }


        public string screen_match(string strFile) {


            Bitmap bit = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);// 1920, 1080);

            Graphics g = Graphics.FromImage(bit);
            g.CopyFromScreen(new Point(0, 0), new Point(0, 0), new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height));
            g.Dispose();

            Bitmap retimg2 = bit;

            Bitmap retimg2b = (Bitmap)retimg2.Clone();


            ExhaustiveTemplateMatching pMatchTools = new ExhaustiveTemplateMatching(0.95f);

            Bitmap pFind = get_pictemp(strFile);
            ;
            int MaxDif = 1;// 30;
            Debug.Print(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss fff"));
            List<TemplateMatch> pMatchs = pMatchTools.ProcessImage2(
                ImageConvert(retimg2),
                ImageConvert(get_pictemp(strFile)), 3 * 5, 30, MaxDif);

            Debug.Print(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss fff"));
            Debug.Print(pMatchs.Count.ToString());
            List<TemplateMatch> pMatchs2 = pMatchTools.ProcessImage3(
                ImageConvert(retimg2),
                ImageConvert(get_pictemp(strFile)),
                pMatchs);

            Debug.Print(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss fff"));
            Debug.Print(pMatchs2.Count.ToString());

            if (pMatchs2.Count == 1) {
                TemplateMatch pMatch = pMatchs2[0];
                    //TxOutput.Text = pMatch.Rectangle.ToString();
                    //FrmTip pFrmTip = new FrmTip();
                    //pFrmTip.Show();
                    //pFrmTip.Left = pMatch.Rectangle.X + pFind.Width * 8 / 10;
                    //pFrmTip.Top = pMatch.Rectangle.Y - 100 + pFind.Height / 2;
                return "1:"+ pMatch.Rectangle.X+":"+ pMatch.Rectangle.Y;
            } else if (pMatchs2.Count > 1) {
                return pMatchs2.Count+":找到太多:";
            } else {
                return "0:0:0";
            }

        }

        public void win_active() {
            pFrmApp.WindowState = FormWindowState.Normal;
            pFrmApp.Activate();
        }


        public void win_min() {
            pFrmApp.WindowState = FormWindowState.Minimized;
        }

        public int screen_width() {
            return Screen.PrimaryScreen.Bounds.Width;
        }
        public int screen_height() {
            return Screen.PrimaryScreen.Bounds.Height;
        }

        public void control_box_set(int ivalue) {
            if (ivalue == 1) {
                pFrmApp.ControlBox = true;
                pFrmApp.FormBorderStyle = FormBorderStyle.Sizable;
            } else {
                pFrmApp.ControlBox = false;
                pFrmApp.FormBorderStyle = FormBorderStyle.None;
            }
        }

        public void picturebox_capture_screen(
                string control_name) {
            PictureBox pControl = (PictureBox)Ctrls[control_name];

            if (pControl != null) {


                Bitmap bit = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);

                Graphics g = Graphics.FromImage(bit);

                g.CopyFromScreen(new Point(0, 0), new Point(0, 0), bit.Size);

                g.Dispose();

                pControl.Image = bit;
            }
        }



        public void picturebox_event(
                string control_name,
                string mouse_up_event) {
            PictureBox pControl = (PictureBox)Ctrls[control_name];

            pControl.Tag = mouse_up_event;
            if (pControl != null) {

                pControl.MouseUp += new MouseEventHandler(my_picturebox_mouse_up);//使用事件函数句柄指向一个具体的函数  

            }
        }

        private void my_picturebox_mouse_up(object sender, MouseEventArgs e) {
            PictureBox pBox = (PictureBox)sender;
            pFrmApp.pJS.p_Engine.Invoke(pBox.Tag.ToString(), new[] { e.X,e.Y});
        }

        

        public void combox_init(string name, string text,
            int x, int y,
            int width, int height) {

            Point p = new Point(x, y);//定义一个具体的位置  
            ComboBox pControl = new ComboBox();//实例化一个button  
            pControl.Name = name;
            pControl.Text = text;
            pControl.Location = p;
            pControl.Size = new Size(width, height);
            pFrmApp.Controls.Add(pControl);//向具体的控件中添加
            if (Ctrls.Contains(name) == false) {
                Ctrls.Add(name, pControl);
            } else {
                this.msg("控件已经存在:" + name);
            }
        }



        public void combox_from_file(string control_name, string file) {
            ComboBox pControl = (ComboBox)Ctrls[control_name];
            if (pControl != null) {
                StreamReader pFile = S_File_Text.Read_Begin(file);
                string strLine = S_File_Text.Read_Line(pFile);
                while (strLine != null) {
                    pControl.Items.Add(strLine);
                    strLine = S_File_Text.Read_Line(pFile);
                }
                S_File_Text.Read_End(pFile);
            }
        }


        public void combox_add(string control_name, string text) {
            ComboBox pControl = (ComboBox)Ctrls[control_name];
            if (pControl != null) {
                pControl.Items.Add(text);
            }
        }


        public void combox_clear(string control_name) {
            ComboBox pControl = (ComboBox)Ctrls[control_name];
            if (pControl != null) {
                pControl.Items.Clear();
            }
        }

        public string combox_text(string control_name) {
            ComboBox pControl = (ComboBox)Ctrls[control_name];
            if (pControl != null) {
                return pControl.Text;
            }
            return "";
        }

        public void combox_text_set(string control_name,string value) {
            ComboBox pControl = (ComboBox)Ctrls[control_name];
            if (pControl != null) {
                pControl.Text= value;
            }
        }

        public int combox_index(string control_name) {
            ComboBox pControl = (ComboBox)Ctrls[control_name];
            if (pControl != null) {
                return pControl.SelectedIndex;
            }
            return -1;
        }

        public int combox_event(string control_name,string myEvent) {
            ComboBox pControl = (ComboBox)Ctrls[control_name];
            if (pControl != null) {
                pControl.Tag = new Function_Callback(myEvent, "");
                pControl.SelectedIndexChanged += new EventHandler(this.MyComBox_Change);
            }
            return -1;
        }

        private void MyComBox_Change(object sender, EventArgs e) {
            ComboBox pControl = (ComboBox)sender;
            Function_Callback pFun = (Function_Callback)pControl.Tag;
            pFrmApp.Call_Event(pFun.Name, pFun.Data);
        }

        public void combox_select(string control_name,int index) {
            ComboBox pControl = (ComboBox)Ctrls[control_name];
            if (pControl != null) {
                pControl.SelectedIndex = index;
            }
        }


        public void Run_JS_Dialog(string args, string callback_event) {
            FrmApp pApp2 = new FrmApp();
            string strFile = Application.StartupPath + "\\JS\\" + args;
            if (S_File.Exists(strFile)) {
                pApp2.strFile = strFile;
            }
            
            FrmApp.pTreapFrmApp.insert(strFile,pApp2);

            pApp2.pParent = pFrmApp;
            pApp2.ShowDialog();

            pFrmApp.Call_Event(callback_event, "");
        }

        public void Run_JS_Out(string args) {
            string strPath = Application.StartupPath + "\\FunnyApp.exe";
            using (Process process = new Process()) {
                process.StartInfo.FileName = strPath;
                process.StartInfo.Arguments = Application.StartupPath + "\\JS\\" + args;
                process.Start();
            }
        }

        private IKeyboardMouseEvents m_GlobalHook;

        private string mouse_event = "";
        private Rectangle hook_area;

        public void moue_hook(string mouse_event,
            int x,int y,int width,int height) {
            this.mouse_event = mouse_event;
            // Note: for the application hook, use the Hook.AppEvents() instead
            m_GlobalHook = Hook.GlobalEvents();

            m_GlobalHook.MouseUpExt += GlobalHookMouseUpExt;
            //m_GlobalHook.KeyPress += GlobalHookKeyPress;

            hook_area = new Rectangle(x, y, width, height);
        }

        private void GlobalHookKeyPress(object sender, KeyPressEventArgs e) {
            Console.WriteLine("KeyPress: \t{0}", e.KeyChar);
        }

        private void GlobalHookMouseUpExt(object sender, MouseEventExtArgs e) {
            Console.WriteLine("MouseDown: \t{0}; \t System Timestamp: \t{1}", e.Button, e.Timestamp);
            
            if (hook_area!=null) {
                if (hook_area.Contains(e.X, e.Y)) {
                    e.Handled = true;
                    pFrmApp.Call_Event(this.mouse_event, e.X + "," + e.Y);
                }
            }
        }

        public void moue_unhook() {
            if (m_GlobalHook != null) { 
                m_GlobalHook.MouseDownExt -= GlobalHookMouseUpExt;
                m_GlobalHook.KeyPress -= GlobalHookKeyPress;
            
                m_GlobalHook.Dispose();
            }
        }


        public void Run_JS(string args) {
            FrmApp pApp = new FrmApp();
            string strFile = Application.StartupPath + "\\JS\\" + args;
            if (S_File.Exists(strFile)) {
                pApp.strFile = strFile;
            }
            
            FrmApp.pTreapFrmApp.insert(strFile,pApp);

            pApp.pParent = pFrmApp;
            //pFrmApp.pIndex = pApp.pIndex;

            pApp.Show();
        }

        /// <summary>
        /// 运行exe，并且访问运行结果
        /// </summary>
        /// <param name="cmd">命令</param>
        /// <returns></returns>
        public string Run_App_Return(string cmds, string args) {
            string error = "";
            string output = Run(cmds, args, out error);

            return output;
        }

        public string Run(string path, string args, out string error) {
            try {
                using (Process process = new Process()) {
                    process.StartInfo.FileName = path;
                    process.StartInfo.Arguments = args;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.Start();
                    process.WaitForExit();
                    error = process.StandardError.ReadToEnd();
                    if (process.ExitCode != 0) {
                        return string.Empty;
                    }
                    return process.StandardOutput.ReadToEnd().Trim().Replace(@"\\", @"\");
                }
            } catch (Exception exception) {
                error = string.Format("Calling {0} caused an exception: {1}.", path, exception.Message);
                return string.Empty;
            }
        }



        public void Run_App(string cmds, string args) {
            string strPath = cmds;
            using (Process process = new Process()) {
                process.StartInfo.FileName = strPath;
                process.StartInfo.Arguments = args;
                process.Start();
            }
        }


        public void splitcontainer_init(
            string name,int x,int y,
            int width,int height,
            string Type) {
            Point p = new Point(x, y);//定义一个具体的位置  
            SplitContainer pControl = new SplitContainer();//实例化一个button  
            pControl.Name = name;
            pControl.Location = p;
            pControl.Size = new Size(width, height);
            pControl.Dock = DockStyle.Fill;
            switch (Type.ToLower()) {
                case "v":
                    pControl.Orientation = Orientation.Vertical;
                    break;
                case "h":
                    pControl.Orientation = Orientation.Horizontal;
                    break;
            }
            pControl.SplitterDistance = 107;
            pFrmApp.Controls.Add(pControl);//向具体的控件中添加
            if (Ctrls.Contains(name) == false) {
                Ctrls.Add(name, pControl);
            } else {
                this.msg("控件已经存在:" + name);
            }
        }

        public void splitcontainer_distance(
            string name,
            int distance) {
            SplitContainer pControl = (SplitContainer)Ctrls[name];
            if (pControl != null) {
                pControl.SplitterDistance = distance;
            }

        }

        public void splitcontainer_add(
            string name,
            int index,string name2,
            string fillType) {
            SplitContainer pControl = (SplitContainer)Ctrls[name];
            if (pControl != null) {
                Control pControl2 = (Control)Ctrls[name2];
                if (pControl2 != null) {
                    if (index == 0) {
                        pControl.Panel1.Controls.Add(pControl2);
                    } else {
                        pControl.Panel2.Controls.Add(pControl2);
                    }
                    switch (fillType.ToLower()) {
                        case "fill":
                            pControl2.Dock = DockStyle.Fill;
                            break;
                        case "top":
                            pControl2.Dock = DockStyle.Top ;
                            break;
                        case "left":
                            pControl2.Dock = DockStyle.Left;
                            break;
                        case "right":
                            pControl2.Dock = DockStyle.Right;
                            break;
                        case "bottom":
                            pControl2.Dock = DockStyle.Bottom;
                            break;
                        case "none":
                            pControl2.Dock = DockStyle.None ;
                            break;
                    }
                }
            }

        }



        public void control_dock(string name,
            string fillType) {

            Control pControl = (Control)Ctrls[name];
            if (pControl != null) {

                switch (fillType.ToLower()) {
                    case "fill":
                        pControl.Dock = DockStyle.Fill;
                        break;
                    case "top":
                        pControl.Dock = DockStyle.Top;
                        break;
                    case "left":
                        pControl.Dock = DockStyle.Left;
                        break;
                    case "right":
                        pControl.Dock = DockStyle.Right;
                        break;
                    case "bottom":
                        pControl.Dock = DockStyle.Bottom;
                        break;
                    case "none":
                        pControl.Dock = DockStyle.None;
                        break;
                }
            }
        }

        public void status_label_init(
            string name,string text,
            int width, int height) {

            ToolStripStatusLabel pControl = new ToolStripStatusLabel();//实例化一个button  
            pControl.Name = name;
            pControl.Text = text;
            pControl.Size = new Size(width, height);

            if (Ctrls.Contains(name) == false) {
                Ctrls.Add(name, pControl);
            } else {
                this.msg("控件已经存在:" + name);
            }
        }

        public void status_label_show(
            string name, string text) {

            ToolStripStatusLabel pControl = (ToolStripStatusLabel)Ctrls[name];
            if (pControl != null) {
                pControl.Text = text;
            }
        }

        public void status_init(string name,
            int x, int y,
            int width, int height,string fillType) {

            Point p = new Point(x, y);//定义一个具体的位置  
            StatusStrip pControl = new StatusStrip();//实例化一个button  
            pControl.Name = name;
            pControl.Location = p;
            pControl.Size = new Size(width, height);

            switch (fillType.ToLower()){
                case "fill":
                    pControl.Dock = DockStyle.Fill;
                    break;
                case "top":
                    pControl.Dock = DockStyle.Top;
                    break;
                case "left":
                    pControl.Dock = DockStyle.Left;
                    break;
                case "right":
                    pControl.Dock = DockStyle.Right;
                    break;
                case "bottom":
                    pControl.Dock = DockStyle.Bottom;
                    break;
                case "none":
                    pControl.Dock = DockStyle.None;
                    break;
            }
            pFrmApp.Controls.Add(pControl);//向具体的控件中添加
            if (Ctrls.Contains(name) == false) {
                Ctrls.Add(name, pControl);
            } else {
                this.msg("控件已经存在:" + name);
            }
            
        }

        public void status_add(
            string name, string name2,
            string fillType) {
            StatusStrip pControl = (StatusStrip)Ctrls[name];
            if (pControl != null) {
                ToolStripItem pControl2 = (ToolStripItem)Ctrls[name2];
                if (pControl2 != null) {
                    pControl.Items.Add(pControl2);

                    switch (fillType.ToLower()) {
                        case "fill":
                            pControl2.Dock = DockStyle.Fill;
                            break;
                        case "top":
                            pControl2.Dock = DockStyle.Top;
                            break;
                        case "left":
                            pControl2.Dock = DockStyle.Left;
                            break;
                        case "right":
                            pControl2.Dock = DockStyle.Right;
                            break;
                        case "bottom":
                            pControl2.Dock = DockStyle.Bottom;
                            break;
                        case "none":
                            pControl2.Dock = DockStyle.None;
                            break;
                    }
                }
            }
        }


        public void panel_init(string name,
            int x, int y,
            int width, int height,
            string fillType) {

            Point p = new Point(x, y);//定义一个具体的位置  
            Panel pControl = new Panel();//实例化一个button  
            pControl.Name = name;
            pControl.Location = p;
            pControl.Size = new Size(width, height);

            switch (fillType.ToLower()) {
                case "fill":
                    pControl.Dock = DockStyle.Fill;
                    break;
                case "top":
                    pControl.Dock = DockStyle.Top;
                    break;
                case "left":
                    pControl.Dock = DockStyle.Left;
                    break;
                case "right":
                    pControl.Dock = DockStyle.Right;
                    break;
                case "bottom":
                    pControl.Dock = DockStyle.Bottom;
                    break;
                case "none":
                    pControl.Dock = DockStyle.None;
                    break;
            }
            pFrmApp.Controls.Add(pControl);//向具体的控件中添加
            if (Ctrls.Contains(name) == false) {
                Ctrls.Add(name, pControl);
            } else {
                this.msg("控件已经存在:" + name);
            }
        }

        public void panel_add(
            string name, string name2,
            string fillType) {
            Panel pControl = (Panel)Ctrls[name];
            if (pControl != null) {
                Control pControl2 = (Control)Ctrls[name2];
                if (pControl2 != null) {
                    pControl.Controls.Add(pControl2);

                    switch (fillType.ToLower()) {
                        case "fill":
                            pControl2.Dock = DockStyle.Fill;
                            break;
                        case "top":
                            pControl2.Dock = DockStyle.Top;
                            break;
                        case "left":
                            pControl2.Dock = DockStyle.Left;
                            break;
                        case "right":
                            pControl2.Dock = DockStyle.Right;
                            break;
                        case "bottom":
                            pControl2.Dock = DockStyle.Bottom;
                            break;
                        case "none":
                            pControl2.Dock = DockStyle.None;
                            break;
                    }
                }
            }
        }


        public void panel_autoscroll(
            string name, string Value) {
            Panel pControl = (Panel)Ctrls[name];
            if (pControl != null) {
                pControl.AutoScroll = (Value == "1");
            }
        }



        public void text_set(string control_name, string text) {
            TextBox pControl = (TextBox)Ctrls[control_name];
            if (pControl != null) pControl.Text = text;
        }



        public void font_size(string control_name, float iSize) {
            Control pControl = (Control)Ctrls[control_name];
            if (pControl != null) {
                pControl.Font = new Font("宋体", iSize, FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));

            }
        }
        


        public string text_read(string control_name) {
            TextBox pControl = (TextBox)Ctrls[control_name];
            if (pControl != null) {
                return pControl.Text;// = text;
            } else {
                return "";
            }
        }


        public void text_read_only(
                string control_name,
                int iRead) {
            TextBox pControl = (TextBox)Ctrls[control_name];
            if (pControl != null) {
                if (iRead == 1) {
                    pControl.ReadOnly = true;
                } else {
                    pControl.ReadOnly = false;
                }
            }
        }


        private void my_button_Click(object sender, EventArgs e) {
            Button button = (Button)sender;
            Function_Callback p = (Function_Callback)button.Tag;
            pFrmApp.pJS.p_Engine.Invoke(p.Name, p.Data);
        }

        public void show_form(int width, int height) {
            pFrmApp.Width = width;
            pFrmApp.Height = height;
            pFrmApp.Left = Screen.PrimaryScreen.WorkingArea.Left+(Screen.PrimaryScreen.WorkingArea.Width - width) / 2;
            pFrmApp.Top = Screen.PrimaryScreen.WorkingArea.Top+(Screen.PrimaryScreen.WorkingArea.Height - height) / 2;
            pFrmApp.Show();
            pFrmApp.WindowState = FormWindowState.Normal;
        }


        public void show_form_pos(int x,int y,int width, int height) {
            pFrmApp.Width = width;
            pFrmApp.Height = height;
            pFrmApp.Left = x;// Screen.PrimaryScreen.WorkingArea.Left + (Screen.PrimaryScreen.WorkingArea.Width - width) / 2;
            pFrmApp.Top = y;// Screen.PrimaryScreen.WorkingArea.Top + (Screen.PrimaryScreen.WorkingArea.Height - height) / 2;
            pFrmApp.Show();
            pFrmApp.WindowState = FormWindowState.Normal;
        }

        public void Notification(string title, string message) {
            pFrmApp.Call_Notifiction(title, message);
        }


        public void ShowInTask(string value) {
            pFrmApp.ShowInTaskbar = (value == "1");
        }

    }
}
