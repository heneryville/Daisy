using Ancestry.Daisy.Program;

namespace Ancestry.Daisy.Statements
{
    public class StatementController<T> 
    {
        public T Scope { get; set; }
        public ContextBundle Context { get; set; }
        public ContextBundle Attachments { get; set; }
        public ITracer Tracer { get; set; }

        public void Trace(string pattern, params object[] templateValues)
        {
            Tracer.Trace(pattern,templateValues);
        }
    }
}
