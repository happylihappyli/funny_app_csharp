﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FunnyApp
{
    public class JS
    {
        public Jint.Engine jint;

        public void Run_Code(FrmApp pApp,string run_codestr1)
        {
            //pApp.tools = new Tools(pApp);
            jint = new Jint.Engine();
            jint.SetValue("sys", pApp.tools);
            jint.SetValue("s_sys", pApp.tools);
            jint.SetValue("s_ui", pApp.ui);
            jint.SetValue("s_math", new S_Math());

            try
            {
                jint.Execute(run_codestr1);
            }
            catch (Exception eee)
            {
                MessageBox.Show("error in script run:" + eee.Message + eee.StackTrace);
            }
        }
    }
}
