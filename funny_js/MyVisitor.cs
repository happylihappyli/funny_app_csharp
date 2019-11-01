using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ECMAScriptParser;

namespace funny_js {
    public class MyVisitor : AbstractParseTreeVisitor<object> {
        public MyVisitor() {
        }

        protected override object DefaultResult => base.DefaultResult;

        public override bool Equals(object obj) {
            return base.Equals(obj);
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }

        public override string ToString() {
            return base.ToString();
        }

        public override object Visit(IParseTree tree) {
            //var a=tree.GetText();
            return base.Visit(tree);
        }

        public double object_2_double(object Express1) {
            double db1 = 0;
            switch (Express1.GetType().ToString()) {
                case "System.Double":
                    db1 = (double)Express1;
                    break;
                case "System.String":
                    db1 = double.Parse((string)Express1);
                    break;
            }
            return db1;
        }

        public override object VisitChildren(IRuleNode node) {
            var type = node.GetType().Name;
            var text = node.GetText();
            Console.WriteLine(type + "==" + text);

            switch (type) {
                case "ProgramContext"://==vara=1+2*3;<EOF>
                    break;
                case "SourceElementsContext":// == vara = 1 + 2 * 3;
                    break;
                case "SourceElementContext":// == vara = 1 + 2 * 3;
                    break;
                case "StatementContext":// == vara = 1 + 2 * 3;
                    break;
                case "VariableStatementContext":// == vara = 1 + 2 * 3;
                    break;
                case "VariableDeclarationListContext":// == a = 1 + 2 * 3
                    break;
                case "VariableDeclarationContext":// == a = 1 + 2 * 3
                    break;
                case "InitialiserContext":// === 1 + 2 * 3
                    break;
                case "AdditiveExpressionContext":// == 1 + 2 * 3
                    {
                        AdditiveExpressionContext pM = (AdditiveExpressionContext)node;
                        SingleExpressionContext p1 = pM.singleExpression(0);
                        SingleExpressionContext p2 = pM.singleExpression(1);
                        if (pM.Plus().ToString().Equals("+")) {
                            object Express1 = this.VisitChildren(p1);
                            object Express2 = this.VisitChildren(p2);

                            double db1 = object_2_double(Express1);
                            double db2 = object_2_double(Express2);

                            return db1 + db2;
                        }
                    }
                    break;
                case "LiteralExpressionContext":// == 1
                    return text;
                case "LiteralContext":// == 1
                    return text;
                case "NumericLiteralContext":// == 1
                    break;
                case "MultiplicativeExpressionContext":// == 2 * 3
                    { 
                        MultiplicativeExpressionContext pM = (MultiplicativeExpressionContext)node;
                        SingleExpressionContext p1 = pM.singleExpression(0);
                        SingleExpressionContext p2 = pM.singleExpression(1);
                        if (pM.Multiply().ToString().Equals("*")) {
                            object Express1 = this.VisitChildren(p1);
                            object Express2 = this.VisitChildren(p2);

                            double db1 = object_2_double(Express1);
                            double db2 = object_2_double(Express2);

                            return db1 * db2;
                        }
                    }
                    //Console.Write(Express1);
                    break;
            }
            object pObj=base.VisitChildren(node);
            return pObj;
        }

        public override object VisitErrorNode(IErrorNode node) {
            return base.VisitErrorNode(node);
        }

        public override object VisitTerminal(ITerminalNode node) {
            return base.VisitTerminal(node);
        }

        protected override object AggregateResult(object aggregate, object nextResult) {
            return base.AggregateResult(aggregate, nextResult);
        }

        protected override bool ShouldVisitNextChild(IRuleNode node, object currentResult) {
            return base.ShouldVisitNextChild(node, currentResult);
        }
    }
}
