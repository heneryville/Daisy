using Ancestry.Daisy.Statements;

namespace Ancestry.Daisy.Language.AST
{
    public interface IGroupOperatorNode<T> : IStatementNode where T : IDaisyAstNode
    {
        T Root { get; }
        new string Text { get; }
        new ILinkedStatement LinkedStatement { get; }
    }

    public class GroupOperatorNode : StatementNode, IGroupOperatorNode<IDaisyAstNode>
    {
        public IDaisyAstNode Root { get; private set; }

        public bool HasCommand { get { return !string.IsNullOrEmpty(Text); } }

        public GroupOperatorNode(string text, IDaisyAstNode root) : base(text)
        {
            Root = root;
        }
    }
}
