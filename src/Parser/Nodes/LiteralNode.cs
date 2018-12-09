using System;
using System.Collections.Generic;

using System.IO;
using System.Linq;

namespace Dlanguage
{

    public enum LiteralType
    {
        Int,
        Real,
        Bool,
        String,
        Arr,
        Tup,
        Func
    }
    public class LiteralNode : BaseNode
    {
        public LiteralType type { get; set; }
        private Type returnType = Type.Nothing;
        private List<BaseNode> children;
        public LiteralNode(LiteralType t, object val) : base(NodeType.LiteralNode)
        {
            this.type = t;
            this.Value = val;
            children = new List<BaseNode>();
            if (t == LiteralType.String)
            {
                string v = (string)val;
                Value = v.Substring(5);
            }
            if (t == LiteralType.Func)
            {
                children.Add((FuncLiteral)Value);

            }
            initReturnType();
        }
        public object Value { get; set; }

        public void initReturnType()
        {
            switch (type)
            {
                case LiteralType.Bool:
                    returnType = Type.Bool;
                    break;
                case LiteralType.Int:
                    returnType = Type.Int;
                    break;
                case LiteralType.Real:
                    returnType = Type.Real;
                    break;
                case LiteralType.String:
                    returnType = Type.String;
                    break;
                case LiteralType.Arr:
                    returnType = Type.Arr;
                    break;
                case LiteralType.Tup:
                    returnType = Type.Tup;
                    break;
                default:
                    returnType = Type.Func;
                    break;

            }
        }
        override public void show(int i, StreamWriter sw)
        {
            sw.WriteLine(indent(i) + "Literal Node");
            switch (type)
            {
                case LiteralType.Bool:
                    sw.WriteLine(indent(i + 2) + (bool)Value);
                    break;
                case LiteralType.Int:
                    sw.WriteLine(indent(i + 2) + (int)Value);
                    break;
                case LiteralType.Real:
                    sw.WriteLine(indent(i + 2) + (double)Value);
                    break;
                case LiteralType.String:
                    sw.WriteLine(indent(i + 2) + (string)Value);
                    break;
                case LiteralType.Arr:
                    if (Value != null)
                        ((ArrLiteral)Value).show(i + 2, sw);
                    else
                        sw.WriteLine(indent(i + 2) + "[]");
                    break;
                case LiteralType.Tup:
                    if (Value != null)
                    {
                        var tupes = (List<Tuple<string, ExprNode>>)Value;
                        foreach (var (s, item) in tupes)
                        {
                            if (s != null)
                                sw.WriteLine(indent(i + 2) + s);
                            item.show(i + 2, sw);
                        }
                    }
                    else
                        sw.WriteLine(indent(i + 2) + "{}");
                    break;
                default:
                    ((FuncLiteral)Value).show(i + 2, sw);
                    break;

            }

        }
        public bool checkScopes(Scope prev)
        {
            if (type == LiteralType.Func)
            {
                return ((FuncLiteral)Value).checkScopes(prev);
            }
            else if (type == LiteralType.Tup)
            {
                var tupes = (List<Tuple<string, ExprNode>>)Value;
                foreach (var (s, item) in tupes)
                    if (item.checkScopes(prev))
                        return true;
                return false;
            }
            //else if (type == LiteralType.Arr)
            //    return ((ArrLiteral)Value).checkScopes(prev);
            return false;
        }

        public override IEnumerable<BaseNode> getChildren()
        {
            if (type == LiteralType.Tup)
            {
                var tupes = (List<Tuple<string, ExprNode>>)Value;
                for (int i = 0; i < tupes.Count; i++)
                {
                    children.Add(tupes[i].Item2);
                }
            }
            return children;
        }

        public Type getReturnType()
        {
            return returnType;
        }
    }
}