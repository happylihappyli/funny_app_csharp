using B_Data.Funny;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FunnyApp.Function {
    public class C_XML {
        public void init(string strXML,string key) {
            XmlDocument pDoc = new XmlDocument();
            pDoc.LoadXml(strXML);
            Object pObj = pDoc;
            IComparable pKey = new C_K_Str("xml:"+key);
            FrmApp.pMap.insert(ref pKey, ref pObj);

        }

        public int count(string key, string xPath) {
            string key2 = "xml:" + key;
            XmlDocument pDoc = (XmlDocument)FrmApp.pMap.find(new C_K_Str(key2));
            return pDoc.SelectNodes(xPath).Count;
        }


        public string read(string key,string xPath) {
            string key2 = "xml:" + key;
            XmlDocument pDoc = (XmlDocument)FrmApp.pMap.find(new C_K_Str(key2));
            return pDoc.SelectNodes(xPath).Item(0).InnerXml;
        }
    }
}
