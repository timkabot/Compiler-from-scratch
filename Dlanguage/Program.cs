using System;
using System.Collections.Generic;

namespace Dlanguage
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Tokenizer tokenizer = new Tokenizer();
            List<DslToken> tokens = tokenizer.Tokenize("var t := []; t[10] := 25; t[100] := func(x)=>x+1;  t[1000] := {a:=1,b:=2.7};");
            for (int i = 0; i < tokens.Count; i++)
            {
                Console.WriteLine( "(TokenType :" +  tokens[i].TokenType + ", TokenValue: " + tokens[i].Value +")");

            }
        }
    }
}