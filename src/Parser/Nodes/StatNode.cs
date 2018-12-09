using System.Collections.Generic;
using System;
using System.IO;
namespace Dlanguage
{
    public enum StatType
    {
        AssNode,
        IfNode,
        LoopNode,
        ReturnNode,
        PrintNode
    }
    public class StatNode : BaseNode
    {
        private Dictionary<string, BaseNode> fields = new Dictionary<string, BaseNode>();
        private List<BaseNode> children;
        private StatType type;
        public StatNode(StatType type) : base(NodeType.TypeNode)
        {
            this.type = type;
            children = new List<BaseNode>();
        }

        override public IEnumerable<BaseNode> getChildren()
        {
            return children;
        }
        public void setAssignment(AssNode assignment)
        {

            fields.Add("value", assignment);
            children.Add(assignment);
        }

        public void setIf(IfNode _if)
        {
            fields.Add("value", _if);
            children.Add(_if);
        }

        public void setLoop(LoopNode _loop)
        {
            fields.Add("value", _loop);
            children.Add(_loop);
        }

        public void setReturn(ReturnNode _return)
        {
            fields.Add("value", _return);
            children.Add(_return);
        }

        public void setPrint(PrintNode _print)
        {
            fields.Add("value", _print);
            children.Add(_print);

        }
        override public void show(int i, StreamWriter sw)
        {
            sw.WriteLine(indent(i) + "Statement Node");
            sw.WriteLine(indent(i + 4) + type);
            fields.GetValueOrDefault("value").show(i + 4, sw);
        }
        public bool checkScopes(Scope prev)
        {
            switch (type)
            {
                case StatType.PrintNode:
                    return ((PrintNode)fields.GetValueOrDefault("value")).checkScopes(prev);
                case StatType.ReturnNode:
                    return ((ReturnNode)fields.GetValueOrDefault("value")).checkScopes(prev);
                case StatType.AssNode:
                    return ((AssNode)fields.GetValueOrDefault("value")).checkScopes(prev);
                case StatType.IfNode:
                    return ((IfNode)fields.GetValueOrDefault("value")).checkScopes(prev);
                default:
                    return ((LoopNode)fields.GetValueOrDefault("value")).checkScopes(prev);
            }
        }

    }
}