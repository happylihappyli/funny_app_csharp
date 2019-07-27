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
            string bbb = PemKeyUtils.RSADecrypt(file, strLine);
            //@"D:\Net\Web\id_rsa", "DMYKcU4QQadIKJzhFEFWCFXDa6NebyTP1FKgy89ZXgQRWisNydq9/5FS1wJyfyZVmKg1paIYKW+N5AdMvRCc4boq4TGDpe9tpJJ6OrOiyImbqREYtReS5U5atTP/CCdDNJh26PBlB67M/mYgibBlKRS0aaEQq8vLpw8wIOOh6gZDA+A45+3km/ok4ySV17ugq1LLui5HQ7geMnD4aa9b1HvPKKjMTXCdMgCYyIcgxlKGcdj2LBsfuoNpVUe+NI+M7kfaMOXOoN+bnWRj+uFcBVfmQYMzHcVh4qzqtV3Fydk2t/RzZdGwDnmPgZIefcsWgHxSnhVPovGCNhXsxotKUg==");
            return bbb;
        }
    }
}