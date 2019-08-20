using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunnyApp {
    public partial class Tools {
        

        public string encrypt_public_key(string file,string strLine){
            //@"D:\Net\Web\id_rsa.pem.pub"
            //string pemstr = File.ReadAllText(@"D:\Net\Web\id_rsa").Trim();
            string aaa = PemKeyUtils.RSAEncrypt(file, strLine);// "hhh,test");
            return aaa;
        }

        public string decrypt_private_key(string file, string strLine){
            try {
                string bbb = PemKeyUtils.RSADecrypt(file, strLine);
                return bbb;
            } catch(Exception ex) {
                return ex.ToString();
            }
        }

        public string Time_Now() {
            //
            return DateTime.Now.ToLongTimeString();
        }


        public string Date_Now() {
            //
            return DateTime.Now.ToLongDateString();
        }

        public string args(int i) {
            return pFrmApp.args[i];
        }
    }
}