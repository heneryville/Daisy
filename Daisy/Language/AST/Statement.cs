namespace Ancestry.Daisy.Language.AST
{
    using Ancestry.Daisy.Statements;

    public interface IStatementNode : IDaisyAstNode
    {
        string Text { get; }
        ILinkedStatement LinkedStatement { get; }
    }

    public class StatementNode : IStatementNode
    {
        public string Text { get; private set; }

        public ILinkedStatement LinkedStatement { get; set; }

        public StatementNode(string statement)
        {
            Text = statement;
        }
    }
}
