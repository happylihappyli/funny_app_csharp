using B_File.Funny;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FunnyApp {
    public partial class Tools {

        public void File_Save(string strFile, string content) {
            S_File_Text.Write(strFile, content, false, false);
        }

        public string File_Read(string strFile) {
            return File.ReadAllText(strFile);
        }

        public string File_Open() {
            OpenFileDialog p = new OpenFileDialog();
            p.ShowDialog();
            return p.FileName;
        }

        public string File_Short_Name(string strFile) {
            return Path.GetFileName(strFile);
        }
    }
}
