using B_File.Funny;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FunnyApp_Run {
    public partial class FrmMain : Form
    {
        public static FrmMain pFrmMain;
        public FrmMain()
        {
            InitializeComponent();
            pFrmMain = this;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("请选择一个节点！");
                return;
            }
            //this.Visible = false;// = FormWindowState.Minimized ;
            String strFile = Application.StartupPath
                + "/JS/" + listBox1.SelectedItem.ToString();

            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = textBox1.Text;
            proc.StartInfo.Arguments = strFile;

            proc.StartInfo.UseShellExecute = true;
            proc.Start();
            //JS.Run_Code(strCode);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = Application.StartupPath + "\\FunnyApp.exe";
            //E:\CloudStation\Robot5\FunnyTeacher\FunnyApp\bin\Debug\FunnyApp.exe
            ArrayList pList=S_Dir.ListFile(Application.StartupPath + "/JS/");
            for (int i = 0; i < pList.Count; i++)
            {
                listBox1.Items.Add(pList[i].ToString());
            }
        }
    }
}
