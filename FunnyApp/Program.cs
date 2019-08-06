using B_Data.Funny;
using B_File.Funny;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FunnyApp
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)  
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            FrmApp pApp = new FrmApp();
            if (args.Length > 0) { 
                string strFile= args[0];
                if (strFile.StartsWith("@")) {
                    strFile = Application.StartupPath + "\\JS\\" + strFile.Substring(1);
                }
                pApp.strFile = strFile;
                IComparable pKey = new C_K_Str(strFile);
                FrmApp.pTreapFrmApp.insert(ref pKey, ref pApp);

            } else {
                string strFile = Application.StartupPath + "/main.js";
                if (S_File.Exists(strFile)) {
                    pApp.strFile = strFile;
                }
                IComparable pKey = new C_K_Str(strFile);
                FrmApp.pTreapFrmApp.insert(ref pKey, ref pApp);
            }
            Application.Run(pApp);


        }
    }
}
