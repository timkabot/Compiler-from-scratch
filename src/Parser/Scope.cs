namespace Dlanguage
{
    using System.Collections.Generic;
    using System;
    public class Scope
    {
        private Dictionary<string, BaseNode> variables = new Dictionary<string, BaseNode>();
        public Scope prev
        {
            get;
            set;
        }
        public bool has(string id)
        {
            return variables.ContainsKey(id) || (prev != null && prev.has(id));
        }
        public List<string> getVars()
        {
            var ret = new List<string>(variables.Keys);
            if (prev != null)
                ret.AddRange(prev.getVars());
            return ret;
        }
        public BaseNode get(string id)
        {
            if (variables.ContainsKey(id))

                return variables.GetValueOrDefault(id);
            else if (prev != null)
                prev.get(id);
            return null;
        }
        public Scope() { }
        public void addVar(string id, BaseNode node)
        {
            if (variables.ContainsKey(id))

                variables.Remove(id);
            variables.Add(id, node);

        }
    }
}