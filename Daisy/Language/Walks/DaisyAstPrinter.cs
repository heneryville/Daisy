namespace Ancestry.Daisy.Language.Walks
{
    using System;
    using System.Text;

    using Ancestry.Daisy.Language.AST;

    public class DaisyAstPrinter : AstTreeWalker<IDaisyAstNode>
    {
        public DaisyAstPrinter(IDaisyAstNode node) : base(node) { }

        private StringBuilder sb = new StringBuilder();
        internal int indent = 0;

        public static String Print(IDaisyAstNode root)
        {
            return new DaisyAstPrinter(root).Print();
        }

        public string Print()
        {
            Walk();
            return sb.ToString();
        }

        protected override bool PreVisit(IAndOperatorNode<IDaisyAstNode> node)
        {
            Pad(sb, indent); sb.Append("AND\r\n");
            indent++;
            return true;
        }

        protected override void PostVisit(IAndOperatorNode<IDaisyAstNode> node)
        {
            indent--;
        }

        protected override bool PreVisit(IOrOperatorNode<IDaisyAstNode> node)
        {
            Pad(sb, indent); sb.Append("OR\r\n");
            indent++;
            return true;
        }

        protected override void PostVisit(IOrOperatorNode<IDaisyAstNode> node)
        {
            indent--;
        }

        protected override bool PreVisit(INotOperatorNode<IDaisyAstNode> node)
        {
            Pad(sb, indent); sb.Append("NOT\r\n");
            indent++;
            return true;
        }

        protected override void PostVisit(INotOperatorNode<IDaisyAstNode> node)
        {
            indent--;
        }

        protected override void Visit(IStatementNode node)
        {
            Pad(sb, indent);
            sb.Append(node.Text);
            sb.Append("\r\n");
        }

        protected override bool PreVisit(IGroupOperatorNode<IDaisyAstNode> node)
        {
            Pad(sb, indent);
            sb.Append("GROUP");
            if(!string.IsNullOrEmpty(node.Text))
            {
                sb.Append("@");
                sb.Append(node.Text);
            }
            sb.Append("\r\n");
            indent++;
            return true;
        }

        protected override void PostVisit(IGroupOperatorNode<IDaisyAstNode> node)
        {
            indent--;
        }

        private static void Pad(StringBuilder sb, int pads)
        {
            for(int i=0; i<pads; ++i)
            {
                sb.Append("-");
            }
        }
    }
}
