namespace Ancestry.Daisy.Linking
{
    using System;
    using System.Text.RegularExpressions;

    using Ancestry.Daisy.Statements;

    public class LinkedDaisyStatement 
    {
        public Match Match { get; set; }

        public string Statement { get; set; }

        public Type ScopeType { get; set; }

        public IStatementDefinition Definition { get; set; }
    }
}