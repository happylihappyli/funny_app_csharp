using B_File.Funny;
using Funny;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FunnyFav {
    public partial class FrmLogin : Form {
        public FrmLogin() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            string a = textBox1.Text;
            string b = textBox2.Text;

            if (a==b) {
                Program.password = a;
                Program.file = comboBox1.Text;
                this.Close();
            } else {
                MessageBox.Show("两次密码不一样！");
            }
        }


        public string encrypt_public_key(string file, string strLine) {
            //@"D:\Net\Web\id_rsa.pem.pub"
            //string pemstr = File.ReadAllText(@"D:\Net\Web\id_rsa").Trim();
            string aaa = PemKeyUtils.RSAEncrypt(file, strLine);// "hhh,test");
            return aaa;
        }

        public string decrypt_private_key(string file, string strLine) {
            try {
                string bbb = PemKeyUtils.RSADecrypt(file, strLine);
                return bbb;
            } catch (Exception ex) {
                return ex.ToString();
            }
        }

        private void FrmLogin_Load(object sender, EventArgs e) {
            if (comboBox1.Items.Count > 0) {
                comboBox1.SelectedIndex = 0;
            }
        }

        private void button2_Click(object sender, EventArgs e) {
            string a = textBox1.Text;
            string b = textBox2.Text;

            if (a == b) {
            } else {
                MessageBox.Show("两次密码不一样！");
                return;
            }

            OpenFileDialog pOpen = new OpenFileDialog();
            pOpen.Title = "选择你的公钥文件(pem格式)进行加密";
            if (pOpen.ShowDialog() != DialogResult.OK) {
                return;
            }
            string file = pOpen.FileName;
            string strLine=encrypt_public_key(file, a);
            S_File_Text.Write("D:\\Net\\Web\\password_funnyfav.txt",strLine,false);

        }

        private void button3_Click(object sender, EventArgs e) {
             
            OpenFileDialog pOpen = new OpenFileDialog();
            pOpen.Title = "选择你的私钥文件进行解密";
            if (pOpen.ShowDialog() != DialogResult.OK) {
                return;
            }
            string file = pOpen.FileName;
            string strLine = S_File_Text.Read("D:\\Net\\Web\\password_funnyfav.txt");
            string a=decrypt_private_key(file, strLine);
            MessageBox.Show(a);
        }
    }
}
