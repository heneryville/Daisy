using System;
using Ancestry.Daisy.Language.AST.Trace;

namespace Ancestry.Daisy.Program
{
    using Ancestry.Daisy.Language;
    using Ancestry.Daisy.Language.Walks;

    public class ExecutionDebugInfo
    {
        private readonly DaisyMode _mode;
        private IPerformanceCounter _performanceCounter;
        public DaisyAst Ast { get; private set; }

        public ExecutionDebugInfo(DaisyAst ast, DaisyMode mode)
        {
            _mode = mode;
            Ast = ast;
            _performanceCounter = mode == DaisyMode.Debug
                ? new PerformanceCounter()
                : NoopPerformanceCounter.Instance;
            measurements = new Lazy<PerformanceMeasurments>(() => PerformanceCounter.Measurments);
        }

        /// <summary>
        /// Essentially a textual representation of the Trace
        /// </summary>
        public string DebugView
        {
            get
            {
                return new DaisyTracePrinter(Trace).Print();
            }
        }

        /// <summary>
        /// Tracing the execution path is held in a TraceNode, which mimics the AST of the 
        /// general program, but with tracing information about how the execution occured.
        /// </summary>
        public TraceNode Trace { get; internal set; }

        internal IPerformanceCounter PerformanceCounter { get { return _performanceCounter; } }

        private Lazy<PerformanceMeasurments> measurements;

        /// <summary>
        /// Data on how much effort Daisy is going through to execute. Available only in Debug mode.
        /// </summary>
        public PerformanceMeasurments Measurments { get {  return measurements.Value; } }
    }
}
