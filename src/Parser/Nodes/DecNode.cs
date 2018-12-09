using System.Collections.Generic;

namespace Dlanguage
{
    using System;
    using System.IO;
    public class DecNode : BaseNode
    {
        private ResultType rtype;
        private object r;
        public string id { get; set; }
        public ExprNode expr { get; set; } // expression  , OPTIONAL
        private List<BaseNode> children;

        public string ID
        {
            get
            {
                return id;
            }
        }
        public DecNode(string id, ExprNode expr) : base(NodeType.DecNode)
        {
            this.id = id;
            this.expr = expr;
            children = new List<BaseNode>();
            children.Add(expr);
        }

        public DecNode(string id) : this(id, null) { }
        override public void show(int i, StreamWriter sw)
        {
            sw.WriteLine(indent(i) + "Declaration Node");
            sw.WriteLine(indent(i + 2) + id);
            if (expr != null)
                expr.show(i + 2, sw);
        }
        override public IEnumerable<BaseNode> getChildren()
        {
            return children;
        }
        public bool checkScopes(Scope prev)
        {

            return expr != null && expr.checkScopes(prev);
        }

    }
}