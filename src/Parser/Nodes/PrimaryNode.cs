using System.Linq;

namespace Dlanguage
{
    using System.Collections.Generic;
    using System;
    using System.IO;
    public enum PrimaryType
    {
        Id,
        readInt,
        readString,
        readReal
    }

    public class PrimaryNode : BaseNode
    {
        public PrimaryType type { get; set; }
        public string id = null;
        public int index = -1;
        private List<Tail> tail = null;
        private List<BaseNode> children;
        public List<BaseNode> indexChildren { get; set; }
        public void setId(string id, List<Tail> tail)
        {
            this.id = id;
            this.tail = tail;
            if (tail != null)
            {
                for (int i = 0; i < tail.Count; i++)
                {
                    children.Add(tail[i]);
                    indexChildren.Add(tail[i]);
                }
            }
        }

        public PrimaryNode(PrimaryType t) : base(NodeType.PrimaryNode)
        {
            this.type = t;
            indexChildren = new List<BaseNode>();
            children = new List<BaseNode>();
        }
        override public void show(int i, StreamWriter sw)
        {
            sw.WriteLine(indent(i) + "Primary Node");
            switch (type)
            {
                case PrimaryType.readInt:
                case PrimaryType.readReal:
                case PrimaryType.readString:
                    sw.WriteLine(indent(i + 2) + type);
                    break;
                default:
                    sw.WriteLine(indent(i + 2) + id);
                    if (tail != null)
                        foreach (var item in tail)
                            item.show(i + 2, sw);
                    break;
            }
        }

        override public IEnumerable<BaseNode> getChildren()
        {
            return children;
        }

        public bool checkScopes(Scope prev)

        {
            if (this.id == null)
            {
                return false;
            }
            //Console.WriteLine("-------- {0} -------", prev.get(this.id) == null);

            if (tail != null)
            {
                foreach (var item in tail)
                    if (item.checkScopes(prev))
                        return true;
            }
            return !prev.has(this.id);

        }
        public bool thereis(BaseNode node, List<Tail> tail)
        {
            if (node == null)
                return false;
            for (int ind = 0; ind < tail.Count; ind++)
                if (node == null)
                    return false;
                else if (node.get() != tail[ind].get())
                    return false;

            return true;
        }
    }
}