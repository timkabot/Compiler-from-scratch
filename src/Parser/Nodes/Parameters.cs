using System.Collections.Generic;
using System;
using System.IO;
namespace Dlanguage
{
    public class Parameters : BaseNode
    {
        private List<string> identifiers;
        public List<string> getIds => identifiers;

        public Parameters(List<string> identifiers) : base(NodeType.ParametersNode)
        {
            this.identifiers = identifiers;
        }
        override public void show(int i, StreamWriter sw)
        {
            sw.Write(indent(i) + "Parameters: ");
            foreach (var item in identifiers)
                sw.Write(item + " ");
            sw.WriteLine();

        }

        public override IEnumerable<BaseNode> getChildren()
        {
            throw new NotImplementedException();
        }
    }
}