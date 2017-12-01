using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ancestry.Daisy.Program;
using NUnit.Framework;

namespace Ancestry.Daisy.Tests.Daisy.Unit.Program
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture,Category("Unit")]
    public class PerformanceMeasurementsTest
    {
        /// <summary>
        /// Its the aggregates performance measurements.
        /// </summary>
        [Test]
        public void ItAggregatesPerformanceMeasurements()
        {
            var agg = new AggregatePerformanceMeasurments();
            Assert.AreEqual(0, agg.Executions);

            var agg2 = agg.Aggregate(new PerformanceMeasurments() {
                ExecutionTime = 2,
                OpsExecuted = 3,
                StatementsExecutedByScope = new Dictionary<Type, int>()
                {
                    {typeof(int), 2},
                    {typeof(string), 6}
                }
            });

            Assert.AreEqual(2, agg2.ExecutionTime);
            Assert.AreEqual(3, agg2.OpsExecuted);
            Assert.AreEqual(6, agg2.StatementsExecutedByScope[typeof(string)]);
            Assert.AreEqual(1, agg2.Executions);

            var agg3 = agg2.Aggregate(new PerformanceMeasurments() {
                ExecutionTime = 2,
                OpsExecuted = 3,
                StatementsExecutedByScope = new Dictionary<Type, int>()
                {
                    {typeof(int), 2},
                    {typeof(string), 6}
                }
            });

            Assert.AreEqual(4, agg3.ExecutionTime);
            Assert.AreEqual(6, agg3.OpsExecuted);
            Assert.AreEqual(12, agg3.StatementsExecutedByScope[typeof(string)]);
            Assert.AreEqual(2, agg3.Executions);
        }
    }
}
