using System.Collections.Generic;

namespace Dlanguage
{
    using System;
    using System.IO;
    public class FuncLiteral : BaseNode
    {
        private Scope fscope;
        private Parameters parameters;
        private FunBody funBody;
        private List<BaseNode> children;

        public FuncLiteral(Parameters parameters, FunBody funBody) : base(NodeType.FunctionalLiteralNode)
        {

            this.parameters = parameters;
            this.funBody = funBody;
            children = new List<BaseNode>();
            children.Add(funBody);
        }

        override public IEnumerable<BaseNode> getChildren()
        {
            return children;
        }
        override public void show(int i, StreamWriter sw)
        {
            if (fscope != null)
            {
                sw.Write(indent(i) + "Function Literal, SCOPE: ");
                foreach (var item in fscope.getVars())
                    sw.Write(item + ", ");
                sw.WriteLine();
            }
            else
                sw.WriteLine(indent(i) + "Function Literal:");
            if (parameters != null)
                parameters.show(i + 2, sw);
            funBody.show(i + 2, sw);
        }
        public bool checkScopes(Scope s)
        {
            if (parameters == null)
            {
                return funBody.checkScopes(s);
            }
            else
            {
                var paramScope = new Scope();
                foreach (var item in parameters.getIds)
                    paramScope.addVar(item, null);
                paramScope.prev = s;
                this.fscope = paramScope;
                return funBody.checkScopes(paramScope);
            }
        }
    }
}