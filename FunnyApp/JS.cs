using FunnyApp.Function;
using System;
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

            jint.SetValue("s_sys", pApp.sys);
            jint.SetValue("s_ui", pApp.pUI);
            jint.SetValue("s_math", new S_Math());
            jint.SetValue("s_string", new C_String());
            jint.SetValue("s_net", new C_Net(pApp));
            jint.SetValue("s_time", new C_Time(pApp));
            jint.SetValue("s_index", FrmApp.pIndex);// new C_Index(pApp));
            jint.SetValue("s_xml", new C_XML());
            jint.SetValue("s_file", new C_File(pApp));
            jint.SetValue("s_au3", new C_AU3(pApp));
            jint.SetValue("s_tcp", new C_TCP(pApp));


            
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
