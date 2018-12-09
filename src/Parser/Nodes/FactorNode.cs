using System.Net.Http.Headers;

namespace Dlanguage
{
    using System.Collections.Generic;
    using System;
    using System.IO;
    public class FactorNode : BaseNode
    {
        private TermNode lt;
        private List<ComplexTerm> tail;
        private List<BaseNode> children;
        public FactorNode(TermNode lt, List<ComplexTerm> tail) : base(NodeType.FactorNode)
        {
            this.lt = lt;
            this.tail = tail;
            children = new List<BaseNode>();
            children.Add(lt);
            if (tail != null)
            {
                for (int i = 0; i < tail.Count; i++)
                    children.Add(tail[i]);
            }
        }

        public List<ComplexTerm> getTail()
        {
            return tail;
        }
        public FactorNode(TermNode term) : this(term, null) { }
        override public void show(int i, StreamWriter sw)
        {
            sw.WriteLine(indent(i) + "Factor Node");
            lt.show(i + 2, sw);
            if (tail != null)
                foreach (var item in tail)
                    item.show(i + 2, sw);
        }
        public bool checkScopes(Scope prev)
        {
            return lt.checkScopes(prev) || (tail != null && checkScopesAll(tail, prev));
        }
        public bool checkScopesAll(List<ComplexTerm> tail, Scope prev)
        {
            for (int ind = 0; ind < tail.Count; ++ind)
                if (tail[ind].checkScopes(prev))
                    return true;
            return false;

        }

        override public IEnumerable<BaseNode> getChildren()
        {
            return children;
        }
    }
}