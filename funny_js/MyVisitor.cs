using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var a=tree.GetText();
            return base.Visit(tree);
        }

        public override object VisitChildren(IRuleNode node) {
            var type = node.GetType().Name;
            var a = node.GetText();
            Console.WriteLine(type + "==" + a);
            return base.VisitChildren(node);
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
