using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunnyApp.Function {
    public class C_Time {


        public string Time_Now() {
            //
            return DateTime.Now.ToLongTimeString();
        }


        public string Date_Now() {
            //
            return DateTime.Now.ToLongDateString();
        }
    }
}
