using System.Collections.Generic;

namespace Dlanguage
{
    using System.Collections.Specialized;
    using System;
    using System.IO;
    public class rootNode : BaseNode
    {
        public rootNode() : base(NodeType.rootNode)
        {
            children = new List<BaseNode>();
        }
        private BodyNode program;
        private List<BaseNode> children;
        public void setProgram(BodyNode b)
        {
            this.program = b;
            children.Add(program);
        }
        override public void show(int indent, StreamWriter sw)
        {

            sw.WriteLine("Root Node");
            program.show(indent + 2, sw);
        }

        override public IEnumerable<BaseNode> getChildren()
        {
            return children;
        }
        public void Draw(StreamWriter sw)
        {
            show(0, sw);
        }
        public void checkScopes()
        {
            if (program != null && program.checkScopes(null))
                throw new SemanticError("Scoupe Error, access to variable that is not visible in current scoupe");
        }
    }
}