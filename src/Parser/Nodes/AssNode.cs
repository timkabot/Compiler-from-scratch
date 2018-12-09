using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Dlanguage
{
    using System;
    using System.IO;
    public class AssNode : BaseNode
    {
        public PrimaryNode prim { get; set; }
        public ExprNode expr { get; set; }
        private List<ExprNode> children;
        public AssNode(PrimaryNode p, ExprNode e) : base(NodeType.AssNode)
        {
            this.prim = p;
            this.expr = e;
            children = new List<ExprNode>();
            children.Add(expr);

        }
        override public void show(int i, StreamWriter sw)
        {
            sw.WriteLine(indent(i) + "Assigment Node");
            prim.show(i + 2, sw);
            expr.show(i + 2, sw);
        }
        override public IEnumerable<BaseNode> getChildren()
        {
            return children;
        }
        public bool checkScopes(Scope scope)
        {
            return (prim.checkScopes(scope) || expr.checkScopes(scope));
        }
    }
}