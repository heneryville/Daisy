namespace Ancestry.Daisy.Documentation.Utils
{
    using System.Text.RegularExpressions;

    internal static class StringExtentions
    {
        public static string ConsolidateWhitespace(this string input)
        {
            return Regex.Replace(input.Trim(),"\\s+"," ");
        }
    }
}
