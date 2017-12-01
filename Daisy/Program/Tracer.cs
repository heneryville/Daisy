using System.Collections.Generic;

namespace Ancestry.Daisy.Program
{
    public interface ITracer
    {
        void Trace(string pattern, params object[] templateValues);
        List<string> Tracings { get; }
    }

    public class Tracer : ITracer
    {
        public Tracer()
        {
            Tracings = new List<string>();
        }

        public void Trace(string pattern, params object[] templateValues)
        {
            var tracing = string.Format(pattern, templateValues);
            Tracings.Add(tracing);
        }

        public List<string> Tracings { get; private set; }
    }

    public class NoOpTracer : ITracer
    {
        private static List<string> tracings = new List<string>();
        public NoOpTracer() { }

        public void Trace(string pattern, params object[] templateValues) { return; }

        public List<string> Tracings { get { return tracings; } }
    }
}
