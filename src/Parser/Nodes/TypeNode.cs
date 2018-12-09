using System.Collections.Generic;

namespace Dlanguage
{
    using System;
    using System.IO;
    public enum Type
    {
        Int,
        Real,
        Bool,
        String,
        Empty,
        Arr,
        Tup,
        Func,
        Expr,
        Nothing
    }
    public class TypeNode : BaseNode
    {
        private bool hasExpr;
        private ExprNode l, r;
        private Type type;
        private List<BaseNode> children;
        public TypeNode(Type type) : base(NodeType.TypeNode)
        {
            this.type = type;
            children = new List<BaseNode>();
        }
        public void setExpr(ExprNode l, ExprNode r)
        {
            this.l = l;
            this.r = r;
            children.Add(l);
            children.Add(r);
            this.hasExpr = true;
        }
        override public void show(int i, StreamWriter sw)
        {
            sw.WriteLine(indent(i) + "Type Node");
            sw.WriteLine(indent(i) + type);
        }

        public override IEnumerable<BaseNode> getChildren()
        {
            return children;
        }
    }
}