using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunnyApp.Function {
    public class C_Time {
        public FrmApp pFrmApp = null;
        public C_Time(FrmApp pFrmApp) {
            this.pFrmApp = pFrmApp;
        }

        public string Time_Now() {
            //
            return DateTime.Now.ToLongTimeString();
        }


        public string Date_Now() {
            //
            return DateTime.Now.ToLongDateString();
        }



        public void setTimeout(string strFunction, int iSec,string memo) {
            //SetTimeout(1000 * iSec, delegate {
            //    pFrmApp.Call_Event(strFunction, "");//, data.ToString());
            //});

            var t = Task.Run(async delegate {
                Console.WriteLine("iSec秒");
                await Task.Delay(1000 * iSec);
                if (strFunction.Equals("check_connected")) {
                    Console.WriteLine("iSec秒后会执行此输出语句");
                } else {
                    Console.WriteLine("iSec秒后会执行此输出语句");
                }
                pFrmApp.Call_Event(strFunction, memo);
                //return 42;
            });
        }


        //private static void SetTimeout(double interval, Action action) {
        //    System.Timers.Timer timer = new System.Timers.Timer(interval);
        //    timer.Elapsed += delegate (object sender, System.Timers.ElapsedEventArgs e) {
        //        timer.Enabled = false;
        //        action();
        //    };
        //    timer.Enabled = true;

        //}
    }
}
