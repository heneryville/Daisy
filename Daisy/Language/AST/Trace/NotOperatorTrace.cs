namespace Ancestry.Daisy.Language.AST.Trace
{
    public class NotOperatorTrace: TraceNode, INotOperatorNode<TraceNode>
    {
        public TraceNode Inner { get; private set; }

        public NotOperatorTrace(TraceNode inner,object scope, bool outcome) : base(scope,outcome)
        {
            Inner = inner;
        }
    }
}
