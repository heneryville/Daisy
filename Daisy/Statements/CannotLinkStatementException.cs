namespace Ancestry.Daisy.Statements
{
    using System;
    using System.Reflection;

    public class CannotLinkStatementException : Exception
    {

        public CannotLinkStatementException(MethodInfo method,string message) : base(message)
        {
            StatementName = method.Name;
        }

        public object Scope { get; set; }

        public string Statement { get; set; }

        public string StatementName { get; set; }
    }
}