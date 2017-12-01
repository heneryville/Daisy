using System.Collections.Generic;
using Ancestry.Daisy.Statements;

namespace Ancestry.Daisy.Language.AST.Trace
{
    public class StatementTrace : TraceNode, IStatementNode
    {
        public string Text { get; private set; }
        public List<string> Tracings { get; set; }

        public ILinkedStatement LinkedStatement { get; set; }

        public StatementTrace(string statement, List<string> tracings, object scope, bool outcome) : base(scope, outcome)
        {
            Text = statement;
            Tracings = tracings;
        }
    }
}
