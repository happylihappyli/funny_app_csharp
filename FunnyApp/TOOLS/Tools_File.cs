using B_File.Funny;
using B_IniFile;
using CS_Encrypt;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FunnyApp {
    public partial class Tools {

        public bool File_Exists(string strFile) {
            return S_File.Exists(strFile);
        }

        public void File_Save(string strFile, string content) {
            if (strFile.StartsWith("@")) {
                strFile = strFile.Replace("@", Path_JS());
            }
            S_File_Text.Write(strFile, content, false, false);
        }

        public void File_Append(string strFile, string content) {
            S_File_Text.Write(strFile, content, true, false);
        }

        public string File_Read(string strFile) {
            if (strFile.StartsWith("@")) {
                strFile = strFile.Replace("@", Path_JS());
            }

            if (S_File.Exists(strFile)) { 
                return File.ReadAllText(strFile);
            } else {
                return "";
            }
        }


        public string File_Open() {
            OpenFileDialog p = new OpenFileDialog();
            p.ShowDialog();
            return p.FileName;
        }

        public string File_Short_Name(string strFile) {
            return Path.GetFileName(strFile);
        }

        public string Ini_Read(string file,string section,string key) {
            if (S_File.Exists(file) == false) {
                return "";
            }
            IniFile pIni = new IniFile(file);
            return  pIni.Read_Item(section, key);
        }
        public void Ini_Save(string file, string section, string key,string value) {
            IniFile pIni = new IniFile(file);
            pIni.AddSection(section);
            pIni.AddKey(key, value,section);
            pIni.Save();
        }

        public void Open_Fold(String strPath) {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo() {
                FileName = strPath,
                UseShellExecute = true,
                Verb = "open"
            });
        }


        public string AES_Encrypt(string strLines,string password) {
            return AES.Encrypt(strLines, password);
        }

        public string AES_Decrypt(string strLines, string password) {
            return AES.Decrypt(strLines, password);
        }
    }
}
