using Ancestry.Daisy.Language.AST.Trace;

namespace Ancestry.Daisy.Language.Walks
{
    using System.Text;
    using Ancestry.Daisy.Language.AST;

    public class DaisyTracePrinter : AstTreeWalker<TraceNode>
    {
        public DaisyTracePrinter(TraceNode root) : base(root)
        {
        }

        private StringBuilder sb;
        internal int indent = 0;

        private bool isFirstStatementInGroup;

        public string Print(int initialIndent = 0)
        {
            indent = initialIndent;
            sb = new StringBuilder();
            isFirstStatementInGroup = true;
            Walk();
            return sb.ToString();
        }

        protected override void InfixVisit(IAndOperatorNode<TraceNode> node)
        {
            var trace = (AndOperatorTrace) node;
            Pad(sb, indent); sb.Append("AND (").Append(trace.Outcome).Append(")");
            if (trace.Right == null) sb.AppendLine("<Skipped>");
            else sb.Append(" ");
        }

        protected override void InfixVisit(IOrOperatorNode<TraceNode> node)
        {
            var trace = (OrOperatorTrace) node;
            Pad(sb, indent); sb.Append("OR ").Append(trace.Outcome).Append(") ");
            if (trace.Right == null) sb.AppendLine("<Skipped>");
            else sb.Append(" ");
        }

        protected override bool PreVisit(INotOperatorNode<TraceNode> node)
        {
            var trace = (NotOperatorTrace) node;
            sb.Append("NOT (").Append(trace.Outcome).Append(") ");
            return true;
        }

        protected override void Visit(IStatementNode node)
        {
            var trace = (StatementTrace) node;
            if(isFirstStatementInGroup)
            {
                Pad(sb, indent);
            }
            sb.Append(node.Text).Append("(").Append(trace.Outcome).Append(")");
            foreach (var tracing in trace.Tracings)
            {
                sb.AppendLine();
                Pad(sb,indent);
                sb.Append("!").Append(tracing);
            }
            sb.AppendLine();
            isFirstStatementInGroup = false;
        }

        protected override bool PreVisit(IGroupOperatorNode<TraceNode> node)
        {
            var trace = (GroupOperatorTrace) node;
            if(isFirstStatementInGroup)
            {
                Pad(sb, indent);
            }
            sb.Append("GROUP");
            if (!string.IsNullOrEmpty(node.Text))
            {
                sb.Append("@");
                sb.Append(node.Text);
            }
            sb.Append(" (").Append(trace.Outcome).Append(")");
            foreach (var tracing in trace.Tracings)
            {
                sb.AppendLine();
                Pad(sb,indent);
                sb.Append("!").Append(tracing);
            }
            if (trace.Frames.Count > 0)
            {
                sb.AppendLine();
                foreach (var frame in trace.Frames)
                {
                    Pad(sb, indent + 1);
                    sb.AppendLine("******" + frame.Scope.GetType().Name + " " + frame.Scope + "=>(" + frame.Outcome +
                                  ")*******");
                    sb.Append(new DaisyTracePrinter(frame).Print(indent + 1));
                }
                Pad(sb, indent + 1);
                sb.AppendLine("***********");
            }
            else { sb.AppendLine(); }
            indent++;
            isFirstStatementInGroup = true;
            return false;
        }

        protected override void PostVisit(IGroupOperatorNode<TraceNode> node)
        {
            indent--;
            isFirstStatementInGroup = false;
        }

        private static void Pad(StringBuilder sb, int pads)
        {
            for (int i = 0; i < pads; ++i)
            {
                sb.Append("-");
            }
        }
    }
}
