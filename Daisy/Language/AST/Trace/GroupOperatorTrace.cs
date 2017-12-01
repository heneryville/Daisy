using System.Collections.Generic;

namespace Ancestry.Daisy.Language.AST.Trace
{
    public class GroupOperatorTrace : StatementTrace, IGroupOperatorNode<TraceNode>
    {
        public TraceNode Root { get; private set; }

        public bool HasCommand { get { return !string.IsNullOrEmpty(Text); } }

        public List<TraceNode> Frames { get; private set; }

        public GroupOperatorTrace(string text,List<string> tracings, List<TraceNode> frames, object scope, bool outcome) : base(text,tracings, scope, outcome)
        {
            Root = null;
            Frames = frames;
        }
    }
}
