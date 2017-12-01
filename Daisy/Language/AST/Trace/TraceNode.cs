namespace Ancestry.Daisy.Language.AST.Trace
{
    public class TraceNode : IDaisyAstNode
    {
        protected TraceNode(object scope,bool outcome)
        {
            Outcome = outcome;
            Scope = scope;
        }

        public bool Outcome { get; private set; }

        public object Scope { get; private set; }
    }
}
