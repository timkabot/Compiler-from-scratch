using System.Collections.Generic;
using System.Linq;

using System.IO;
namespace Dlanguage
{
    using System;

    public class PrintNode : BaseNode
    {
        private List<ExprNode> exprs;

        public PrintNode(List<ExprNode> e) : base(NodeType.PrintNode)
        {
            this.exprs = e;
            children = new List<BaseNode>();
            for (int i = 0; i < e.Count(); i++)
            {
                children.Add(e[i]);
            }
        }

        private List<BaseNode> children;
        override public void show(int i, StreamWriter sw)
        {
            sw.WriteLine(indent(i) + "Print Node");
            if (exprs == null || exprs.Count == 0)
            {
                sw.WriteLine(indent(i + 2) + "Empty");
                return;
            }
            foreach (var item in exprs)
                item.show(i + 2, sw);

        }


        override public IEnumerable<BaseNode> getChildren()
        {
            return children;
        }

        public bool checkScopes(Scope scope)
        {
            if (exprs == null || exprs.Count == 0)
                return false;
            else
                foreach (var item in exprs)
                    if (item.checkScopes(scope))
                        return true;
            return false;
        }


    }

}