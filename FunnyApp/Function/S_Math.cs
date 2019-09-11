using B_Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunnyApp {
    public class S_Math {

        public double Math_Cal(string strLine) {
            C_Math pMath = new C_Math();
            return pMath.EvaluateExpression(strLine);
        }

        public double round(double value) {
            return Math.Round(value);
        }

        public double sqrt(double value) {
            return Math.Sqrt(value);
        }

        public double ln(double value) {
            return Math.Log(value);
        }
    }
}
