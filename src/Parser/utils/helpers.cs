namespace Dlanguage
{
    class tokenType
    {
        public static string getValue(string tok)
        {
            return tok.Split("_")[1];
        }
        public static bool isRead(string t)
        {
            return t == "readInt_t" || t == "readReal_t" || t == "readString_t";
        }
        public static bool isSemi(string s)
        {
            return s == "semi_t";
        }
        public static bool isLogic(string s)
        {
            return s == "and_t" || s == "xor_t" || s == "or_t";
        }
        public static bool isRel(string s)
        {
            return s == "eq_t" || s == "neq_t" || s == "gt_t" || s == "lt_t" || s == "ge_t" || s == "le_t";
        }
        public static bool isBool(string s)
        {
            return s == "true_t" || s == "false_t";
        }
        public static string indent(int i)
        {
            return new string(' ', i);
        }
    }
}