namespace Ancestry.Daisy.Language
{
    using System;

    public class InconsistentWhitespaceException : Exception
    {
        public InconsistentWhitespaceException(string msg) : base(msg) { }

        public int LineNumber { get; set; }
        public string Line { get; set; }
        public char UnexpectedWhitespace { get; set; }
    }
}