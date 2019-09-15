using B_Data.Funny;
using B_File.Funny;
using B_Segmentation.com.Funny.Segmentation;
using B_String.Funny;
using B_TreapVB.TreapVB;
using Lucene.Net.Analysis;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FunnyApp.Function {
    public class C_Index {

        private FSDirectory luceneIndexDirectory = null;
        private Analyzer analyzer = new WhitespaceAnalyzer();
        private IndexWriter writer = null;
        private C_Segmentation pSeg = null;
        private string Seg_Path = "";

        public FrmApp pFrmApp = null;
        private string callback_init = "";

        public C_Index(FrmApp pFrmApp) {
            this.pFrmApp = pFrmApp;
        }

        public void Init_Seg(string strPath,string callback_init) {
            this.callback_init = callback_init;
            this.Seg_Path = strPath;
            //初始化
            Thread thread = new Thread(Init_Seg_Sub);
            thread.Start();
        }

        public void Init_Seg_Sub() {
            pSeg = new C_Segmentation();

            pSeg.pTreapWord = new C_Treap_Funny<Treap<C_Word_Seg>>();


            //网络的词汇
            ArrayList pList = S_Dir.ListFile(Seg_Path + "\\");

            for (int i = 0; i < pList.Count; i++) {
                string strFile = Seg_Path + "\\" + pList[i];
                if (strFile.EndsWith(".txt")) {
                    StreamReader pReader = S_File_Text.Read_Begin(strFile);

                    string strLine = "";
                    while (pReader.Peek() != -1) {
                        strLine = S_File_Text.Read_Line(pReader);
                        pSeg.readLine_fromDic(strLine);
                    }
                }
            }

            pFrmApp.Call_Event(this.callback_init, "");
        }


        public void Create_Start(string indexPath,bool bNew) {

            luceneIndexDirectory = FSDirectory.Open(indexPath);
            writer = new IndexWriter(luceneIndexDirectory, analyzer, bNew, IndexWriter.MaxFieldLength.UNLIMITED);
        }

        public string Seg(string Content) {
            return pSeg.Segmentation_List(Content, false);
        }

        public void Add_Document(int ID, string Content) {

            Document doc = new Document();
            doc.Add(new Field("ID", ID+"", Field.Store.YES, Field.Index.NOT_ANALYZED));
            string strLine = pSeg.Segmentation_List(Content, false);
            doc.Add(new Field("Content", strLine, Field.Store.YES, Field.Index.ANALYZED));
            writer.AddDocument(doc);
        }


        public void Remove_Document(int ID) {

            writer.DeleteDocuments(new Term("ID",ID+""));
        }


        public void Create_End() { 

            writer.Optimize();
            writer.Flush(true, true, true);
            writer.Dispose();
            luceneIndexDirectory.Dispose();
        }

        public string Search(string indexPath,string searchTerm) {
            luceneIndexDirectory = FSDirectory.Open(indexPath);

            IndexSearcher searcher = new IndexSearcher(luceneIndexDirectory);

            QueryParser parser = new QueryParser(
                Lucene.Net.Util.Version.LUCENE_30, "Content", analyzer);

            Query query = parser.Parse(searchTerm);

            ScoreDoc[] hitsFound = searcher.Search(query, 10).ScoreDocs;

            string xml = "";
            for (int i = 0; i < hitsFound.Length; i++) {
                ScoreDoc pScore = hitsFound[i];

                Document doc = searcher.Doc(pScore.Doc);
                xml +="<item>" +
                    "<id>"+ doc.Get("ID")+ "</id>\n" +
                    "<content>" + S_Strings.CData_Encode(doc.Get("Content")) + "</content>\n" +
                    "</item>\n";
            }

            return "<data>"+xml+"</data>";
        }

    }
}
