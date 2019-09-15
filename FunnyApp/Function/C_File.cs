using B_Data.Funny;
using B_File.Funny;
using B_IniFile;
using CS_Encrypt;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FunnyApp {
    public class C_File {
        FrmApp pFrmApp = null;
        public C_File(FrmApp pFrmApp) {
            this.pFrmApp = pFrmApp;
        }

        public bool File_Exists(string strFile) {
            return S_File.Exists(strFile);
        }

        public void delete(string strFile) {
            if (S_File.Exists(strFile))
            S_File.Recycle(strFile);
        }

        public void save(string strFile, string content) {
            if (strFile.StartsWith("@")) {
                strFile = strFile.Replace("@", pFrmApp.sys.Path_JS());
            }
            S_File_Text.Write(strFile, content, false, false);
        }

        public void append(string strFile, string content) {
            S_File_Text.Write(strFile, content, true, false);
        }

        public void copy(string strFile,string strFile2,bool bOverWrite) {
            File.Copy(strFile, strFile2,bOverWrite);
        }

        [Obsolete("use read(xxx) instead")]
        public string File_Read(string strFile) {
            return read(strFile);
        }
        public string read(string strFile) {
            if (strFile.StartsWith("@")) {
                strFile = strFile.Replace("@", pFrmApp.sys.Path_JS());
            }

            if (S_File.Exists(strFile)) { 
                return File.ReadAllText(strFile);
            } else {
                return "";
            }
        }

        public void Read_Begin(string file,string key) {
            object pReader= S_File_Text.Read_Begin(file);
            FrmApp.pMap.insert(key, pReader);
        }

        public string Read_Line(string key) {

            StreamReader pReader = (StreamReader)FrmApp.pMap.find(key);
            string strLine= S_File_Text.Read_Line(pReader);
            return strLine;
        }

        public void Read_End(string key) {

            StreamReader pReader = (StreamReader)FrmApp.pMap.find(key);
            S_File_Text.Read_End(pReader);
        }


        public string File_List_File(string strDir) {
            string strReturn = "";
            ArrayList pArrayList=S_Dir.ListFile(strDir);
            for(int i=0;i< pArrayList.Count; i++) {
                strReturn += pArrayList[i]+"|";
            }
            if (strReturn.EndsWith("|")) {
                strReturn = strReturn.Substring(0, strReturn.Length - 1);
            }
            return strReturn;
        }

        public string File_List_Dir(string strDir) {
            string strReturn = "";
            ArrayList pArrayList = S_Dir.ListDir(strDir);
            for (int i = 0; i < pArrayList.Count; i++) {
                strReturn += pArrayList[i] + "|";
            }
            if (strReturn.EndsWith("|")) {
                strReturn = strReturn.Substring(0, strReturn.Length - 1);
            }
            return strReturn;
        }



        public long Size(string file) {
            FileInfo pInfo = new System.IO.FileInfo(file);

            return pInfo.Length;
        }

        public string Bin_Read(string strFile,int start,int size) {
            
            FileStream fs;
            BinaryReader br;

            byte[] buffer = new byte[size];

            try { 
                fs = new FileStream(strFile, FileMode.Open);           //获取流对象
                br = new BinaryReader(fs);
                fs.Seek(start,SeekOrigin.Begin); //二进制读取
                br.Read(buffer, 0, size);
                br.Close();
                fs.Close();
            }
            catch(Exception ex) {
                ex.ToString();
            }

            return Convert.ToBase64String(buffer);
        }

        public int Bin_Write(string strFile,int start,string base64) {
            byte[] buffer = Convert.FromBase64String(base64);


            FileStream fs;
            BinaryWriter bw;
            
            fs = new FileStream(strFile, FileMode.OpenOrCreate);           //获取流对象
            bw = new BinaryWriter(fs);
            bw.Seek(start, SeekOrigin.Begin);
            bw.Write(buffer,0,buffer.Length);
            bw.Close();
            fs.Close();
            return buffer.Length;
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
            pIni.DeleteKey(key,section);
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


    }
}
