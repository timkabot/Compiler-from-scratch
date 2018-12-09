using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using Convert = Dlanguage.Convert;

namespace Dlanguage
{

    public enum UnaryType
    {
        Primary,
        Literal,
        Expr
    }
    public class UnaryNode : BaseNode
    {
        private Dictionary<string, BaseNode> fields = new Dictionary<string, BaseNode>();
        private string prefix;
        private List<BaseNode> children;
        private UnaryType type;
        public void setPrimary(string pref, PrimaryNode primary, TypeNode type)
        {
            if (pref != null)
                prefix = pref;
            if (type != null)
                fields.Add("type", type);
            fields.Add("primary", primary);
            children.Add(primary);
        }
        public void setLiteral(LiteralNode lit)
        {
            fields.Add("literal", lit);
            children.Add(lit);
        }
        public void setExpr(ExprNode expr)
        {
            fields.Add("expr", expr);
            children.Add(expr);
        }
        public UnaryNode(UnaryType t) : base(NodeType.UnaryNode)
        {
            this.type = t;
            children = new List<BaseNode>();
        }
        override public void show(int i, StreamWriter sw)
        {

            switch (type)
            {
                case UnaryType.Expr:
                    fields.GetValueOrDefault("expr").show(i + 2, sw);
                    break;
                case UnaryType.Literal:
                    fields.GetValueOrDefault("literal").show(i + 2, sw);
                    break;
                default:
                    if (prefix != null)
                        sw.WriteLine(indent(i + 2) + prefix);
                    fields.GetValueOrDefault("primary").show(i + 2, sw);
                    if (fields.ContainsKey("type"))
                        fields.GetValueOrDefault("type").show(i + 2, sw);
                    break;
            }
        }



        public bool checkScopes(Scope prev)

        {
            switch (type)
            {
                case UnaryType.Expr:
                    return ((ExprNode)fields.GetValueOrDefault("expr")).checkScopes(prev);
                case UnaryType.Primary:
                    return ((PrimaryNode)fields.GetValueOrDefault("primary")).checkScopes(prev);
                default:
                    return ((LiteralNode)fields.GetValueOrDefault("literal")).checkScopes(prev);

            }
        }

        public override IEnumerable<BaseNode> getChildren()
        {
            return children;
        }

    }
    public class ComplexUnary : BaseNode
    {
        private TermOp op;
        private UnaryNode unary;
        private List<BaseNode> children;
        public ComplexUnary(string prefix, UnaryNode node) : base(NodeType.ComplexUnaryNode)
        {
            this.op = Convert.toTerm(prefix);
            this.unary = node;
            children = new List<BaseNode>();
            children.Add(unary);
        }

        public UnaryNode getUnary()
        {
            return unary;
        }

        public TermOp getOp()
        {
            return op;
        }
        override public void show(int i, StreamWriter sw)
        {
            sw.WriteLine(indent(i) + op);
            unary.show(i, sw);

        }


        public override IEnumerable<BaseNode> getChildren()
        {
            return children;
        }

        public bool checkScopes(Scope prev)

        {
            return unary.checkScopes(prev);
        }
    }
}
