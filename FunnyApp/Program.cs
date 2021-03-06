﻿using B_Data.Funny;
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
            pApp.args = args;
            if (args.Length > 0) { 
                string strFile= args[0];
                if (strFile.StartsWith("@")) {
                    strFile = Application.StartupPath + "\\JS\\" + strFile.Substring(1);
                }
                pApp.strFile = strFile;
                FrmApp.pTreapFrmApp.insert(strFile, pApp);

            } else {
                string strFile = Application.StartupPath + "/main.js";
                if (S_File.Exists(strFile)) {
                    pApp.strFile = strFile;
                }
                FrmApp.pTreapFrmApp.insert(strFile, pApp);
            }
            Application.Run(pApp);


        }
    }
}
