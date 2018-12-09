namespace Dlanguage
{
    public enum LogicOp
    {
        And,
        Or,
        Xor
    }

    public enum RelOp
    {
        eq,
        neq,
        gt,
        lt,
        ge,
        le
    }

    public enum FactorOp
    {
        Plus,
        Minus
    }

    public enum TermOp
    {
        Mult,
        Div
    }
    public enum ResultType
    {
        Int,
        String,
        Bool,
        Real,
        Arr,
        Tuple,
        Void
    }
}