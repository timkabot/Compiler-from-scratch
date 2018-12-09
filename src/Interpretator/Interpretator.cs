using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Linq.Expressions;

namespace Dlanguage.Interpretator
{
    public class Interpretator
    {
        private rootNode root;
        private Dictionary<string, int> intVariables = new Dictionary<string, int>();
        private Dictionary<string, bool> boolVariables = new Dictionary<string, bool>();
        private Dictionary<string, double> realVariables = new Dictionary<string, double>();
        private Dictionary<string, string> stringVariables = new Dictionary<string, string>();

        private Dictionary<string, Dictionary<int,int> > integerArrays = new Dictionary<string, Dictionary<int, int>>()
            , integerTuples;
        private Dictionary<string, Dictionary<int,object>> tupleVariables = new Dictionary<string, Dictionary<int, object>>();

        private Dictionary<string, Dictionary<int,string> > stringArrays, stringTuples;
       
        public static bool IsInt(object value)
        {
            return    value is int
                   || value is uint
                   || value is long
                   || value is ulong;
        }
        public static bool IsReal(object value)
        {
            return value is float
                || value is double
                || value is decimal;
        }

        public static bool IsString(object value)
        {
            return value is string;
        }
        public KeyValuePair<Type, object> find(string name)
        {
            if(intVariables.ContainsKey(name)) return new KeyValuePair<Type, object>(Type.Int,intVariables[name]);
            else if (stringVariables.ContainsKey(name))
                return new KeyValuePair<Type, object>(Type.String, stringVariables[name]);
            else
            {
                throw new Exception("bad variable обращение");
            }
            
        }
        public Interpretator(rootNode root)
        {
            this.root = root;
        }
        
        public void imitateProgram()
        {
            dfs(root);
        }
        
