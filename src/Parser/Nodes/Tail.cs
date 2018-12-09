namespace Dlanguage
{
    using System.Collections.Generic;
    using System;
    using System.IO;
    public enum TailType
    {
        IntLiteral,
        Id,
        ArrIndex,
        FuncArgs
    }
    public class Tail : BaseNode
    {
        /*  0 - intLiteral
            1 - id
            2 - Expr
            3 - func call
        */
        private TailType type;
        override public NodeType get()
        {
            switch (type)
            {
                case TailType.ArrIndex:
                    return NodeType.ArrayLiteralNode;
                case TailType.FuncArgs:
                    return NodeType.FunctionalLiteralNode;
                default:
                    return NodeType.TupleLiteralNode;
            }
        }
        private int literal;
        private string id;
        private List<ExprNode> exprs = new List<ExprNode>();
        public Tail(TailType t) : base(NodeType.TailNode)
        {
            this.type = t;
        }

        override public IEnumerable<BaseNode> getChildren()
        {
            return exprs;
        }
        public void setLit(int ind)
        {
            this.literal = ind;
        }

        public void setId(string id)
        {
            this.id = id;
        }

        public void setArgs(List<ExprNode> args)
        {
            this.exprs = args;
        }
        public void setIndex(ExprNode index)
        {
            this.exprs.Add(index);
        }
        override public void show(int i, StreamWriter sw)
        {
            sw.WriteLine(indent(i) + "Tail Node");
            switch (type)
            {
                case TailType.IntLiteral:
                    sw.WriteLine(indent(i + 4) + type);
                    sw.WriteLine(indent(i + 4) + literal);
                    break;
                case TailType.Id:
                    sw.WriteLine(indent(i + 4) + type);
                    sw.WriteLine(indent(i + 4) + id);
                    break;
                case TailType.ArrIndex:
                    sw.WriteLine(indent(i + 4) + type);
                    exprs[0].show(i + 4, sw);
                    break;
                default:
                    sw.WriteLine(indent(i + 4) + type);
                    foreach (var item in exprs)
                        item.show(i + 4, sw);
                    break;
            }
        }
        public bool checkScopes(Scope scope)
        {
            if (type == TailType.ArrIndex)
                return exprs[0].checkScopes(scope);
            else if (type == TailType.FuncArgs)
                foreach (var item in exprs)
                    if (item.checkScopes(scope))
                        return true;
            return false;
        }
    }
}