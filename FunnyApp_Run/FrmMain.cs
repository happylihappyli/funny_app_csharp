using B_File.Funny;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
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
            run();
            //JS.Run_Code(strCode);
        }

        public void run() {
            if (listBox1.SelectedItem == null) {
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

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e) {
            run();
        }


        public static void SetAssociation(string Extension, string KeyName, string OpenWith, string FileDescription) {
            // The stuff that was above here is basically the same

            // Delete the key instead of trying to change it
            RegistryKey CurrentUser = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FileExts\\" + Extension, true);
            CurrentUser.DeleteSubKey("UserChoice", false);
            CurrentUser.Close();

            // Tell explorer the file association has been changed
            SHChangeNotify(0x08000000, 0x0000, IntPtr.Zero, IntPtr.Zero);
        }

        [DllImport("shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);



        private void button2_Click(object sender, EventArgs e) {
            //SetAssociation(".js", "JSFile", Application.StartupPath + "\\FunnyApp2.exe", "funny_app File");

            FileAssociations.SetAssociation(".js", "JSFile", "funny_app File", Application.StartupPath + "\\FunnyApp.exe");//, Application.StartupPath + "\\FunnyApp.exe");
        }
    }
}
