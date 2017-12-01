namespace Ancestry.Daisy.Language.AST
{
    public interface IAndOperatorNode<T> : IDaisyAstNode where T : IDaisyAstNode
    {
        T Left { get; }
        T Right { get; }
    }

    public class AndOperatorNode : IAndOperatorNode<IDaisyAstNode>
    {
        public IDaisyAstNode Left { get; private set; }

        public IDaisyAstNode Right { get; private set; }

        public AndOperatorNode(IDaisyAstNode left, IDaisyAstNode right)
        {
            Left = left;
            Right = right;
        }
    }
}
