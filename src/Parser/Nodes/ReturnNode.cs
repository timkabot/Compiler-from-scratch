using System.Collections.Generic;

namespace Dlanguage
{
    using System;
    using System.IO;
    public class ReturnNode : BaseNode
    {
        private ExprNode expr;
        public ReturnNode() : this(null) { }

        private List<BaseNode> children;
        // "return expression" case
        public ReturnNode(ExprNode exprNode) : base(NodeType.ReturnNode)
        {
            this.expr = exprNode;
            children = new List<BaseNode>();
            children.Add(exprNode);
        }


        override public IEnumerable<BaseNode> getChildren()
        {
            return children;
        }

        override public void show(int i, StreamWriter sw)
        {
            sw.WriteLine(indent(i) + "Return Node");
            if (expr == null)
                sw.WriteLine(indent(i + 2) + "Empty");
            else
                expr.show(i + 2, sw);
        }
        public bool checkScopes(Scope scope)
        {
            if (expr == null)
                return false;
            return expr.checkScopes(scope);
        }
    }
}