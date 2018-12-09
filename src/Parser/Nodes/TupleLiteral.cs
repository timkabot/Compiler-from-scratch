using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace Dlanguage
{
    using System;
    using System.IO;
    public class TupleLiteral : BaseNode
    {
        private List<KeyValuePair<string, ExprNode>> units;
        private List<BaseNode> children = new List<BaseNode>();
        public TupleLiteral(List<KeyValuePair<string, ExprNode>> units) : base(NodeType.TupleLiteralNode)
        {
            this.units = units;
            children = new List<BaseNode>();
            for (int i = 0; i < units.Count(); i++)
            {
                children.Add(units[i].Value);
            }
        }
        override public IEnumerable<BaseNode> getChildren()
        {
            return children;
        }
        override public void show(int i, StreamWriter sw)
        {
            sw.WriteLine(indent(i) + "Tuple Literal");
            if (units == null || units.Count == 0)
                sw.WriteLine(indent(i + 4) + "Empty");
            else
            {
                foreach (var (s, e) in units)
                    if (s == null)
                        e.show(i + 4, sw);
                    else
                    {
                        sw.WriteLine(indent(i + 4), s);
                        e.show(i + 8, sw);
                    }
            }
        }
        public bool checkScopes(Scope s)
        {
            for (int ind = 0; ind < units.Count; ind++)
                if (units[ind].Value.checkScopes(s))
                    return true;
            return false;
        }
    }
}