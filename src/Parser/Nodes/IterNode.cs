namespace Dlanguage
{
    using System.Collections.Generic;
    using System;
    using System.IO;
    public class IterNode : BaseNode
    {
        public ExprNode l { get; set; }
        public ExprNode r { get; set; }
        private List<BaseNode> children;
        public IterNode(ExprNode l, ExprNode r) : base(NodeType.IterNode)
        {
            this.l = l;
            this.r = r;
            children = new List<BaseNode>();
            children.Add(l);
            children.Add(r);
        }
        override public void show(int i, StreamWriter sw)
        {
            sw.WriteLine(indent(i) + "Iteraror Node");
            l.show(i + 2, sw);
            r.show(i + 2, sw);
        }



        override public IEnumerable<BaseNode> getChildren()
        {
            return children;
        }
        public bool checkScopes(Scope scope)
        {
            return l.checkScopes(scope) || r.checkScopes(scope);

        }
    }
}