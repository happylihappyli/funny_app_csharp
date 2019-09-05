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
            C_File pFile = (C_File)listBox1.SelectedItem;
            string strLine = pFile.File;
            if (strLine.StartsWith("/")){
                strLine = strLine.Substring(1) + "\\index.js";
            }

            string strFile = Application.StartupPath + "\\JS\\" + strLine;

            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = textBox1.Text;
            proc.StartInfo.Arguments = strFile;

            proc.StartInfo.UseShellExecute = true;
            proc.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = Application.StartupPath + "\\FunnyApp.exe";
            ArrayList
            pList = S_Dir.ListDir(Application.StartupPath + "/JS/");
            for (int i = 0; i < pList.Count; i++) {
                string strFile = pList[i].ToString();
                if (strFile.ToLower().Equals("data") == false) {
                    string Name = "【"+ translate(strFile)+ "】";
                    listBox1.Items.Add(new C_File(Name, "/" + strFile));
                }
            }

            pList =S_Dir.ListFile(Application.StartupPath + "/JS/");
            for (int i = 0; i < pList.Count; i++)
            {
                string strFile = pList[i].ToString();
                if (strFile.EndsWith(".js")) {
                    listBox1.Items.Add(new C_File(strFile, strFile));
                }
            }
            listBox1.DisplayMember = "Name";
        }

        private string translate(string strFile) {
            string strReturn = "";
            switch (strFile.ToLower()) {
                case "calculate":
                    strReturn = "计算器";
                    break;
                case "clock":
                    strReturn = "闹钟";
                    break;
                case "funnyfav":
                    strReturn = "加密收藏夹";
                    break;
                case "funnygrid":
                    strReturn = "电子表格";
                    break;
                default:
                    strReturn = strFile;
                    break;
            }
            return strReturn;
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
