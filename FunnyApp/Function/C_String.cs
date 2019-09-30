using B_String.Funny;
using CS_Encrypt;
using Funny;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunnyApp {
    public class C_String {
        public void json(string strJSON,string key) {
            
            JObject jObject = JObject.Parse(strJSON);
            FrmApp.pMap.insert(key, jObject);
        }

        public void json_array(string strJSON, string key) {
            JArray jArray = JArray.Parse(strJSON);
            FrmApp.pMap.insert(key, jArray);
        }

        public int json_array_length(string key) {
            JArray jArray = (JArray)FrmApp.pMap.find(key);//, jObject);
            return jArray.Count;
        }
        public string json_array_item(string key,int index,string name) {
            JArray jArray = (JArray)FrmApp.pMap.find(key);//, jObject);
            JObject pObj = (JObject)jArray[index];

            return pObj.GetValue(name).ToString();
        }

        public string md5(string strLine) {
            return S_Strings.MD5(strLine);
        }

        public string urlencode(string strLine) {
            return S_Strings.UrlEncode(strLine);
        }
        

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


        public string AES_Encrypt(string strLines, string password) {
            return AES.Encrypt(strLines, password);
        }

        public string AES_Decrypt(string strLines, string password) {
            return AES.Decrypt(strLines, password);
        }
    }
}