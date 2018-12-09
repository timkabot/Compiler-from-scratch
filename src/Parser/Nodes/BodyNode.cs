using System.Collections.Generic;
using System;
using System.IO;
namespace Dlanguage
{
    public enum BodyType
    {
        Decl,
        Stat,
        Expr
    }
    public class BodyNode : BaseNode
    {
        private Scope scope;
        private List<(BodyType, BaseNode)> body = new List<(BodyType, BaseNode)>();
        private List<BaseNode> children;

        public BodyNode() : base(NodeType.BodyNode)
        {
            children = new List<BaseNode>();
        }
        public void add(BodyType type, BaseNode o)
        {
            body.Add((type, o));
            children.Add(o);
        }
        public void addAll(BodyType t, List<StatNode> l)
        {
            foreach (var n in l)
                add(t, n);
        }
        override public void show(int i, StreamWriter sw)
        {
            sw.Write(indent(i) + "Body Node, Scope: ");
            if (scope != null)
                foreach (var v in scope.getVars())
                    sw.Write(v + " ");
            sw.WriteLine();
            foreach (var item in body)
                item.Item2.show(i + 2, sw);
        }

        override public IEnumerable<BaseNode> getChildren()
        {
            return children;
        }
        public bool checkScopes(Scope prev)
        {
            var scope = new Scope();
            scope.prev = prev;
            this.scope = scope;
            foreach (var (type, node) in body)
                if (type == BodyType.Decl)
                    scope.addVar(((DecNode)node).ID, node);
            int ret = 0, index = 0;
            foreach (var (type, node) in body)
            {
                if (type == BodyType.Decl)
                {
                    if (((DecNode)node).checkScopes(scope))
                    {
                        Console.WriteLine("THis gives true {0}", index); ret = 1;
                    }
                }
                else if (type == BodyType.Expr)
                {
                    if (((ExprNode)node).checkScopes(scope))
                    {
                        ret = 1;
                    }
                }
                else if (((StatNode)node).checkScopes(scope))
                    ret = 1;
                index++;

            }
            // Console.WriteLine("Inside Body nodes {0}", ret == 1);
            return ret == 1;
        }
    }

}