namespace Dlanguage
{
    using System.Collections.Generic;
    using System;
    using System.IO;
    public class TermNode : BaseNode
    {
        private UnaryNode leftUnaryNode; //left unary node
        private List<ComplexUnary> tail;
        private List<BaseNode> children;
        public TermNode(UnaryNode unary, List<ComplexUnary> tail) : base(NodeType.TermNode)
        {
            children = new List<BaseNode>();
            leftUnaryNode = unary;
            this.tail = tail;
            children.Add(unary);
            if (tail != null)
            {
                for (int i = 0; i < tail.Count; i++)
                {
                    UnaryNode temp = tail[i].getUnary();
                    children.Add(tail[i]);
                }
            }
        }

        public List<ComplexUnary> getTail()
        {
            return tail;
        }

        public TermNode(UnaryNode unary) : this(unary, null) { }
        override public void show(int i, StreamWriter sw)
        {
            sw.WriteLine(indent(i) + "Term Node");
            leftUnaryNode.show(i + 4, sw);
            if (tail != null)
                foreach (var item in tail)
                    item.show(i + 4, sw);
        }


        override public IEnumerable<BaseNode> getChildren() { return children; }

        public bool checkScopes(Scope prev)
        {
            return leftUnaryNode.checkScopes(prev) || (tail != null && checkScopesAll(tail, prev));

        }
        public bool checkScopesAll(List<ComplexUnary> tail, Scope prev)
        {
            for (int ind = 0; ind < tail.Count; ++ind)
                if (tail[ind].checkScopes(prev))
                    return true;
            return false;
        }

    }
    public class ComplexTerm : BaseNode
    {
        private TermNode term;
        FactorOp op;
        public ComplexTerm(TermNode term, string op) : base(NodeType.ComplexTermNode)
        {
            this.term = term;
            this.op = Convert.toFactor(op);
        }

        public FactorOp getop()
        {
            return op;
        }
        override public void show(int i, StreamWriter sw)
        {
            sw.WriteLine(indent(i) + op);
            term.show(i, sw);
        }


        public TermNode getTerm()
        {
            return term;
        }


        override public IEnumerable<BaseNode> getChildren()
        {
            List<BaseNode> children = new List<BaseNode>();
            children.Add(term);
            return children;
        }

        public bool checkScopes(Scope prev)

        {
            return term.checkScopes(prev);
        }
    }
}