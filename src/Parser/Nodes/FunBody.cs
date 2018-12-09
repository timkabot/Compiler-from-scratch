using System.Linq.Expressions;
using System;
using System.IO;
using System.Collections.Generic;

namespace Dlanguage
{
    public enum FuncType
    {
        Complex,
        Expr
    }
    public class FunBody : BaseNode
    {
        private FuncType type;
        private object body;
        private List<BaseNode> children;
        public FunBody(FuncType t) : base(NodeType.FunBodyNode)
        {
            this.type = t;
            children = new List<BaseNode>();
        }
        public void setBody(BodyNode b)
        {
            this.body = b;
            children.Add(b);
        }
        public void setExpr(ExprNode expr)
        {
            this.body = expr;
            children.Add(expr);
        }

        override public void show(int i, StreamWriter sw)
        {
            if (type == FuncType.Expr)
                ((ExprNode)body).show(i + 2, sw);
            else
                ((BodyNode)body).show(i + 2, sw);
        }
        public bool checkScopes(Scope scope)
        {
            if (type == FuncType.Expr)
                return ((ExprNode)body).checkScopes(scope);
            else
            {
                return ((BodyNode)body).checkScopes(scope);
            }
        }

        override public IEnumerable<BaseNode> getChildren()
        {
            return children;
        }
    }
}