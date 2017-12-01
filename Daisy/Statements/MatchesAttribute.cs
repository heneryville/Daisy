namespace Ancestry.Daisy.Statements
{
    using System;
    using System.Text.RegularExpressions;

    [AttributeUsage(AttributeTargets.Method)]
    public class MatchesAttribute : Attribute
    {
        public MatchesAttribute(string regexp)
        {
            if (!regexp.EndsWith("$")) regexp = regexp + "$";
            if (!regexp.StartsWith("^")) regexp = "^" + regexp;
            RegularExpression = new Regex(regexp,RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);
        }

        public Regex RegularExpression { get; private set; }
    }
}
