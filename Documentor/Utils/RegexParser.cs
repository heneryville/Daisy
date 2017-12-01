namespace Ancestry.Daisy.Documentation.Utils
{
    internal class RegexParser
    {
        public static TextSpan? FirstGroup(string expression)
        {
            var start = -1;
            var end = -1;
            bool isEscapedCharacter = false;
            var nestingLevel = 0;
            for (int i = 0; i < expression.Length; ++i)
            {
                if (expression[i] == '\\' && !isEscapedCharacter)
                {
                    isEscapedCharacter = true;
                    continue;
                }
                if (isEscapedCharacter)
                {
                    isEscapedCharacter = false;
                    continue;
                }

                if (expression[i] == '(')
                {
                    if (nestingLevel == 0) start = i;
                    nestingLevel++;
                }
                else if (expression[i] == ')')
                {
                    nestingLevel--;
                    if (nestingLevel == 0)
                    {
                        end = i;
                        break;
                    }
                }
                isEscapedCharacter = false;
            }
            if(start < 0 || end < 0) return null;
            return new TextSpan() {
                Offset = start,
                Length = end - start + 1
            };
        }

        public struct TextSpan
        {
            public int Offset { get; set; }
            public int Length { get; set; }
            public int End { get { return Offset + Length; } }
        }

        public static string Replace(string expression, TextSpan span, string with)
        {
            return expression.Substring(0, span.Offset) + with + expression.Substring(span.Offset + span.Length);
        }
    }
}
