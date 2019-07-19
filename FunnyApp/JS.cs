using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FunnyApp
{
    class JS
    {
         
        public static Jint.Engine jint;

        public static void Run_Code(FrmApp pApp,string run_codestr1)
        {
            Tools tools = new Tools(pApp);


            jint = new Jint.Engine().SetValue("sys", tools);

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
