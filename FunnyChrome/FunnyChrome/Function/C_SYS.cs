using B_IniFile;
using B_Net.Funny;
using B_String.Funny;
using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Funny {


    public class C_SYS {
        //方法名要小写，大写会失败
        public ChromiumWebBrowser browser;
        FrmMain pFrmMain;
        public string user_name = "";
        public string user_md5 = "";

        public C_SYS(FrmMain pFrmMain,ChromiumWebBrowser browser) {
            this.pFrmMain = pFrmMain;
            this.browser = browser;

            IniFile pIni = new IniFile("D:\\Net\\Web\\main.ini");
            user_name = pIni.Read_Item("main", "account");
            user_md5 = pIni.Read_Item("main", "md5");
        }
        public string test() {
            return "aaa";
        }


        public string http_post(string url, string data) {
            string strReturn = "";
            strReturn = S_Net.http_post("", url, data, "POST", "utf-8", "");
            return strReturn;
        }

        public string http_post_add(string url, string data) {
            data += "&email=" + S_Strings.UrlEncode(user_name)
                + "&md5=" + S_Strings.UrlEncode(user_md5);
            string strReturn = "";
            strReturn = S_Net.http_post("", url, data, "POST", "utf-8", "");

            pFrmMain.Display_Post(strReturn);

            return strReturn;
        }



        public string http_post2(string url, string data, string refer) {
            string strReturn = "";
            strReturn = S_Net.http_post("", url, data, "POST", "utf-8", refer);
            return strReturn;
        }


    }
}
