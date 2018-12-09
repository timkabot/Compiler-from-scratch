namespace Dlanguage
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Diagnostics;
    using System;
    public class Parser
    {
        private List<string> tokens;
        private int cur = 0; // current scope
        private List<Scope> scopes = new List<Scope>();
        public Parser(List<string> tokens)
        {
            this.tokens = tokens;
            //add global scope by default
            scopes.Add(new Scope());
        }


        /*
        Main function that return AST
         */
        public rootNode getAST()
        {
            return parseProgram(tokens);
        }

        /*
        Parses Program : list of declarations
         */
        private rootNode parseProgram(List<string> tokens)
        {
            var root = new rootNode();
            root.setProgram(parseBody(tokens));
            return root;
        }
        /*
            parse Declaration
            returns index of next declaration , identifier and DecNode
        */
        private DecNode parseDeclaration(List<string> tokens)
        {
            ////Console.WriteLine("Inside Parse Declar");
            //printList(tokens);
            if (tokens.Count > 1 && tokens[2] == "assign_t")
            {
                if (tokens.Count > 3)
                {
                    var node = parseExpression(tokens.GetRange(3, tokens.Count - 3));
                    return new DecNode(tokens[1], node);
                }
                else
                    throw new SyntaxError("Syntax Error");
            }
            else
            {
                return new DecNode(tokens[1], null);
            }
        }

        /*
            parse Expression
            returns Expression node with relation / relation op relation
         */
        private ExprNode parseExpression(List<string> tokens)
        {
            // Console.WriteLine("Inside Parse Expression");
            // printList(tokens);
            int ind = findToken(tokens, tokenType.isLogic, 0);
            if (ind == -1)
            {
                return new ExprNode(parseRelation(tokens));
            }
            else
            {
                return new ExprNode(
                    parseRelation(tokens.GetRange(0, ind)),
                    tokens[ind],
                    parseRelation(tokens.GetRange(ind + 1, tokens.Count - ind - 1))
                );
            }
        }
        private RelNode parseRelation(List<string> tokens)
        {
            ////Console.WriteLine("Parse Relation");
            //printList(tokens);
            int ind = findToken(tokens, tokenType.isRel, 0);
            if (ind == -1)
            {
                return new RelNode(parseFactor(tokens));
            }
            else
            {
                return new RelNode(
                    parseFactor(tokens.GetRange(0, ind)),
                    tokens[ind],
                    parseFactor(tokens.GetRange(ind + 1, tokens.Count - ind - 1))
                );
            }
        }
        private int findToken(List<string> tokens, Func<string, bool> f, int start)
        {
            int r = 0;
            int c = 0;
            int s = 0;
            for (int ind = start; ind < tokens.Count; ++ind)
            {
                if (f(tokens[ind]) && r == 0 && s == 0 && c == 0 && ind != 0)
                    return ind;
                if (tokens[ind] == "lround_t")
                    r += 1;
                if (tokens[ind] == "rround_t")
                    r -= 1;
                if (tokens[ind] == "lcurly_t")
                    c += 1;
                if (tokens[ind] == "rcurly_t")
                    c -= 1;
                if (tokens[ind] == "lsquare_t")
                    s += 1;
                if (tokens[ind] == "rsquare_t")
                    s -= 1;
            }
            return -1;
        }
        private FactorNode parseFactor(List<string> tokens)
        {
            ////Console.WriteLine("Inside Parse Factor");
            ////printList(tokens);
            int ind = findToken(tokens, isTerm, 0);
            if (ind == -1)
            {
                return new FactorNode(parseTerm(tokens));
            }
            else
            {
                var complterms = new List<ComplexTerm>();
                parseComplexTerms(tokens.GetRange(ind, tokens.Count - ind), ref complterms);
                //Console.WriteLine("CHECK");
                //printList(tokens);
                return new FactorNode(
                    parseTerm(tokens.GetRange(0, ind)),
                    complterms
                );
            }
        }
        private TermNode parseTerm(List<string> tokens)
        {
            //Console.WriteLine("Inside Parse Term");
            //printList(tokens);
            int ind = findToken(tokens, isFactor, 0);

            if (ind == -1)
            {
                return new TermNode(parseUnary(tokens));
            }
            else
            {
                var complexTerms = new List<ComplexUnary>();
                parseComplexUnary(tokens.GetRange(ind, tokens.Count - ind), complexTerms);
                return new TermNode(
                    parseUnary(tokens.GetRange(0, ind)),
                    complexTerms
                );
            }
        }
        private void parseComplexTerms(List<string> tokens, ref List<ComplexTerm> tail)
        {
            //Console.WriteLine("Inside Parse Complex term");
            //printList(tokens);
            int curr = 0;
            while (true)
            {
                if (isTerm(tokens[curr]))
                {
                    int end = findToken(tokens, isTerm, curr + 1);

                    if (end != -1)
                    {

                        tail.Add(new ComplexTerm(parseTerm(tokens.GetRange(curr + 1, end - curr - 1)), tokens[curr]));
                        curr = end;
                    }
                    else
                    {
                        tail.Add(new ComplexTerm(parseTerm(tokens.GetRange(curr + 1, tokens.Count - curr - 1)), tokens[curr]));
                        break;
                    }
                }
                else throw new SyntaxError("Syntax Error");
            }
        }
        private bool isFactor(string s)
        {
            return s == "mult_t" || s == "div_t";
        }
        private bool isTerm(string s)
        {
            return s == "plus_t" || s == "minus_t";
        }



        /*
        takes tokens and empty list as input
        and it should find all ComplexUnary Nodes in tokens and add them to tail list
         ComplexUnary has shape of    ( * | /) Unary
         */
        private void parseComplexUnary(List<string> tokens, List<ComplexUnary> tail)
        {
            //Console.WriteLine("Inside Parse Complex Unary");
            //printList(tokens);
            int curr = 0;
            while (true)
            {
                if (isFactor(tokens[curr]))
                {
                    int end = findToken(tokens, isFactor, curr + 1);

                    if (end != -1)
                    {
                        tail.Add(new ComplexUnary(tokens[curr], parseUnary(tokens.GetRange(curr + 1, end - curr - 1))));
                        curr = end;
                    }
                    else
                    {
                        tail.Add(new ComplexUnary(tokens[curr], parseUnary(tokens.GetRange(curr + 1, tokens.Count - curr - 1))));
                        break;
                    }
                }
                else throw new SyntaxError("Syntax Error");
            }
        }

        /*

         */

        /*
        check whether s is + | - | not
         */
        private bool isPrimaryPrefix(string s)
        {
            return s == "plus_t" || s == "minus_t" || s == "not_t";
        }

        private bool isLiteral(List<string> tokens)
        {
            string t = tokens[0];
            return (t.Length > 4 && "int__".Equals(t.Substring(0, 5)))
                || (t.Length > 5 && "real__".Equals(t.Substring(0, 6)))
                || (t.Length > 4 && "str__".Equals(t.Substring(0, 5)))
                || t == "true_t" || t == "false_t" || t == "lcurly_t"
                || t == "lsquare_t" || t == "func_t";
        }
        private bool isExpr(List<string> tokens)
        {
            return tokens[0] == "lround_t" && tokens.Last() == "rround_t";
        }
        private UnaryNode parseUnary(List<string> tokens)
        {
            //Console.WriteLine("Inside Parse Unary");
            //printList(tokens);
            if (isLiteral(tokens))
            {
                // //Console.WriteLine("isLiteral");
                var node = new UnaryNode(UnaryType.Literal);
                node.setLiteral(parseLiteral(tokens));
                return node;
            }
            else if (isExpr(tokens))
            {
                // //Console.WriteLine("isExpr");

                var node = new UnaryNode(UnaryType.Expr);
                node.setExpr(parseExpression(tokens.GetRange(1, tokens.Count - 2)));
                return node;
            }
            else
            {
                // //Console.WriteLine("isPrimary");

                string pref = null;
                TypeNode tn = null;
                int start = 0, end = tokens.Count - 1; // start and end are boundary indeces of primary node
                var node = new UnaryNode(UnaryType.Primary);
                if (isPrimaryPrefix(tokens[0]))
                {
                    pref = tokens[0];
                    start += 1;

                }
                int isInd = findToken(tokens, (string s) => (s == "is_t"), 0);
                if (isInd != -1)
                {
                    end = isInd - 1;
                    tn = parseType(tokens.GetRange(isInd + 1, tokens.Count - isInd - 1));
                }
                node.setPrimary(
                    pref,
                    parsePrimary(tokens.GetRange(start, end - start + 1)),
                    tn
                );
                return node;
            }
        }
        public static void printList(List<string> s)
        {
            foreach (var item in s)
                Console.Write(item + " ");
            Console.WriteLine("\n");

        }
        private PrimaryNode parsePrimary(List<string> tokens)
        {
            //Console.WriteLine("Inside Parse Primary");
            //printList(tokens);
            PrimaryNode pn;
            if (tokenType.isRead(tokens[0]))
            {
                pn = new PrimaryNode(Convert.toRead(tokens[0]));
            }
            else
            {
                pn = new PrimaryNode(PrimaryType.Id);
                if (tokens.Count == 1)
                {
                    //Console.WriteLine("ITS OK");
                    pn.setId(tokens[0], null);
                }
                else
                    pn.setId(tokens[0], parseTail(tokens.GetRange(1, tokens.Count - 1)));
            }
            return pn;
        }
        private bool hasType(List<string> tokens)
        {
            string s = tokens[0];
            return s == "bool_t" || s == "int_t" || s == "real_t" || s == "string_t"
                || s == "empty_t" || s == "lcurly_t" || s == "lsquare_t"
                || s == "func_t" || findToken(tokens, (string st) => (st == "dots_t"), 0) != -1;
        }
        private List<Tail> parseTail(List<string> tokens)
        {
            //Console.WriteLine("Inside Parse Tail");
            //printList(tokens);
            if (tokens.Count < 2)
                return null;
            var tails = new List<Tail>();
            int cur = 0;
            while (true)
            {
                if (cur >= tokens.Count)
                    return tails;
                if (tokens[cur] == "dot_t")
                {
                    int lit;
                    if (Int32.TryParse(tokens[cur + 1].Split("__")[1], out lit))
                    {
                        tails.Add(new Tail(TailType.IntLiteral));
                        tails.Last().setLit(lit);
                        cur += 2;
                        continue;
                    }
                    else if (tokens[cur + 1].Length > 3 && "id__".Equals(tokens[cur + 1].Substring(0, 4)))
                    {
                        tails.Add(new Tail(TailType.Id));
                        tails.Last().setId(tokens[cur + 1]);
                        cur += 2;
                        continue;
                    }
                    else
                        throw new SyntaxError("After dot must be int literal or Id");
                }
                else if (tokens[cur] == "lsquare_t")
                {
                    int closed = findToken(tokens, (string s) => (s == "rsquare_t"), cur + 1);
                    if (closed != -1)
                    {
                        tails.Add(new Tail(TailType.ArrIndex));
                        tails.Last().setIndex(parseExpression(
                            tokens.GetRange(cur + 1, closed - cur - 1)
                        ));
                        cur = closed + 1;
                        continue;
                    }
                    else
                        throw new SyntaxError("must be ]");
                }
                else if (tokens[cur] == "lround_t")
                {
                    //Console.WriteLine("CHECK FOR ) {0}", cur);
                    int closed = findToken(tokens, (string s) => (s == "rround_t"), cur + 1);
                    if (closed != -1)
                    {
                        tails.Add(new Tail(TailType.FuncArgs));
                        tails.Last().setArgs(
                            parseExpressions(tokens.GetRange(cur + 1, closed - cur - 1))
                        );
                        cur = closed + 1;
                        continue;
                    }
                    else
                    {
                        ////printList(tokens);
                        throw new SyntaxError("must be )");
                    }
                }
            }
        }
        private List<ExprNode> parseExpressions(List<string> tokens)
        {
            // Console.WriteLine("Inside EXPRESSIONS");
            // printList(tokens);
            int cur = 0;
            // foreach (string s in tokens)
            // //Console.Write(s + " ,");
            var epxrs = new List<ExprNode>();
            if (tokens.Count == 0)
                return epxrs;
            while (true)
            {
                int coma = findToken(tokens, (string s) => (s == "coma_t"), cur);
                if (coma != -1)
                {
                    // //Console.WriteLine("inside parseExpS must be coma: {0}", tokens[coma]);
                    // //Console.WriteLine("inside parseExpS arr range: {0}, {1}", cur, coma - 1);
                    epxrs.Add(parseExpression(tokens.GetRange(cur, coma - cur)));
                    cur = coma + 1;
                }

                else
                {

                    // //Console.WriteLine("inside parseExpS arr range: {0}, {1}", cur, tokens.Count - 1);
                    epxrs.Add(parseExpression(tokens.GetRange(cur, tokens.Count - cur)));// cur = 0 ind 3 count 4
                    return epxrs;
                }
            }

            throw new ArgumentException();
        }
        private TypeNode parseType(List<string> tokens)
        {
            //Console.WriteLine("Inside parse Type");
            //printList(tokens);
            int ind = findToken(tokens, (string s) => (s == "dots_t"), 0);
            if (ind == -1)
            {
                return new TypeNode(Convert.toType(tokens[0]));
            }
            else
            {
                var node = new TypeNode(Type.Expr);
                node.setExpr(
                    parseExpression(tokens.GetRange(0, ind)),
                    parseExpression(tokens.GetRange(ind + 1, tokens.Count - ind - 1))
                );
                return node;
            }
        }

        private LiteralNode parseLiteral(List<string> tokens)
        {
            //Console.WriteLine("Inside parse Literal");
            //printList(tokens);
            if (tokens.Count == 1)
            {
                // //Console.WriteLine("HERE");
                int ilit;
                double dlit;
                if (tokens[0].Substring(0, 3) == "int" && Int32.TryParse(tokens[0].Split("__")[1], out ilit))
                    return new LiteralNode(LiteralType.Int, ilit);
                else if (tokens[0].Substring(0, 4) == "real" && Double.TryParse(tokens[0].Split("__")[1], out dlit))
                    return new LiteralNode(LiteralType.Real, dlit);
                else if (tokens[0].Length > 4 && "str__" == tokens[0].Substring(0, 5))
                    return new LiteralNode(LiteralType.String, tokens[0]);
                else
                    return new LiteralNode(LiteralType.Bool, tokens[0] == "true_t");
            }
            else
            {
                if (tokens[0] == "lsquare_t" && tokens.Last() == "rsquare_t")
                {
                    if (tokens.Count == 2)
                        return new LiteralNode(LiteralType.Arr, null);
                    else
                        return new LiteralNode(
                            LiteralType.Arr,
                            parseExpressions(tokens.GetRange(1, tokens.Count - 2))
                        );
                }
                else if (tokens[0] == "lcurly_t" && tokens.Last() == "rcurly_t")
                {
                    if (tokens.Count == 2)

                        return new LiteralNode(LiteralType.Tup, null);
                    else
                        return new LiteralNode(
                            LiteralType.Tup,
                            parseAugmentExpressions(tokens.GetRange(1, tokens.Count - 2))
                        );
                }
                else if (tokens[0] == "func_t")
                    return new LiteralNode(LiteralType.Func, parseFunc(tokens));
                else
                {
                    //Console.WriteLine("Wrong Lit: {0}", tokens[0]);
                    throw new SyntaxError("wrong Literal");
                }
            }

        }
        private FuncLiteral parseFunc(List<string> tokens)
        {
            //Console.WriteLine("Inside Parse Func");
            //printList(tokens);
            int param = findToken(tokens, (string s) => (s == "is_t" || s == "lambda_t"), 0);
            if (param > 1)
            {
                return new FuncLiteral(
                    parseParams(tokens.GetRange(1, param - 1)),
                    parseFunBody(tokens.GetRange(param, tokens.Count - param))
                );
            }
            else if (param == 1)
                return new FuncLiteral(null, parseFunBody(tokens.GetRange(1, tokens.Count - 1)));
            else throw new SyntaxError("GAVNO");
        }
        private Parameters parseParams(List<string> tokens)
        {
            //Console.WriteLine("Insde parse params");
            //printList(tokens);
            if (tokens.Count < 2)
                throw new SyntaxError("syntax error");
            var t = tokens.GetRange(1, tokens.Count - 2);
            var ids = new List<string>();
            if (t.Count % 2 == 0)
                throw new SyntaxError("Syntax error");
            for (int ind = 0; ind < t.Count; ind += 2)
                ids.Add(t[ind]);
            return new Parameters(ids);

        }
        private FunBody parseFunBody(List<string> tokens)
        {
            //Console.WriteLine("Inside parse Fun Body");
            //printList(tokens);
            if (tokens[0] == "is_t")
            {
                var body = new FunBody(FuncType.Complex);
                body.setBody(parseBody(tokens.GetRange(2, tokens.Count - 4)));
                return body;
            }
            else
            {
                var body = new FunBody(FuncType.Expr);
                body.setExpr(parseExpression(tokens.GetRange(1, tokens.Count - 1)));
                return body;
            }

        }
        private List<Tuple<string, ExprNode>> parseAugmentExpressions(List<string> tokens)
        {
            //Console.WriteLine("Inside parse Augment Express ( pair in tuple");
            ////printList(tokens);
            int cur = 0;
            var tupes = new List<Tuple<string, ExprNode>>();
            while (true)
            {
                int ind = findToken(tokens, (string s) => (s == "coma_t"), cur);
                if (ind != -1)
                {
                    var (id, expr) = parseAugmentExpr(tokens.GetRange(cur, ind - cur));
                    tupes.Add(Tuple.Create(id, expr));
                    cur = ind + 1;
                    continue;
                }
                else
                {
                    var (id, expr) = parseAugmentExpr(tokens.GetRange(cur, tokens.Count - cur)); // cur = 2 count = 4 ind  3
                    tupes.Add(Tuple.Create(id, expr));
                    return tupes;
                }
            }
        }
        private (string, ExprNode) parseAugmentExpr(List<string> tokens)
        {
            //Console.WriteLine("inside parse Pair (id, expr)");
            ////printList(tokens);
            if (tokens.Count >= 2 && tokens[1] == "assign_t")
                return (tokens[0], parseExpression(tokens.GetRange(2, tokens.Count - 2)));
            else
                return (null, parseExpression(tokens));
        }
        /*
            !!!!!  EVERY KIND OF STATEMENT (ass, return //print loop if )  HAS SEMI COLON AT THE END !!!!!!!!!!

         */
        private List<StatNode> parseStatements(List<string> tokens)
        {
            //Console.WriteLine("Inside parse Statmens");
            //printList(tokens);
            if (tokens.Count == 0)
                return null;
            var statements = new List<StatNode>();
            int cur = 0;
            // foreach (string s in tokens)
            // //Console.Write(s + " ");
            // //Console.WriteLine();
            while (true)
            {
                if (cur >= tokens.Count)
                    return statements;
                int ind = findToken(tokens, (string s) => (s == "semi_t"), cur + 1);
                List<string> t;
                if (ind != -1)
                {
                    // //Console.WriteLine("\n\nITS OK\n\n\n");

                    t = tokens.GetRange(cur, ind - cur);
                    cur = ind + 1;
                }
                else
                {
                    // ////Console.WriteLine("\n\nITS OK\n\n\n");
                    t = tokens.GetRange(cur, tokens.Count - cur);
                    cur = tokens.Count;
                }
                StatType type = toStateType(t);
                statements.Add(new StatNode(type));
                switch (type)
                {
                    case StatType.AssNode:
                        statements.Last().setAssignment(parseAssignment(t));
                        break;
                    case StatType.PrintNode:
                        // ////Console.WriteLine("JEPA");
                        statements.Last().setPrint(parsePrint(t));
                        break;
                    case StatType.ReturnNode:
                        statements.Last().setReturn(parseReturn(t));
                        break;
                    case StatType.IfNode:
                        ////Console.WriteLine("\nITS OK\n");
                        statements.Last().setIf(parseIf(t));
                        break;
                    default:
                        statements.Last().setLoop(parseLoop(t));
                        break;
                }
            }

        }
        private StatType toStateType(List<string> tokens)
        {
            if (tokens[0] == "print_t")
                return StatType.PrintNode;
            if (tokens[0] == "while_t" || tokens[0] == "for_t")
                return StatType.LoopNode;
            if (tokens[0] == "if_t")
                return StatType.IfNode;
            if (tokens[0] == "return_t")
                return StatType.ReturnNode;
            return StatType.AssNode;
        }
        private AssNode parseAssignment(List<string> tokens)
        {
            //Console.WriteLine("Inside parse Assignment");
            //printList(tokens);
            int ind = findDelim(tokens);
            //Console.WriteLine("Index of Delim: {0}", ind);
            return new AssNode(
                parsePrimary(tokens.GetRange(0, ind)),
                parseExpression(tokens.GetRange(ind + 1, tokens.Count - ind - 1))
            );
        }
        private bool nextTail(string s)
        {
            return s == "dot_t" || s == "lsquare_t";
        }
        private int findDelim(List<string> tokens)
        {
            int cur = 1;
            while (nextTail(tokens[cur]))
            {
                if (tokens[cur] == "dot_t")
                    cur += 2;
                else
                    cur = findToken(tokens, (string s) => (s == "rsquare_t"), cur + 1) + 1;
            }
            return cur;
        }
        private PrintNode parsePrint(List<string> tokens)
        {
            //Console.WriteLine("inside Parse Print");
            //printList(tokens);
            return new PrintNode(parseExpressions(tokens.GetRange(1, tokens.Count - 1)));
        }
        private ReturnNode parseReturn(List<string> tokens)
        {
            if (tokens.Count == 1)
                return new ReturnNode();
            else
                return new ReturnNode(parseExpression(tokens.GetRange(1, tokens.Count - 1)));
        }
        private IfNode parseIf(List<string> tokens)
        {
            //Console.WriteLine("Inside parse If");
            //printList(tokens);
            if (tokens.Count < 6 || tokens[0] != "if_t" || tokens.Last() != "end_t")
                throw new SyntaxError("Syntax Error");
            int _then = findToken(tokens, (string s) => (s == "then_t"), 0);
            var expr = parseExpression(tokens.GetRange(1, _then - 1));
            int open = findToken(tokens, (string s) => (s == "lcurly_t"), 0);
            int closed = findToken(tokens, (string s) => (s == "rcurly_t"), open + 1);
            //Console.WriteLine(open.ToString(), closed.ToString());
            // //Console.WriteLine("Open and closed curly bracket in IF statement: {0}, {1}", open, closed);
            if (closed == tokens.Count - 2)
                return new IfNode(
                    parseExpression(tokens.GetRange(1, _then - 1)),
                    parseBody(tokens.GetRange(open + 1, closed - open - 1))
                );
            else
            {
                if (tokens.Count < 9 || tokens[closed + 1] != "else_t")
                    throw new SyntaxError("Syntax Error");
                return new IfNode(
                    parseExpression(tokens.GetRange(1, _then - 1)),
                    parseBody(tokens.GetRange(open + 1, closed - open - 1)),
                    parseBody(tokens.GetRange(closed + 3, tokens.Count - closed - 5)) // closed = 3 else 4 { 5 ind 6  end 7 count 8
                );
            }
        }
        private LoopNode parseLoop(List<string> tokens)
        {
            if (tokens[0] == "while_t")
            {
                var node = new LoopNode(LoopType.While);
                var (e, body) = parseWhile(tokens);
                node.setWhilte(e, body);
                return node;
            }
            else
            {
                var node = new LoopNode(LoopType.For);
                var (i, it, body) = parseFor(tokens);
                node.setFor(i, it, body);
                return node;
            }
        }
        private (ExprNode, BodyNode) parseWhile(List<string> tokens)
        {
            //Console.WriteLine("inside Parse While");
            //printList(tokens);
            int ind = findToken(tokens, (string s) => (s == "loop_t"), 0);
            if (ind == -1)
                throw new SyntaxError("loop clause is missing");
            else
            {

                if (tokens.Last() != "end_t")
                    throw new SyntaxError("end clause is missing");
                else
                    return (parseExpression(tokens.GetRange(1, ind - 1)),
                            parseBody(tokens.GetRange(ind + 2, tokens.Count - ind - 4))
                    );
            }
        }
        private (string, IterNode, BodyNode) parseFor(List<string> tokens)
        {
            int _in = findToken(tokens, (string s) => (s == "in_t"), 0);
            int _loop = findToken(tokens, (string s) => (s == "loop_t"), 0);
            if (tokens[0] != "for_t" || _in == -1 || _loop == -1 || tokens.Last() != "end_t")
                throw new SyntaxError("Syntax Error");
            return (tokens[1],
                    parseIter(tokens.GetRange(3, _loop - 3)),
                    parseBody(tokens.GetRange(_loop + 2, tokens.Count - _loop - 4)) // loop 2 count 5 ind 4
            );
        }
        private IterNode parseIter(List<string> tokens)
        {
            int ind = findToken(tokens, (string s) => (s == "dots_t"), 0);
            if (ind == -1)
                throw new SyntaxError("Syntax Error");
            else
            {
                var it = new IterNode(
                    parseExpression(tokens.GetRange(0, ind)),
                    parseExpression(tokens.GetRange(ind + 1, tokens.Count - ind - 1))
                );
                return it;
            }

        }
        private bool isStatement(List<string> tokens)
        {
            return findToken(tokens, (string s) => (s == "assign_t"), 0) != -1
                || tokens[0] == "print_t" || tokens[0] == "return_t" || tokens[0] == "if_t" || tokens[0] == "while_t"
                || tokens[0] == "for_t";
        }
        private BodyType toBody(List<string> t)
        {
            if (t[0] == "var_t")
                return BodyType.Decl;
            if (isStatement(t))
                return BodyType.Stat;
            return BodyType.Expr;
            // throw new SyntaxError("Syntax error");
        }
        // returns List of (type, object)
        private BodyNode parseBody(List<string> tokens)
        {

            if (tokens.Count == 0)
                return null;
            int cur = 0;
            var body = new BodyNode();
            while (true)
            {
                if (cur >= tokens.Count)
                    return body;
                int ind = findToken(tokens, (string s) => (s == "semi_t"), cur);
                if (ind == -1)
                {
                    ////printList(tokens);
                    throw new SyntaxError("; is missing");
                }
                else
                {
                    var t = tokens.GetRange(cur, ind - cur); // cur = 3 ind  5
                    var type = toBody(t);
                    switch (type)
                    {
                        case BodyType.Decl:
                            body.add(type, parseDeclaration(t));
                            break;
                        case BodyType.Expr:
                            body.add(type, parseExpression(t));
                            break;
                        default:
                            body.addAll(type, parseStatements(t));
                            break;
                    }
                    cur = ind + 1;
                }
            }
        }
    }
}