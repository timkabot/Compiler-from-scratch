using System.Collections.Generic;

namespace Dlanguage
{
    public class Tokenizer
    {
        private List<TokenDefinition> _tokenDefinitions;
        public Tokenizer()
        {
            _tokenDefinitions = new List<TokenDefinition>();

            _tokenDefinitions.Add(new TokenDefinition(TokenType.If, "^if"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.Then, "^then"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.Else, "^else"));
            
            _tokenDefinitions.Add(new TokenDefinition(TokenType.CloseParenthesis, "^\\)"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.Comma, "^,"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.Equals, "^="));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.Var, "^var"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.End, "^end"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.Or, "^or"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.Xor, "^xor"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.Not, "^not"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.ReadInt, "^readInt"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.ReadReal, "^readReal"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.ReadString, "^readString"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.Print, "^print"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.Return, "^return"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.Bool, "^bool"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.Empty, "^empty"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.True, "^true"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.False, "^false"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.Is, "^is"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.String, "^string"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.Func, "^func"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.For, "^for"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.SequenceTerminator, "^;"));

            _tokenDefinitions.Add(new TokenDefinition(TokenType.In, "^in"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.Loop, "^loop"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.Int, "^int"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.Real, "^real"));

            _tokenDefinitions.Add(new TokenDefinition(TokenType.Assign, "^:="));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.Smaller, "^<"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.Bigger, "^>"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.SmallerOrEq, "^<="));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.BiggerOrEq, "^>="));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.NotEquals, "^\\/="));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.Sum, "^\\+"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.Subtraction, "^\\-"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.Multiplication, "^\\*"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.Division, "^/"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.OpenVectorBracket, "^\\["));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.ClosedVectorBracket, "^\\]"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.OpenTupleBracket, "^\\{"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.CloseTupleBracket, "^\\}"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.ReturnType, "^=>"));

            _tokenDefinitions.Add(new TokenDefinition(TokenType.OpenParenthesis, "^\\("));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.StringValue, "^'[^']*'"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.Number, "^\\d+"));
        }
        public List<DslToken> Tokenize(string lqlText)
        {
            var tokens = new List<DslToken>();
            string remainingText = lqlText;

            while (!string.IsNullOrWhiteSpace(remainingText))
            {
                var match = FindMatch(remainingText);
                if (match.IsMatch)
                {
                    tokens.Add(new DslToken(match.TokenType, match.Value));
                    remainingText = match.RemainingText;
                }
                else
                {
                    remainingText = remainingText.Substring(1);
                }
            }

            tokens.Add(new DslToken(TokenType.EndOfProgram, string.Empty));

            return tokens;
        }

        private TokenMatch FindMatch(string lqlText)
        {
            foreach (var tokenDefinition in _tokenDefinitions)
            {
                var match = tokenDefinition.Match(lqlText);
                if (match.IsMatch)
                    return match;
            }

            return new TokenMatch() {  IsMatch = false };
        }
    }
}