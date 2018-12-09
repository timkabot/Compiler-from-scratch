using System.Collections.Generic;

namespace Dlanguage
{
    using System;
    using System.IO;
    public class ExprNode : BaseNode
    {
        public ResultType rtype;
        public object r;
        private RelNode l_relation;
        private RelNode r_relation;
        private string op;
        private List<BaseNode> children;
        public ExprNode(RelNode lr, string op, RelNode rr) : base(NodeType.ExprNode)
        {
            l_relation = lr;
            r_relation = rr;
            this.op = op;
            children = new List<BaseNode>();
            children.Add(l_relation);
            children.Add(r_relation);
        }
        public ExprNode(RelNode lr) : this(lr, null, null) { }

        public string getOp()
        {
            return op;
        }
        public RelNode getLeftRelation()
        {
            return l_relation;
        }

        public RelNode getRightRelation()
        {
            return r_relation;
        }
        override public void show(int i, StreamWriter sw)
        {
            sw.WriteLine(indent(i) + "Expression Node");
            if (r_relation == null)
                l_relation.show(i + 2, sw);
            else
            {
                sw.WriteLine(indent(i + 2) + "Operation: {0}", Convert.toLogic(op));
                l_relation.show(i + 2, sw);
                r_relation.show(i + 2, sw);
            }
        }
        override public IEnumerable<BaseNode> getChildren()
        {
            return children;
        }

        // id is excluded id that is not considered when checking scopes
        public bool checkScopes(Scope prev)
        {
            return l_relation.checkScopes(prev) || (r_relation != null && r_relation.checkScopes(prev));
        }
        /*
        override public void execute()
        {
            if (r_relation == null)
            {
                l_relation.execute();
                rtype = l_relation.rtype;
                r = l_relation.r;
            }
            else
            {
                l_relation.execute();
                r_relation.execute();
                rtype = toType(l_relation.rtype, op, r_relation.rtype);
                r =
            }
        } */
    }

}