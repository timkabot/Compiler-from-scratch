namespace Dlanguage
{
    using System;
    using System.Collections.Generic;
    class SyntaxError : Exception
    {
        public SyntaxError() : base() { }
        public SyntaxError(string error) : base(error) { }
    }

    class SemanticError : Exception
    {
        public SemanticError(string err) : base(err) { }
    }
}