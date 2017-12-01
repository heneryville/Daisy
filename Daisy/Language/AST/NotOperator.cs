namespace Ancestry.Daisy.Language.AST
{
    public interface INotOperatorNode<T> : IDaisyAstNode where T:IDaisyAstNode
    {
        T Inner { get; }
    }

    public class NotOperatorNode : INotOperatorNode<IDaisyAstNode>
    {
        public IDaisyAstNode Inner { get; private set; }

        public NotOperatorNode(IDaisyAstNode inner)
        {
            Inner = inner;
        }
    }
}