        public  List< KeyValuePair<Type, object> > dfs(BaseNode node)
        {
            if (node.GetType() == typeof(LiteralNode))
            {
                LiteralNode n = (LiteralNode) node;
                if (n.type == LiteralType.Func)
                {
                    return dfs((FuncLiteral)n.Value);
                }
                else if (n.type == LiteralType.Tup)
                {
                }
                else
                {
                    List<KeyValuePair<Type, object>> temp = new List<KeyValuePair<Type, object>>();
                    KeyValuePair<Type, object> pair = new KeyValuePair<Type, object>(n.getReturnType(), n.Value);
                    temp.Add(pair);
                    return temp;
                }
            }
            if (node.GetType() == typeof(IfNode))
            {
                IfNode n = (IfNode) node;

                List<KeyValuePair<Type, object>> ifUslovie = dfs(n.expr);
                bool uslovie = (bool)ifUslovie[0].Value;
                if (uslovie)
                {
                    return dfs(n.body);
                }
                else
                {
                    if(n.else_body != null)
                        return dfs(n.else_body);
                }
                
            }
            List<BaseNode> children = node.getChildren().ToList();
            
            List< KeyValuePair<Type, object> > returnTypes = new List< KeyValuePair<Type, object> >(); //our return objects
                
            for (int i = 0; i < children.Count(); i++)
            {
                if(children[i] == null) continue;
                List< KeyValuePair<Type, object> > temp = dfs(children[i]);
                for (int j = 0; j < temp.Count; j++)
                    returnTypes.Add(temp[j]);
            }

            if (node.GetType() == typeof(LoopNode))
            {
                LoopNode n = (LoopNode) node;
                LoopType loopType = n.type;
                String identifier = n.id;
                if (loopType == LoopType.For)
                {
                    int top = (int)returnTypes[1].Value;
                
                    intVariables[identifier]=(int)returnTypes[0].Value;
                    
                    while (intVariables[identifier] < top)
                    {
                        dfs(n.fields["body"]);
                        intVariables[identifier]++;  
                    }
                    while(intVariables[identifier] > top)
                    {
                        dfs(n.fields["body"]);
                        intVariables[identifier]--;  
                    }
                }

                if (loopType == LoopType.While)
                {
                    bool uslovie = (bool) dfs(n.fields["expr"])[0].Value;
                    while (uslovie)
                    {
                        dfs(n.fields["body"]);
                        uslovie = (bool) dfs(n.fields["expr"])[0].Value;
                    }
                }
                return returnTypes;
            }
            if (node.GetType() == typeof(RelNode))
            {
                RelNode n = (RelNode) node;
                if(n.getRFactor() != null)
                {
                    List< KeyValuePair<Type, object> > temp = new List<KeyValuePair<Type, object>>();
                    KeyValuePair<Type, object> left  = returnTypes[0], 
                                               right = returnTypes[1];
                    string op = n.Op;
                    bool result = false;
                    if (left.Key == Type.Int && right.Key == Type.Int)
                    { 
                      if(op == "lt_t") {result = (int)left.Value < (int)right.Value;}
                      else if (op == "gt_t")
                      {
                          result = (int)left.Value > (int)right.Value;
                      }
                      else if(op == "le_t") {result = (int)left.Value <= (int)right.Value;}
                      else if(op == "ge_t") {result = (int)left.Value >= (int)right.Value;}
                      else if(op == "eq_t") {result = (int)left.Value == (int)right.Value;}
                      else if(op == "neq_t") {result = (int)left.Value != (int)right.Value;}
                    }
                    else if (left.Key == Type.Int && right.Key == Type.Real)
                    {
                        if(op == "lt_t") {result = (int)left.Value < (double)right.Value;}
                        else if(op == "gt_t") {result = (int)left.Value > (double)right.Value;}
                        else if(op == "le_t") {result = (int)left.Value <= (double)right.Value;}
                        else if(op == "ge_t") {result = (int)left.Value >= (double)right.Value;}
                        else if(op == "eq_t") {result = (int)left.Value == (double)right.Value;}
                        else if(op == "neq_t") {result = (int)left.Value != (double)right.Value;}
                    }
                     else if (left.Key == Type.Real && right.Key == Type.Int)
                    {
                        if(op == "lt_t") {result = (double)left.Value < (int)right.Value;}
                        else if(op == "gt_t") {result = (double)left.Value > (int)right.Value;}
                        else if(op == "le_t") {result = (double)left.Value <= (int)right.Value;}
                        else if(op == "ge_t") {result = (double)left.Value >= (int)right.Value;}
                        else if(op == "eq_t") {result = (double)left.Value == (int)right.Value;}
                        else if(op == "neq_t") {result = (double)left.Value != (int)right.Value;}
                    }
                    
                     else if(left.Key == Type.Real && right.Key == Type.Real)
                    {
                        if(op == "lt_t") {result = (double)left.Value < (double)right.Value;}
                        else if(op == "gt_t") {result = (double)left.Value > (double)right.Value;}
                        else if(op == "le_t") {result = (double)left.Value <= (double)right.Value;}
                        else if(op == "ge_t") {result = (double)left.Value >= (double)right.Value;}
                        else if(op == "eq_t") {result = (double)left.Value == (double)right.Value;}
                        else if(op == "neq_t") {result = (double)left.Value != (double)right.Value;}
                    }
                    else
                    {
                        throw new Exception("Wrong types comparison");
                    }
                    temp.Add(new KeyValuePair<Type, object>(Type.Bool,result));
                    return temp;
                }
            }

            if (node.GetType() == typeof(ExprNode))
            {
                ExprNode n = (ExprNode) node;
                if (n.getRightRelation() != null)
                {
                    List< KeyValuePair<Type, object> > temp = new List<KeyValuePair<Type, object>>();
                    string op = n.getOp();
                    if (returnTypes[0].Key != Type.Bool || returnTypes[1].Key != Type.Bool)
                    {
                        throw new Exception("Wrong or, xor, and operation");
                    }
                    bool left = (bool)returnTypes[0].Value, right = (bool)returnTypes[1].Value;
                    if (op == "or_t")
                    {
                        temp.Add(new KeyValuePair<Type, object>(Type.Bool,left || right));
                    }
                    if (op == "xor_t")
                    {
                        temp.Add(new KeyValuePair<Type, object>(Type.Bool,left ^ right));
                    }
                    if (op == "and_t")
                    {
                        temp.Add(new KeyValuePair<Type, object>(Type.Bool,left && right));
                    }
                }

                return returnTypes;
            }
            
            if (node.GetType() == typeof(AssNode))
            {
                AssNode n = (AssNode) node;
                string id = n.prim.id;
                object v = returnTypes[0].Value;
                if (intVariables.ContainsKey(id))
                    intVariables[id] = (int) v;
                else if (stringVariables.ContainsKey(id))
                    stringVariables[id] = (string) v;
                else if (realVariables.ContainsKey(id))
                    realVariables[id] = (double) v;
                else if (integerArrays.ContainsKey(id))
                {   
                    List< KeyValuePair<Type, object> > index = new List< KeyValuePair<Type, object> >(); //our return objects
                    index = dfs(n.prim.indexChildren[0]);
                    integerArrays[id][(int) index[0].Value] = (int) v;                 
                }
                else if (tupleVariables.ContainsKey(id))
                {
                    throw new Exception("Tuples are immutable");
                }
                
            }

         
            if (node.GetType() == typeof(LiteralNode))
            {
                LiteralNode n = (LiteralNode) node;
                if (n.type == LiteralType.Tup)
                {
                    List< KeyValuePair<Type, object> > finalResult = new List< KeyValuePair<Type, object> >(); //our return objects
                    Dictionary<int,object> v = new Dictionary<int, object>();
                    for (int i = 0; i < returnTypes.Count; i++)
                    {
                        v.Add(i,returnTypes[i].Value);
                    }

                    finalResult.Add(new KeyValuePair<Type, object>(Type.Tup,v));
                    return finalResult;
                }
            }
            if(node.GetType() == typeof(DecNode))
            {
                DecNode d = (DecNode) node;
                switch (returnTypes[0].Key)
                {
                    case Type.Int:
                        intVariables.Add(d.id,(int)returnTypes[0].Value);
                        break;
                    case Type.Real:
                        realVariables.Add(d.id,(double)returnTypes[0].Value);
                        break;
                    case Type.String:
                        stringVariables.Add(d.id,(string) returnTypes[0].Value);
                        break;
                    case Type.Arr:
                   
                        if(returnTypes[0].Value !=null )integerArrays.Add(d.id,(Dictionary<int, int>)returnTypes[0].Value);
                        else 
                            integerArrays.Add(d.id,new Dictionary<int, int>());
                        break;
                    case Type.Tup:
                        Dictionary<int,object> v = (Dictionary<int,object>)returnTypes[0].Value;
                        tupleVariables.Add(d.id,v);
                        break;
                    case Type.Bool:
                        boolVariables.Add(d.id,(bool)returnTypes[0].Value);
                        break;
                    
                }
            }
            
            //получение по id
            if (node.GetType() == typeof(PrimaryNode))
            {
                PrimaryNode n = (PrimaryNode) node;
                List<KeyValuePair<Type,object>> temp = new List<KeyValuePair<Type, object>>();
                if (n.type == PrimaryType.Id)
                {
                    string name = n.id;
                    if (intVariables.ContainsKey(name))
                        temp.Add(new KeyValuePair<Type, object>(Type.Int, intVariables[name]));
                    else if (stringVariables.ContainsKey(name))
                        temp.Add(new KeyValuePair<Type, object>(Type.String, stringVariables[name]));
                    else if (realVariables.ContainsKey(name))
                        temp.Add(new KeyValuePair<Type, object>(Type.Real, realVariables[name]));
                    else if (integerArrays.ContainsKey(name))
                    {
                        if (returnTypes.Count == 0)
                        {
                            temp.Add(new KeyValuePair<Type, object>(Type.Arr, integerArrays[name]));
                        }
                        else
                        {
                            if (!integerArrays[name].ContainsKey((int) returnTypes[0].Value))
                            {
                                integerArrays[name].Add((int) returnTypes[0].Value, -1);
                            }
                       
                        temp.Add(new KeyValuePair<Type, object>(Type.Int,
                            integerArrays[name][(int) returnTypes[0].Value]));
                        }
                }
                    else if (boolVariables.ContainsKey(name))
                    {
                        temp.Add(new KeyValuePair<Type, object>(Type.Bool,boolVariables[name]));
                    }
                    else if (tupleVariables.ContainsKey(name))
                    {
                         Dictionary<int,object> tuple = tupleVariables[name];
                   
                            int index = (int) returnTypes[0].Value;
                            object o = tuple[index];
                            if (IsInt(o))
                            {
                                temp.Add(new KeyValuePair<Type, object>(Type.Int, tuple[index]));
                            }
                            else if (IsReal(o))
                            {
                                temp.Add(new KeyValuePair<Type, object>(Type.Real, tuple[index]));
                            }
                            else if (IsString(o))
                            {
                                temp.Add(new KeyValuePair<Type, object>(Type.String, tuple[index]));
                            }


                    }

                    return temp;
                }
                if (n.type == PrimaryType.readInt)
                {
                    temp.Add(new KeyValuePair<Type, object>(Type.Int,int.Parse(Console.ReadLine())));
                    return temp;
                }
                if (n.type == PrimaryType.readReal)
                {
                    temp.Add(new KeyValuePair<Type, object>(Type.Real,double.Parse(Console.ReadLine())));
                    return temp;
                }
                if (n.type == PrimaryType.readString)
                {
                    temp.Add(new KeyValuePair<Type, object>(Type.String,Console.ReadLine()));
                    return temp;
                }
            }
            //Принт результатов
            if (node.GetType() == typeof(PrintNode))
            {
                Console.Write("Print :");
                for (int i = 0; i < returnTypes.Count; i++)
                {
                    Console.Write(" " + returnTypes[i].Value);
                }
            }
            
            // умножение и деление
            if (node.GetType() == typeof(TermNode))
            {
                TermNode n = (TermNode) node;
                if (n.getTail() != null)
                {
                    KeyValuePair<Type, object> result = returnTypes[0];
                    List<KeyValuePair<Type, object>> finalResult = new List<KeyValuePair<Type, object>>();
                    for (int i = 1; i < returnTypes.Count; i++)
                    {
                        Type temp = returnTypes[i].Key;
                        object value = returnTypes[i].Value;
                        ComplexUnary tempUnary = (ComplexUnary) children[i];
                        TermOp sign = tempUnary.getOp();
                        if (sign == TermOp.Div)
                        {
                            if (result.Key == Type.Int && temp == Type.Int)    
                                result = new KeyValuePair<Type, object>(Type.Int,(int)result.Value/(int)value); 
                            else if (result.Key==Type.Int && temp==Type.Real)    
                                result = new KeyValuePair<Type, object>(Type.Real,(int)result.Value/(int)value);
                            else if (result.Key==Type.Real && temp==Type.Int)    
                                result = new KeyValuePair<Type, object>(Type.Real,(int)result.Value/(int)value);
                            else if(result.Key==Type.Real && temp==Type.Real)  
                                result = new KeyValuePair<Type, object>(Type.Real,(int)result.Value/(int)value); 
                            else
                            {
                                throw new Exception("Wrong division found");
                            }   
                        }
                        else if (sign == TermOp.Mult)
                        {
                            if (result.Key == Type.Int && temp == Type.Int)
                            {
                                result = new KeyValuePair<Type, object>(Type.Int,(int)result.Value*(int)value);
                            }//result stays the same
                            else if (result.Key == Type.Int && temp == Type.Real)
                                result = new KeyValuePair<Type, object>(Type.Real,(int)result.Value*(int)value);
                            else if (result.Key == Type.Real && temp == Type.Int)
                                result = new KeyValuePair<Type, object>(Type.Real,(int)result.Value*(int)value);
                            else if (result.Key == Type.Real && temp == Type.Real)
                            {
                                result = new KeyValuePair<Type, object>(Type.Real,(int)result.Value*(int)value);
                            }
                            else
                            {
                                throw new Exception("Wrong multiplication found");
                            }
                            
                        }
                     
                    }   
                    finalResult.Add(result);
                    return finalResult;
                }  
            }
            //сумма и вычитание
            if(node.GetType() ==  typeof(FactorNode))
            {
                FactorNode n = (FactorNode) node;

                if (n.getTail() != null)
                {
                    KeyValuePair<Type, object> result = returnTypes[0];
                    List<KeyValuePair<Type, object>> finalResult = new List<KeyValuePair<Type, object>>();
                    
                    for (int i = 1; i < returnTypes.Count; i++)
                    {
                        Type temp = returnTypes[i].Key;
                        object value = returnTypes[i].Value;
                        ComplexTerm tempTerm = (ComplexTerm) children[i];
                        FactorOp sign = tempTerm.getop();

                        if (sign == FactorOp.Minus)
                        {
                            if (result.Key == Type.Int && temp == Type.Int)    
                                result = new KeyValuePair<Type, object>(Type.Int,(int)result.Value-(int)value); 
                            else if (result.Key==Type.Int && temp==Type.Real)  
                                result = new KeyValuePair<Type, object>(Type.Real,(double)result.Value-(double)value);
                            else if (result.Key==Type.Real && temp==Type.Int)  
                                result = new KeyValuePair<Type, object>(Type.Real,(double)result.Value-(double)value);
                            else if(result.Key==Type.Real && temp==Type.Real)  
                                result = new KeyValuePair<Type, object>(Type.Real,(double)result.Value-(double)value);
                            else
                            {
                                throw new Exception("wrong type subtraction");
                            }
                        }
                        else if (sign == FactorOp.Plus)
                        {
                            if (result.Key == Type.Int && temp == Type.Int)    
                                result = new KeyValuePair<Type, object>(Type.Int,(int)result.Value+(int)value); 
                            else if (result.Key==Type.Int && temp==Type.Real)  
                                result = new KeyValuePair<Type, object>(Type.Real,(double)result.Value+(double)value);
                            else if (result.Key==Type.Real && temp==Type.Int)  
                                result = new KeyValuePair<Type, object>(Type.Real,(double)result.Value+(double)value);
                            else if (result.Key==Type.Real && temp==Type.Real)  
                                result = new KeyValuePair<Type, object>(Type.Real,(double)result.Value+(double)value);
                            else if (result.Key == Type.String && temp==Type.String) 
                                result = new KeyValuePair<Type, object>(Type.String,(string)result.Value+(string)value);
                            else if (result.Key == Type.Arr && temp == Type.Arr)
                            {
                                Dictionary<int, int> first = (Dictionary<int, int>)result.Value;
                                Dictionary<int, int> second = (Dictionary<int, int>)value;
                                Dictionary<int, int> q = (from e in first.Concat(second)
                                        group e by e.Key into g
                                        select new { Name = g.Key, Count = g.Sum(kvp => kvp.Value) })
                                    .ToDictionary(item => item.Name, item => item.Count);
                                result = new KeyValuePair<Type, object>(Type.Arr, q);
                            }
            
                            else
                            {
                                throw new Exception("wrong type summation");
                            }
                        }
                                 
                    }
                    finalResult.Add(result);
                    return finalResult;
                }     
            }
            
            return returnTypes;
        }
    }
}