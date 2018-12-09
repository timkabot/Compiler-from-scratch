using System.Collections.Generic;

namespace Dlanguage
{
    using System.IO;
    using System;
    public class RelNode : BaseNode
    {
        private FactorNode l_factor;
        private FactorNode r_factor;
        public string Op;
        private List<BaseNode> children;
        public RelNode(FactorNode lf, string op, FactorNode rf) : base(NodeType.RelNode)
        {
            l_factor = lf;
            r_factor = rf;
            Op = op;
            children = new List<BaseNode>();
            children.Add(l_factor);
            children.Add(r_factor);
        }
        public RelNode(FactorNode factor) : this(factor, null, null) { }

        public FactorNode getLFactor()
        {
            return l_factor;
        }

        public string getOp()
        {
            return Op;
        }
        public FactorNode getRFactor()
        {
            return r_factor;
        }
        override public void show(int i, StreamWriter sw)
        {
            sw.WriteLine(indent(i) + "Relation Node");
            if (r_factor == null)
                l_factor.show(i + 2, sw);
            else
            {
                sw.WriteLine(indent(i + 2) + Op);
                l_factor.show(i + 2, sw);
                r_factor.show(i + 2, sw);
            }
        }
        override public IEnumerable<BaseNode> getChildren()
        {
            return children;
        }

        public bool checkScopes(Scope prev)
        {
            return l_factor.checkScopes(prev) || (r_factor != null && r_factor.checkScopes(prev));
        }
    }
}