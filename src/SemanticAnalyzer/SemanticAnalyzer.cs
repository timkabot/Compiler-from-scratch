using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace Dlanguage.SemanticAnalyzer
{
    public class SemanticAnalyzer
    {
        private rootNode root = null;

        public SemanticAnalyzer(rootNode root)
        {
            this.root = root;
        }

        public void TypeCheck()
        {
            //here we will check semantic types
            List<Type> myTypes = bfs(root);
             for (int i = 0; i < myTypes.Count; i++)
            {
               // Console.Write(myTypes[i]);
            }
           
        }

        public List<Type> bfs(BaseNode node)
        {            
            if (node.GetType() == typeof(LiteralNode))
            {
                LiteralNode n = (LiteralNode) node;
                List<Type> temp = new List<Type>();
                temp.Add(n.getReturnType());
                return temp;
            }
            
            List<BaseNode> children = node.getChildren().ToList();
            List<Type> returnTypes = new List<Type>(); 
            
            for (int i = 0; i < children.Count(); i++)
            {
                if(children[i] == null || children[i].GetType() == typeof(PrintNode)) continue;
                List<Type> temp = bfs(children[i]);
                for (int j = 0; j < temp.Count; j++)
                   returnTypes.Add(temp[j]);
            }
            
            //check for and | or | xor
            if (node.GetType() == typeof(ExprNode))
            {
                
                List<Type> finalResult = new List<Type>();
                finalResult.Add(Type.Bool);
                ExprNode n = (ExprNode) node;
                if (n.getRightRelation() != null)
                {
                    RelNode left = n.getLeftRelation(), right = n.getRightRelation();
                    if (returnTypes[0] != Type.Bool && returnTypes[1] != Type.Bool)
                    {
                        throw new Exception("wrong comparison of types");
                    }

                    return finalResult;
                }
            
            }
            
            //check for < | <= | > | >= | = | /=
            if (node.GetType() == typeof(RelNode))
            {
                List<Type> finalResult = new List<Type>();
                finalResult.Add(Type.Bool);
                RelNode n = (RelNode) node;
                if (n.getRFactor() != null && returnTypes.Count==2)
                {
                    Type left = returnTypes[0], right = returnTypes[1];
                    if (left == Type.Int && right == Type.Int)     {}
                    else if(left == Type.Int && right==Type.Real)  {}
                    else if(left == Type.Real && right==Type.Int)  {}
                    else if(left == Type.Real && right==Type.Real) {}
                    else
                    {
                        throw new Exception("Comparison types error");
                    }
                    return finalResult;

                }
            }
            
            //check for +,-
            if(node.GetType() ==  typeof(FactorNode))
            {
                FactorNode n = (FactorNode) node;
                if (n.getTail() != null && returnTypes.Count>0)
                {
                    Type result = returnTypes[0];
                    List<Type> finalResult = new List<Type>();
                    if (result == Type.Func)
                        result = Type.Int;
                    
                  
                    
                    for (int i = 1; i < returnTypes.Count; i++)
                    {
                        Type temp = returnTypes[i]; 
                        ComplexTerm tempTerm = (ComplexTerm) children[i];
                        FactorOp sign = tempTerm.getop();
                      

                        if (sign == FactorOp.Minus)
                        {
                            if (result == Type.Int && temp == Type.Int)    { }
                            else if (result==Type.Int && temp==Type.Real)    {result = Type.Real;}
                            else if (result==Type.Real && temp==Type.Int)    {result = Type.Real;}
                            else if(result==Type.Real && temp==Type.Real)  { }
                            else
                            {
                                throw new Exception("Wrong subtraction found");
                            }   
                        }
                        else if (sign == FactorOp.Plus)
                        {
                            if (result == Type.Int && temp == Type.Int)    { }
                            else if (result==Type.Int && temp==Type.Real)    {result = Type.Real;}
                            else if (result==Type.Real && temp==Type.Int)    {result = Type.Real;}
                            else if(result==Type.Real && temp==Type.Real)  { }
                            else if(result ==Type.String && temp==Type.String){}
                            else if(result==Type.Tup && temp==Type.Tup){}
                            else if(result==Type.Arr && temp==Type.Arr){}
                            else
                            {
                                throw new Exception("Wrong addition found");
                            }   
                        }
                                                 
                    }
                    finalResult.Add(result);
                    return finalResult;
                }     
            }
            //check for /,*
            if (node.GetType() == typeof(TermNode))
            {    
                TermNode n = (TermNode) node;
                if (n.getTail() != null)
                {
                    Type result = returnTypes[0];
                    List<Type> finalResult = new List<Type>();
                    if (result == Type.Func)
                        result = Type.Int;

                    for (int i = 1; i < returnTypes.Count; i++)
                    {
                        Type temp = returnTypes[i]; 
                        ComplexUnary tempUnary = (ComplexUnary) children[i];
                        TermOp sign = tempUnary.getOp();
                        if (sign == TermOp.Div)
                        {
                            if (result == Type.Int && temp == Type.Int)    { }
                            else if (result==Type.Int && temp==Type.Real)    {result = Type.Real;}
                            else if (result==Type.Real && temp==Type.Int)    {result = Type.Real;}
                            else if(result==Type.Real && temp==Type.Real)  { }
                            else
                            {
                                throw new Exception("Wrong division found");
                            }   
                        }
                        else if (sign == TermOp.Mult)
                        {
                            if(result == Type.Int && temp == Type.Int){}//result stays the same
                            else if (result == Type.Int && temp == Type.Real)
                                result = Type.Real;
                            else if (result == Type.Real && temp == Type.Int)
                                result = Type.Real;
                            else if(result == Type.Real && temp == Type.Real){}
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
            /*Console.Write(node.GetType() +" : ");
            for (int i = 0; i < returnTypes.Count; i++)
            {
                Console.Write(returnTypes[i] + " ");
            }
            Console.WriteLine();*/
            return returnTypes;
        }
    }
}