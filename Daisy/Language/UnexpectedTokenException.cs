namespace Ancestry.Daisy.Language
{
    using System;

    public class UnexpectedTokenException : Exception
    {
        public Token EncounteredToken { get; set; }

        public TokenKind[] ExpectedTokenKinds { get; set; }

        public UnexpectedTokenException(Token current, TokenKind kind): this(current, new[]{kind})
        {
        }

        public UnexpectedTokenException(Token current, TokenKind[] tokenKinds) :
            base(string.Format("On line {0} expected {1} but got {2}.",
            current.Line, string.Join(",",tokenKinds), current.Kind ))
        {
            EncounteredToken = current;
            ExpectedTokenKinds = tokenKinds;
        }
    }
}