using B_File.Funny;
using CS_Encrypt;
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

namespace FunnyFav {
    public partial class FrmMain : Form {
        public FrmMain() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            string name = textBox1.Text;
            string url = textBox2.Text;
            C_Link pLink = new C_Link(name,url);
            listBox1.Items.Add(pLink);
            listBox1.DisplayMember = "Display";
            save_bookmarks();
        }

        private void button3_Click(object sender, EventArgs e) {
            
        }

        public void save_bookmarks() {
            
            if (Program.password == "") {
                MessageBox.Show("密码为空！");
                return;
            }
            string strLines = "";
            for (int i = 0; i < listBox1.Items.Count ; i++) {
                C_Link pLink = (C_Link)listBox1.Items[i];
                strLines += pLink.Name + "\t" + pLink.URL + "\n";
            }
            strLines=AES.Encrypt(strLines, Program.password);
            string strFile = Program.file;
            S_File_Text.Write(strFile, strLines,false);
        }

        public void read_file() {
            string strFile = Program.file;
            if (S_File.Exists(strFile) == false) {
                return;
            }

            string strLines = S_File_Text.Read(strFile);
            strLines = AES.Decrypt(strLines, Program.password);
            string[] strSplit = strLines.Split('\n');
            for (int i = 0; i < strSplit.Length; i++) {
                string[] strSplit2 = strSplit[i].Split('\t');
                if (strSplit2.Length > 1) {
                    C_Link pLink = new C_Link(strSplit2[0], strSplit2[1]);
                    listBox1.Items.Add(pLink);
                }
            }
            listBox1.DisplayMember = "Display";
        }


        private void listBox1_DoubleClick(object sender, EventArgs e) {
            C_Link pLink = (C_Link)listBox1.SelectedItem;
            string url=pLink.URL;
            Process.Start(@"chrome.exe", "--incognito "+ url);
        }

        private void FrmMain_Load(object sender, EventArgs e) {
            FrmLogin pLogin = new FrmLogin();
            pLogin.ShowDialog();

            if (Program.password == "") {
                return;
            }

            read_file();
        }

        private void readToolStripMenuItem_Click(object sender, EventArgs e) {
            read_file();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e) {
            save_bookmarks();
        }

        private void button2_Click_1(object sender, EventArgs e) {
            textBox1.Text = "";
            textBox2.Text = "";
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e) {
            listBox1.Items.Remove(listBox1.SelectedItem);
            save_bookmarks();
        }
    }
}
