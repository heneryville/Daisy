using System;

namespace Ancestry.Daisy.Statements
{
    public class StatementParameter
    {
        public Type Type { get; set; }

        public string Name { get; set; }
    }

    public class ProceedParameter : StatementParameter
    {
        public Type TransformsTo { get; set; }
    }
}
