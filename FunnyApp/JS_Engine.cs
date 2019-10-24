using FunnyApp.Function;
using System;
using System.Windows.Forms;

namespace FunnyApp
{
    public class JS_Engine
    {
        public Jint.Engine p_Engine;

        public void Run_Code(FrmApp pApp,string run_codestr1)
        {
            p_Engine = new Jint.Engine();

            
            //非静态变量
            p_Engine.SetValue("s_sys", pApp.pSYS);
            p_Engine.SetValue("s_ui", pApp.pUI);


            //静态变量，只有一个
            p_Engine.SetValue("s_tcp", FrmApp.pTCP);
            p_Engine.SetValue("s_index", FrmApp.pIndex);// new C_Index(pApp));

            //其他
            p_Engine.SetValue("s_math", new S_Math());
            p_Engine.SetValue("s_string", new C_String());
            p_Engine.SetValue("s_net", new C_Net(pApp));
            p_Engine.SetValue("s_time", pApp.pTime);
            p_Engine.SetValue("s_xml", new C_XML());
            p_Engine.SetValue("s_file", new C_File(pApp));
            p_Engine.SetValue("s_au3", new C_AU3(pApp));


            try
            {
                p_Engine.Execute(run_codestr1);
            }
            catch (Exception eee)
            {
                MessageBox.Show("error in script run:" + eee.Message + eee.StackTrace);
            }
        }
    }
}
