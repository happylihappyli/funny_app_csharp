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
                pApp.File = args[0];
            } else {
                string strFile = Application.StartupPath + "/main.js";
                if (S_File.Exists(strFile)) {
                    pApp.File = strFile;
                }
            }
            Application.Run(pApp);


        }
    }
}
