using System;
using System.IO;
using System.Net.Http.Headers;

namespace Dlanguage
{
    class Program
    {
        static void Main(string[] args)
        {
            var tokens = Tokenizer.Parse();
            var parser = new Parser(tokens);
            var root = parser.getAST();

            //Semantic analyze
            SemanticAnalyzer.SemanticAnalyzer analyzer = new SemanticAnalyzer.SemanticAnalyzer(root);
            analyzer.TypeCheck();
            
            //Interpretator
            Interpretator.Interpretator i =
                new Interpretator.Interpretator(root);
            i.imitateProgram();
            
            File.Delete("./output");
            StreamWriter sw = new StreamWriter(File.OpenWrite("./output"));

            root.checkScopes();

            //Console.WriteLine(root.checkScopes());
            root.Draw(sw);

            sw.Close();
        }
    }
}
