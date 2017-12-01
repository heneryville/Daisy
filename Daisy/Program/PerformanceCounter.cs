using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Ancestry.Daisy.Statements;

namespace Ancestry.Daisy.Program
{
    public interface IPerformanceCounter
    {
        void Start();
        void End();
        void CountOp();
        PerformanceMeasurments Measurments { get; }
        void Count(InvokationContext context, IStatementDefinition definition);
    }

    public class NoopPerformanceCounter : IPerformanceCounter
    {
        public void Start() { }
        public void End() { }
        public void CountOp() { }
        public PerformanceMeasurments Measurments { get { return singleMeansurements; }}
        public void Count(InvokationContext context, IStatementDefinition definition) { }
        private static PerformanceMeasurments singleMeansurements = new PerformanceMeasurments();
        private static IPerformanceCounter singleCounter = new NoopPerformanceCounter();
        public static IPerformanceCounter Instance { get { return singleCounter; } }
    }

    public class PerformanceCounter : IPerformanceCounter
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private int _opsCounter;
        private readonly List<Tuple<InvokationContext,IStatementDefinition>> _counts = new List<Tuple<InvokationContext, IStatementDefinition>>();

        public void Start() { _stopwatch.Start(); }

        public void End()
        {
            _stopwatch.Stop();
        }

        public void CountOp() { _opsCounter++; }

        public PerformanceMeasurments Measurments
        {
            get
            {
                return new PerformanceMeasurments()
                {
                    StatementsExecutedByScope = _counts.GroupBy(x => x.Item1.Scope.GetType()).ToDictionary(x => x.Key, y => y.Count()),
                    StatementsExecuted = _counts.GroupBy(x => Tuple.Create(x.Item2.ScopeType, x.Item2.Name)).ToDictionary(x => x.Key, x => x.Count()),
                    OpsExecuted = _opsCounter,
                    ExecutionTime = _stopwatch.ElapsedMilliseconds,
                };
            }
        }

        public void Count(InvokationContext context, IStatementDefinition definition)
        {
            _counts.Add(Tuple.Create(context,definition));
        }
    }

    public class PerformanceMeasurments
    {
        private static IDictionary<Type,int> defaultScopes = new Dictionary<Type, int>();
        private static IDictionary<Tuple<Type,string>,int> defaultStatements = new Dictionary<Tuple<Type,string>, int>();
        public PerformanceMeasurments ()
        {
            StatementsExecutedByScope = defaultScopes;
            StatementsExecuted = defaultStatements;
        }

        public IDictionary<Type,int> StatementsExecutedByScope { get; set; }

        public IDictionary<Tuple<Type,string>,int> StatementsExecuted { get; set; }

        public int TotalStatementsExecuted { get { return StatementsExecutedByScope.Sum(x => x.Value); } }

        public int OpsExecuted { get; set; }

        public double ExecutionTime { get; set; }

    }

    public class AggregatePerformanceMeasurments : PerformanceMeasurments
    {
        public int Executions { get; set; }

        public AggregatePerformanceMeasurments Aggregate(PerformanceMeasurments other)
        {
            return new AggregatePerformanceMeasurments()
            {
                ExecutionTime = ExecutionTime + other.ExecutionTime,
                OpsExecuted = OpsExecuted + other.OpsExecuted,
                StatementsExecutedByScope = StatementsExecutedByScope
                                            .Concat(other.StatementsExecutedByScope)
                                            .GroupBy(x => x.Key)
                                            .ToDictionary(x => x.Key, y => y.Sum(z => z.Value)),
                Executions = Executions + ((other is AggregatePerformanceMeasurments) ? ((AggregatePerformanceMeasurments)other).Executions : 1),
                StatementsExecuted = StatementsExecuted
                                            .Concat(other.StatementsExecuted)
                                            .GroupBy(x => x.Key)
                                            .ToDictionary(x => x.Key, x => x.Sum(y=> y.Value))
            };
        }
    }
}
