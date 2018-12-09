namespace Dlanguage
{
    class Convert
    {
        public static FactorOp toFactor(string s)
        {
            if (s == "plus_t")
                return FactorOp.Plus;
            return FactorOp.Minus;
        }
        public static TermOp toTerm(string s)
        {
            if (s == "mult_t")
                return TermOp.Mult;
            return TermOp.Div;
        }
        public static RelOp toRel(string s)
        {
            switch (s)
            {
                case "eq_t":
                    return RelOp.eq;
                case "neq_t":
                    return RelOp.neq;
                case "gt_t":
                    return RelOp.gt;
                case "ge_t":
                    return RelOp.ge;
                case "lt_t":
                    return RelOp.lt;
                default:
                    return RelOp.le;
            }
        }
        public static LogicOp toLogic(string s)
        {
            switch (s)
            {
                case "and_t":
                    return LogicOp.And;
                case "or_t":
                    return LogicOp.Or;
                default:
                    return LogicOp.Xor;
            }
        }
        public static PrimaryType toRead(string s)
        {
            switch (s)
            {
                case "readInt_t":
                    return PrimaryType.readInt;
                case "readReal_t":
                    return PrimaryType.readReal;
                default:
                    return PrimaryType.readString;
            }
        }
        public static Type toType(string s)
        {
            switch (s)
            {
                case "int_t":
                    return Type.Int;
                case "real_t":
                    return Type.Real;
                case "string_t":
                    return Type.String;
                case "empty_t":
                    return Type.Empty;
                case "bool_t":
                    return Type.Bool;
                case "func_t":
                    return Type.Func;
                case "open_vector_bracket_t":
                    return Type.Arr;
                default:
                    return Type.Tup;
            }
        }
        public bool toBool(string s)
        {
            return s == "true_t";
        }
    }
}