using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunnyApp_Run {
    public class C_File {
        private string name = "";
        private string file = "";

        public C_File(string Name, string File) {
            this.name = Name;
            this.file = File;
        }

            
        public string Name {
            get { return this.name; }
        }
        public string File {
            get { return this.file; }
        }
    }
}
