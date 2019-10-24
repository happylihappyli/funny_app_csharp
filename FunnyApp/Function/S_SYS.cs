using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunnyApp.Function {
    public class S_SYS {


        public static void beep(int frequency = 1000, int duration = 400, int n = 5) {

            for (int i = 1; i < n; i++) {
                Console.Beep(frequency, duration);
            }
        }

    }
}
