namespace Ancestry.Daisy.Utils
{
    using System.Collections.Generic;
    using System.Text;
    using System.IO;

    internal static class StringExtentions
    {
        public static Stream ToStream(this string str)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(str));
        }
        
        public static IEnumerable<string> SplitNameParts(string str)
        {
            var sb = new StringBuilder();
            foreach (var c in str)
            {
                if( (char.IsUpper(c)
                      || c == '_')
                    && sb.Length > 0
                    )
                {
                    yield return sb.ToString();
                    sb = new StringBuilder();
                }
                if(c != '_') sb.Append(c);
            }
            if(sb.Length > 0)
            {
                yield return sb.ToString();
            }
        }
    }
}
