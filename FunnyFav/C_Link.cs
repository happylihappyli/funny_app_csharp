using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunnyFav {
    public class C_Link {
        private string name = "";
        private string url = "";

        public C_Link(string name,string url) {
            this.name = name;
            this.url = url;
        }

        public string URL {
            get { return this.url; }
            set { this.url=value; }
        }
        public string Name {
            get { return this.name; }
            set { this.name = value; }
        }

        public string Display {
            get { return this.name + "  " + this.url; }
        }
    }
}
