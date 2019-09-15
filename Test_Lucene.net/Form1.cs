using B_Data.Funny;
using B_File.Funny;
using B_Segmentation.com.Funny.Segmentation;
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test_Lucene.net {
    public partial class Form1 : Form {

        private FSDirectory luceneIndexDirectory = null;
        private Analyzer analyzer = new WhitespaceAnalyzer();
        private IndexWriter writer = null;
        string indexPath = @"D:\Data\index";
        private C_Segmentation pSeg = null;

        public Form1() {
            InitializeComponent();
        }



        public void Init_Seg(string strPath) {
            //初始化

            pSeg = new C_Segmentation();

            pSeg.pTreapWord = new C_Treap_Funny<Treap<C_Word_Seg>>();


            //网络的词汇
            ArrayList pList = S_Dir.ListFile(strPath + "\\Dic\\Segmentation\\");

            for(int i = 0;i< pList.Count; i++) {
                string strFile= strPath+ "\\Dic\\Segmentation\\" + pList[i];
                if (strFile.EndsWith(".txt")) {
                    StreamReader pReader = S_File_Text.Read_Begin(strFile);

                    string strLine = "";
                    while (pReader.Peek() != -1) {
                        strLine = S_File_Text.Read_Line(pReader);
                        pSeg.readLine_fromDic(strLine);
                    }
                }
            }
        }


        private void button1_Click(object sender, EventArgs e) {
            Init_Seg(@"D:\Funny\FunnyAI\Data");

            luceneIndexDirectory = FSDirectory.Open(indexPath);
            writer = new IndexWriter(luceneIndexDirectory, analyzer,true, IndexWriter.MaxFieldLength.UNLIMITED);


            Document doc = new Document();
            doc.Add(new Field("ID", "1", Field.Store.YES, Field.Index.NOT_ANALYZED));
            string strLine = pSeg.Segmentation_List("你好中国",false);
            doc.Add(new Field("Content", strLine, Field.Store.YES, Field.Index.ANALYZED));
            writer.AddDocument(doc);


            doc = new Document();
            doc.Add(new Field("ID", "2", Field.Store.YES, Field.Index.NOT_ANALYZED));
            strLine = pSeg.Segmentation_List("中国社会", false);
            doc.Add(new Field("Content", strLine, Field.Store.YES, Field.Index.ANALYZED));
            writer.AddDocument(doc);




            doc = new Document();
            doc.Add(new Field("ID", "3", Field.Store.YES, Field.Index.NOT_ANALYZED));
            strLine = pSeg.Segmentation_List("中国是发展中国家", false);
            doc.Add(new Field("Content", strLine, Field.Store.YES, Field.Index.ANALYZED));
            writer.AddDocument(doc);




            writer.Optimize();
            writer.Flush(true,true,true);
            writer.Dispose();
            luceneIndexDirectory.Dispose();
        }

        private void button2_Click(object sender, EventArgs e) {

            string searchTerm = txSearch.Text;

            luceneIndexDirectory = FSDirectory.Open(indexPath);

            IndexSearcher searcher = new IndexSearcher(luceneIndexDirectory);

            QueryParser parser = new QueryParser(
                Lucene.Net.Util.Version.LUCENE_30, "Content", analyzer);

            Query query = parser.Parse(searchTerm);

            ScoreDoc[] hitsFound = searcher.Search(query,10).ScoreDocs;

            string html = "";
            for (int i = 0; i < hitsFound.Length; i++) {
                ScoreDoc pScore = hitsFound[i];

                Document doc =  searcher.Doc(pScore.Doc);
                html += doc.Get("ID") + doc.Get("Content")+"<br>";
            }

            webBrowser1.DocumentText = html;

        }
    }
}
