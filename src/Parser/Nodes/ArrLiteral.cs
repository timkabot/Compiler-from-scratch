using System.Collections.Generic;
using System;
using System.IO;
namespace Dlanguage
{
    public class ArrLiteral : BaseNode
    {
        private List<ExprNode> exprNodes;
        public ArrLiteral(List<ExprNode> exprNodes) : base(NodeType.ArrayLiteralNode)
        {
            this.exprNodes = exprNodes;
        }
        override public void show(int i, StreamWriter sw)
        {
            sw.WriteLine(indent(i) + "Array Literal");
            if (exprNodes.Count == 0)
                sw.WriteLine(indent(i + 2) + "Empty");
            else
            {
                foreach (var item in exprNodes)
                    item.show(i + 2, sw);
            }
        }


        override public IEnumerable<BaseNode> getChildren()
        {
            return exprNodes;
        }
        public bool checkScopes(Scope scope)
        {
            for (int ind = 0; ind < exprNodes.Count; ind++)
                if (exprNodes[ind].checkScopes(scope))
                    return true;
            return false;

        }
    }
}