using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Tests.Daisy.Performance
{
    using System.Diagnostics;

    using Ancestry.Daisy.Statements;
    using Ancestry.Daisy.Tests.Daisy.Component.Controllers;
    using Ancestry.Daisy.Tests.Daisy.Component.Domain;

    using NUnit.Framework;

    /// <summary>
    /// 
    /// </summary>
    [TestFixture,Category("Performance")]
    public class ExecutionPerformanceTest
    {
        /// <summary>
        /// Its the executes performantly.
        /// </summary>
        [Test]
        public void ItExecutesPerformantly()
        {
            var code = Component.Statements.UserHasNoRecentTransactions;
            var statements = new StatementSet().FromAssemblyOf<UserController>();
            var pgrm = DaisyCompiler.Compile<User>(code, statements);

            var stopwatch = new Stopwatch();
            var iterations = 50000;
            stopwatch.Start();

            for(int i=0; i<iterations; ++i)
            {
                pgrm.Execute(Component.TestData.Ben);
            }

            stopwatch.Stop();
            Console.WriteLine("Elapsed: " + stopwatch.ElapsedMilliseconds);
            Console.WriteLine("Per execution: " + ((double)stopwatch.ElapsedMilliseconds)/iterations);
            Assert.Less(stopwatch.ElapsedMilliseconds, 30000);
        }
    }
}
