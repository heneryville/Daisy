namespace Ancestry.Daisy.Language
{
    /// <summary>
    /// The WhitespaceEater is in charge of counting and managing whitespace
    /// at the begining of lines.
    ///   It handles:
    ///     Consistency of types of whitespace (tabs vs spaces)
    ///     Consistency of number of whitespaces (# of spaces being counted as an indent)
    ///     The number of indents on each line
    ///     The change in the number of idents from the last line
    ///     Counts the number of indents open at a time
    /// </summary>
    public class WhitespaceEater
    {
        private int openIndents;
        private int spacesPerGroup;
        private char whitespaceType = 'a';

        public int OpenIndents { get { return openIndents; } }

        internal static TrimResult TrimLeadingSpaces(string line, int lineCnt)
        {
            var whitespaceType = 'a';
            var cursor = 0;
            for(;cursor<line.Length; ++cursor)
            {
                var c = line[cursor];
                if(c == ' ' || c == '\t')
                {
                    if(whitespaceType == 'a')
                    {
                        whitespaceType = c;
                    }
                    else if(c != whitespaceType)
                    {
                        throw new InconsistentWhitespaceException(string.Format("Line {0} has mixed use of leading tabs and spaces",lineCnt));
                    }
                }
                else
                {
                    break;
                }
            }
            return new TrimResult() {
                    Line = line.Substring(cursor),
                    Trimmed = cursor,
                    WhitespaceType = whitespaceType
                };
        }

        public struct TrimResult
        {
            public char WhitespaceType { get; set; }
            public string Line { get; set; }
            public int Trimmed { get; set; }
        }

        private int Indents(int spaces, int line)
        {
            spacesPerGroup = spacesPerGroup == 0 ? spaces : spacesPerGroup;
            if(spacesPerGroup != 0)
            {
                if (spaces % spacesPerGroup != 0) 
                    throw new InconsistentWhitespaceException(string.Format(
                        "Expected {0} spaces per indent, but got {1} on line {2}, which is not divisible by {0}",
                        spacesPerGroup, spaces, line));
                return spaces / spacesPerGroup;
            }
            return 0;
        }

        public WhitespaceMorsel Eat(string line, int lineCnt)
        {
            var trim = TrimLeadingSpaces(line, lineCnt);
            if(whitespaceType == 'a')
            {
                whitespaceType = trim.WhitespaceType;
            }
            else if (whitespaceType != trim.WhitespaceType && trim.WhitespaceType != 'a')
            {
                throw new InconsistentWhitespaceException(string.Format("Some lines start with spaces, and use tabs"));
            }
            var indents = Indents(trim.Trimmed, lineCnt);
            var back = new WhitespaceMorsel() {
                Line = trim.Line,
                Indents = indents,
                DeltaIndents = indents - openIndents
            };
            openIndents = indents;
            return back;
        }

        public class WhitespaceMorsel
        {
            /// <summary>
            /// The number of indents this line has
            /// </summary>
            public int Indents { get; set; }

            public int DeltaIndents { get; set; }

            /// <summary>
            /// A string representing the line with the whitespace trimmed from the front.
            /// </summary>
            public string Line { get; set; }
        }
    }
}
