using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunnyApp
{
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Media;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;

    namespace FunnyApp
    {

        class baidu_tts
        {
            public string fileName = "";
            public string text = "";

            private static string tok = "";



            public static string para_API_key = "l9LlAn6wIWPLnVKyNN7hIf4A";

            public static string para_API_secret_key = "KQDopzedVFtpwGKdB4NqdDdrBUaZxcXQ";


            public static string gettoken()
            {

                //方法参数说明:
                //para_API_key:API_key(你的KEY)
                //para_API_secret_key(你的SECRRET_KEY)

                //方法返回值说明:
                //百度认证口令码,access_token
                string access_html = null;
                string access_token = null;
                string getAccessUrl = "https://openapi.baidu.com/oauth/2.0/token?grant_type=client_credentials" +
               "&client_id=" + para_API_key + "&client_secret=" + para_API_secret_key;
                try
                {
                    System.Net.HttpWebRequest getAccessRequest = System.Net.WebRequest.Create(getAccessUrl) as System.Net.HttpWebRequest;
                    //getAccessRequest.Proxy = null;
                    getAccessRequest.ContentType = "multipart/form-data";
                    getAccessRequest.Accept = "*/*";
                    getAccessRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727)";
                    getAccessRequest.Timeout = 30000;//30秒连接不成功就中断 
                    getAccessRequest.Method = "post";

                    System.Net.HttpWebResponse response = getAccessRequest.GetResponse() as System.Net.HttpWebResponse;
                    using (StreamReader strHttpComback = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        access_html = strHttpComback.ReadToEnd();
                    }
                }
                catch (System.Net.WebException ex)
                {
                    Console.Write(ex.Message);
                    Console.ReadLine();
                }
                JObject jo = JObject.Parse(access_html);
                access_token = jo["access_token"].ToString();//得到返回的toke
                return access_token;
            }


            //private static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";



            public static string EncryptWithMD5(string source)
            {
                byte[] sor = Encoding.UTF8.GetBytes(source);
                MD5 md5 = MD5.Create();
                byte[] result = md5.ComputeHash(sor);
                StringBuilder strbul = new StringBuilder(40);
                for (int i = 0; i < result.Length; i++)
                {
                    strbul.Append(result[i].ToString("x2"));//加密结果"x2"结果为32位,"x3"结果为48位,"x4"结果为64位

                }
                return strbul.ToString();
            }



            public static void DownFile(string uRLAddress, string localPath, string filename)
            {
                System.Net.WebClient client = new System.Net.WebClient();
                Stream str = client.OpenRead(uRLAddress);
                StreamReader reader = new StreamReader(str);
                byte[] mbyte = new byte[100000000];
                int allmybyte = (int)mbyte.Length;
                int startmbyte = 0;

                while (allmybyte > 0)
                {

                    int m = str.Read(mbyte, startmbyte, allmybyte);
                    if (m == 0)
                    {
                        break;
                    }
                    startmbyte += m;
                    allmybyte -= m;
                }

                reader.Dispose();
                str.Dispose();

                //string paths = localPath + System.IO.Path.GetFileName(uRLAddress);
                string path = localPath + filename;
                FileStream fstr = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
                fstr.Write(mbyte, 0, startmbyte);
                fstr.Flush();
                fstr.Close();
            }


            private const string spd = "5";
            private const string pit = "6";
            private const string vol = "9";
            private const string cuid = "00-12-7B-16-74-8D";

            private const string rest = "tex={0}&lan=zh&per=0&ctp=1&cuid={1}&tok={2}&spd={3}&pit={4}&vol=9&aue=6";// 4为pcm-16k；5为pcm-8k；6为wav（内容同pcm-16k）
            public static void tts(string strLine)
            {

                baidu_tts pTTS = new baidu_tts();
                pTTS.text = strLine;
                Thread p = new Thread(pTTS.Play_Sound);
                p.Start();
            }

            public void Play_Sound()
            {
                tok = gettoken();


                text = HttpUtility.UrlEncode(text, Encoding.UTF8);
                text = HttpUtility.UrlEncode(text, Encoding.UTF8);

                string strUpdateData = string.Format(rest, text, cuid, tok, spd, pit);

                string texmd5 = EncryptWithMD5(text) + RandomStr.CreateRadNum(5, true);

                DownFile("http://tsn.baidu.com/text2audio?" + strUpdateData,
                    "D:\\", texmd5 + ".wav");


                this.fileName = "D:\\" + texmd5 + ".wav";

                FileStream fs = new FileStream(this.fileName, FileMode.Open, FileAccess.Read);
                int nBytes = (int)fs.Length;//计算流的长度
                byte[] byteArray = new byte[nBytes];//初始化用于MemoryStream的Buffer
                int nBytesRead = fs.Read(byteArray, 0, nBytes);//将File里的内容一次性的全部读到byteArray中去
                using (MemoryStream ms = new MemoryStream(byteArray))//初始化MemoryStream,并将Buffer指向FileStream的读取结果数组
                {

                    SoundPlayer player = new SoundPlayer();
                    ms.Position = 0;
                    player.Stream = null;
                    player.Stream = ms;
                    player.Load();
                    player.PlaySync();

                }
            }


        }

        public static class RandomStr
        {


            public static string codeSerial = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,0,1,2,3,4,5,6,7,8,9";
            public static string codeSerialn = "0,1,2,3,4,5,6,7,8,9";

            public static string CreateRadNum(int codeLen, bool isonly_num)
            {

                int Length = 4;
                if (codeLen == 0)
                {
                    codeLen = Length;
                }
                string[] arr;
                if (isonly_num == true)
                {

                    arr = codeSerialn.Split(',');

                }
                else
                {

                    arr = codeSerial.Split(',');
                }

                string code = "";

                int randValue = -1;


                for (int i = 0; i < codeLen; i++)
                {
                    randValue = rand.Next(0, arr.Length - 1);

                    code += arr[randValue];
                }

                return code;
            }



            public static Random rand = new Random(unchecked((int)DateTime.Now.Ticks));
            public static Random init()
            {
                rand = new Random(unchecked((int)DateTime.Now.Ticks));
                return rand;
            }

        }

    }

}
