using System.Collections.Generic;

namespace Dlanguage
{
    using System;
    using System.IO;
    public class IfNode : BaseNode
    {

        public ExprNode expr { get; set; }
        public BodyNode body { get; set; }
        public BodyNode else_body { get; set; }
        private List<BaseNode> children;
        public IfNode(ExprNode e, BodyNode b, BodyNode el) : base(NodeType.IfNode)
        {
            this.expr = e;
            this.body = b;
            this.else_body = el;
            children = new List<BaseNode>();
            //children.Add(expr);
            //children.Add(body);
            if (else_body != null) children.Add(else_body);
        }
        public IfNode(ExprNode e, BodyNode b) : this(e, b, null) { }
        override public void show(int i, StreamWriter sw)
        {
            sw.WriteLine(indent(i) + "If Node");
            expr.show(i + 2, sw);
            body.show(i + 2, sw);
            if (else_body != null)
                else_body.show(i + 2, sw);

        }


        override public IEnumerable<BaseNode> getChildren()
        {
            return children;
        }
        public bool checkScopes(Scope scope)
        {
            bool val = expr.checkScopes(scope) || body.checkScopes(scope);
            if (else_body == null)
                return val;
            else
                return val || else_body.checkScopes(scope);

        }
    }
}