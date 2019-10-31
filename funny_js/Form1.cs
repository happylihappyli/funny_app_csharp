using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace funny_js {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            
            string js_script = "var a=1+2*3;";


            ECMAScriptLexer lexer = new ECMAScriptLexer(CharStreams.fromstring(js_script));
            ECMAScriptParser parser = new ECMAScriptParser(new CommonTokenStream(lexer));
            ECMAScriptParser.ProgramContext ctx = parser.program();
            //if (JavaMain.bDebug) {
            //    new TreeViewer(Arrays.asList(ECMAScriptParser.ruleNames), ctx).open();
            //}

            MyVisitor visitor = new MyVisitor();
            visitor.Visit(ctx);
        }
    }
}
