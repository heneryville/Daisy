namespace Ancestry.Daisy.Language.AST
{
    public interface IOrOperatorNode<T> : IDaisyAstNode where T:IDaisyAstNode
    {
        T Left { get; }
        T Right { get; }
    }

    public class OrOperatorNode : IOrOperatorNode<IDaisyAstNode>
    {
        public IDaisyAstNode Left { get; private set; }

        public IDaisyAstNode Right { get; private set; }

        public OrOperatorNode(IDaisyAstNode left, IDaisyAstNode right)
        {
            Left = left;
            Right = right;
        }
    }
}
