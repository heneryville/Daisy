namespace Ancestry.Daisy.Language
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public enum TokenKind
    {
        And,
        Or,
        Not,
        EOF,
        StartGroup,
        EndGroup,
        Statement
    }

    public struct Token
    {
        public TokenKind Kind { get; set; }

        public string Value { get; set; }

        public int Line { get; set; }
    }

    public class Lexer
    {
        private StreamReader reader;
        private WhitespaceEater eater = new WhitespaceEater();
        private int lineNum = 0;

        public Lexer(Stream stream)
        {
            reader = new StreamReader(stream);
        }

        public IEnumerable<Token> LexLine()
        {
            lineNum++;
            var line = reader.ReadLine();
            return InterpretLine(line);
        }

        private string ChunkOff(string line, string clause, List<Token> tokens, TokenKind kind)
        {
            if(line.StartsWith(clause + " ") || line  == clause)
            {
                tokens.Add(new Token() { Kind = kind, Line = lineNum});
                line = line.Substring(clause.Length).TrimStart();
            }
            return line;
        }

        public static int TrimLeadingSpaces(ref string line)
        {
            var @out = line.TrimStart();
            var count = line.Length - @out.Length;
            line = @out;
            return count;
        }

        internal IEnumerable<Token> InterpretLine(string line)
        {
            if (line == null) return null;
            var commentStart = line.IndexOf("//");
            if (commentStart >= 0) line = line.Substring(0, commentStart);
            if (line.Length == 0) return Enumerable.Empty<Token>();
            var tokens = new List<Token>();
            line = InterpretSpaces(tokens,line);

            line = ChunkOff(line, "AND", tokens, TokenKind.And);
            line = ChunkOff(line, "OR", tokens, TokenKind.Or);
            line = ChunkOff(line, "NOT", tokens, TokenKind.Not);
            line = line.Trim();

            if(line.Length != 0)
            {
                tokens.Add(new Token() { Kind = TokenKind.Statement, Line = lineNum, Value = line });
            }
            return tokens;
        }

        private string InterpretSpaces(List<Token> tokens, string line)
        {
            var morsel = eater.Eat(line, lineNum);
            foreach (var blah in Enumerable.Range(0,Math.Abs(morsel.DeltaIndents)))
            {
                tokens.Add(new Token() {
                    Kind = morsel.DeltaIndents > 0 ? TokenKind.StartGroup : TokenKind.EndGroup,
                    Line = lineNum});
            }
            return morsel.Line;
        }

        public IEnumerable<Token> Lex()
        {
            IEnumerable<Token> tokens = null;
            while( (tokens = LexLine()) != null)
            {
                foreach (var token in tokens)
                {
                    yield return token;
                }
            }
            for(int i = 0; i < eater.OpenIndents; ++i)
            {
                yield return new Token() { Kind = TokenKind.EndGroup, Line = lineNum };
            }

            yield return new Token() { Kind = TokenKind.EOF, Line = lineNum };
        }
    }
}
