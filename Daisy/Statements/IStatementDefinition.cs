namespace Ancestry.Daisy.Statements
{
    using System;

    public interface IStatementDefinition
    {
        Type ScopeType { get; }
        string Name { get; }
        Type TransformsScopeTo { get; }
        ILinkedStatement Link(string statement);
    }

    public interface ILinkedStatement
    {
        IStatementDefinition Definition { get; }
        bool Execute(InvokationContext context);
    }
}
