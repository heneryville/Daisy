namespace Ancestry.Daisy.Language.AST.Trace
{
    public class OrOperatorTrace : TraceNode, IOrOperatorNode<TraceNode>
    {
        public TraceNode Left { get; private set; }

        public TraceNode Right { get; private set; }

        public OrOperatorTrace(TraceNode left, TraceNode right,object scope, bool outcome): base(scope,outcome)
        {
            Left = left;
            Right = right;
        }
    }
}
