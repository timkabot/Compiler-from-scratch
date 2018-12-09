namespace Dlanguage
{
    using System.Collections.Generic;
    class Tokens
    {
        private static Dictionary<string, string> tokens;

        public static Dictionary<string, string> initTokens()
        {

            if (tokens == null)
                tokens = new Dictionary<string, string>();
            tokens.Add("if", "if_t");
            tokens.Add("then", "then_t");
            tokens.Add("else", "else_t");
            tokens.Add("var", "var_t");
            tokens.Add("end", "end_t");

            tokens.Add("or", "or_t");
            tokens.Add("xor", "xor_t");
            tokens.Add("not", "not_t");
            tokens.Add("and", "and_t");

            tokens.Add("readInt", "readInt_t");
            tokens.Add("readReal", "readReal_t");
            tokens.Add("readString", "readString_t");

            tokens.Add("print", "print_t");
            tokens.Add("return", "return_t");
            tokens.Add("bool", "bool_t");

            tokens.Add("true", "true_t");
            tokens.Add("false", "false_t");
            tokens.Add("while", "while_t");
            tokens.Add("func", "func_t");
            tokens.Add("for", "for_t");
            tokens.Add("in", "in_t");
            tokens.Add("loop", "loop_t");
            tokens.Add("int", "int_t");
            tokens.Add("real", "real_t");
            tokens.Add("string", "string_t");
            tokens.Add("empty", "empty_t");
            tokens.Add("is", "is_t");

            tokens.Add(":=", "assign_t");
            tokens.Add("<", "lt_t");
            tokens.Add(">", "gt_t");
            tokens.Add("<=", "le_t");
            tokens.Add(">=", "ge_t");
            tokens.Add("=", "eq_t");
            tokens.Add("/=", "neq_t");
            tokens.Add("+", "plus_t");
            tokens.Add("-", "minus_t");
            tokens.Add("*", "mult_t");
            tokens.Add("/", "div_t");
            tokens.Add("[", "lsquare_t");
            tokens.Add("]", "rsquare_t");
            tokens.Add("{", "lcurly_t");
            tokens.Add("}", "rcurly_t");
            tokens.Add("=>", "lambda_t");
            tokens.Add(";", "semi_t");
            tokens.Add("(", "lround_t");
            tokens.Add(")", "rround_t");
            tokens.Add(",", "coma_t");
            tokens.Add(".", "dot_t");
            tokens.Add("..", "dots_t");
            return tokens;
        }
    }

}