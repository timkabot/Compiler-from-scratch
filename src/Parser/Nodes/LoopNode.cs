using System.Collections.Generic;
using System.IO;

namespace Dlanguage
{
    using System;
    public enum LoopType
    {
        While,
        For
    }
    public class LoopNode : BaseNode
    {
        public LoopType type { get; set; }
        private List<BaseNode> children;
        public string id; // may be null if it is while loop
        public Dictionary<string, BaseNode> fields = new Dictionary<string, BaseNode>();
        public LoopNode(LoopType t) : base(NodeType.LoopNode)
        {
            this.type = t;
            children = new List<BaseNode>();
        }
        public void setWhilte(ExprNode expr, BodyNode body)
        {
            fields.Add("expr", expr);
            fields.Add("body", body);
            //children.Add(body);
            children.Add(expr);
        }
        public void setFor(string id, IterNode iterator, BodyNode body)
        {
            this.id = id;
            fields.Add("iter", iterator);
            fields.Add("body", body);
            children.Add(iterator);
            //children.Add(body);
        }
        override public void show(int i, StreamWriter sw)
        {
            if (type == LoopType.For)
            {
                sw.WriteLine(indent(i) + "For Loop");
                sw.WriteLine(indent(i + 2) + "Identifier: " + id);
                fields.GetValueOrDefault("iter").show(i + 2, sw);
                fields.GetValueOrDefault("body").show(i + 2, sw);
            }
            else
            {
                sw.WriteLine(indent(i) + "While Loop");
                fields.GetValueOrDefault("expr").show(i + 2, sw);
                fields.GetValueOrDefault("body").show(i + 2, sw);
            }

        }
        public void execute()
        {
            if (type == LoopType.For)
            {
                var iter = (IterNode)fields.GetValueOrDefault("iter");
                // iter.l.execute();
                // iter.r.execute();
            }
        }

        public override IEnumerable<BaseNode> getChildren()
        {
            return children;
        }
        public bool checkScopes(Scope scope)
        {
            if (type == LoopType.For)
                return ((IterNode)fields.GetValueOrDefault("iter")).checkScopes(scope)
                    || ((BodyNode)fields.GetValueOrDefault("body")).checkScopes(scope);
            else
                return ((ExprNode)fields.GetValueOrDefault("expr")).checkScopes(scope)
                    || ((BodyNode)fields.GetValueOrDefault("body")).checkScopes(scope);

        }
    }
}