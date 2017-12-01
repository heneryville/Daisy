using System;
using System.Collections.Generic;
using System.Linq;

namespace Ancestry.Daisy.Linking
{
    using Ancestry.Daisy.Statements;

    public abstract class LinkingError
    {
        public string Statement { get; set; }

        public Type ScopeType { get; set; }
    }

    public class NoLinksFoundError : LinkingError
    {
        public NoLinksFoundError(string statement, Type scopeType)
        {
            Statement = statement;
            ScopeType = scopeType;
        }

        public override string ToString()
        {
            return string.Format("No statements linked '{0}'", Statement);
        }
    }

    public class NoLinksPermittedError : LinkingError
    {
        public IStatementDefinition Definition { get; set; }

        public NoLinksPermittedError(string statement, Type scopeType, IStatementDefinition definition)
        {
            Definition = definition;
            Statement = statement;
            ScopeType = scopeType;
        }

        public override string ToString()
        {
            return string.Format("Statement {0} does not allow any subrules, but some were provided", Statement);
        }
    }

    public class MultipleLinksFoundError : LinkingError
    {
        public IList<IStatementDefinition> MatchedStatements { get; set; }

        public MultipleLinksFoundError(string statement, Type scopeType, IList<IStatementDefinition> matchedStatements)
        {
            MatchedStatements = matchedStatements;
            Statement = statement;
            ScopeType = scopeType;
        }

        public override string ToString()
        {
            return string.Format("Multiple statements linked to '{0}'. They are: {1}", Statement,
                string.Join(",",MatchedStatements.Select(x => x.Name)));
        }
    }
}
