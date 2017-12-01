namespace Ancestry.Daisy.Language.AST.Trace
{
    public class AndOperatorTrace : TraceNode, IAndOperatorNode<TraceNode>
    {
        public TraceNode Left { get; private set; }

        public TraceNode Right { get; private set; }

        public AndOperatorTrace(TraceNode left, TraceNode right, object scope, bool outcome) : base(scope,outcome)
        {
            Left = left;
            Right = right;
        }
    }
}
