namespace Ancestry.Daisy.Program
{
    using Ancestry.Daisy.Language;

    public interface IDaisyExecution
    {
        bool Outcome { get; }
        ExecutionDebugInfo DebugInfo { get; }
        ContextBundle Attachments { get; }
    }

    public class DaisyExecution : IDaisyExecution
    {
        public bool Outcome { get; internal set; }

        public ExecutionDebugInfo DebugInfo { get; private set; }

        public ContextBundle Attachments { get; private set; }

        internal DaisyExecution(DaisyAst ast, DaisyMode mode)
        {
            Attachments = new ContextBundle();
            DebugInfo = new ExecutionDebugInfo(ast,mode);
        }
    }
}
