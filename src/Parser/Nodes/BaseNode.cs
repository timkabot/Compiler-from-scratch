using System.Collections.Generic;

namespace Dlanguage
{
    using System.IO;
    public abstract class BaseNode
    {
        private string resultType;
        private NodeType type;

        public BaseNode(NodeType nt, string resultType = "")
        {
            this.type = nt;
            this.resultType = resultType;
        }
        virtual public NodeType get()
        {
            return this.type;
        }
        public BaseNode(NodeType nt) => this.type = nt;
        abstract public void show(int i, StreamWriter sw);
        public static string indent(int i)
        {
            return new string(' ', i);
        }

        abstract public IEnumerable<BaseNode> getChildren();

    }
}