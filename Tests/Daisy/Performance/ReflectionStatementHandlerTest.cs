using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ancestry.Daisy.Program;

namespace Ancestry.Daisy.Tests.Daisy.Performance
{
    using System.Diagnostics;

    using Ancestry.Daisy.Statements;

    using NUnit.Framework;

    /// <summary>
    /// 
    /// </summary>
    [TestFixture,Category("Performance")]
    public class ReflectionStatementHandlerTest
    {
        private class MyController : StatementController<string>
        {
            public bool MyStatement() { return true; }
        }

        /// <summary>
        /// Its the initializes controller quickly.
        /// </summary>
        [Test]
        [Ignore]
        public void ItInitializesControllerQuickly()
        {
            var type = typeof(MyController);
            var method = type.GetMethod("MyStatement");
            var refHandler = (ReflectionStatementDefinition.ReflectionLinkedStatement)new ReflectionStatementDefinition(method, type)
                .Link("MyStatement");
            var context = new InvokationContext() {
                Scope = "The Scope",
                Context = new ContextBundle(),
                Attachments = new ContextBundle(),
            };
            const int iterations = 1000000;

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < iterations; ++i)
            {
                refHandler.InitializeController(refHandler.CreateController(), context);
            }
            stopwatch.Stop();
            Console.WriteLine("Elapsed: " + stopwatch.ElapsedMilliseconds);
            Console.WriteLine("Per initialization: " + ((double)stopwatch.ElapsedMilliseconds)/iterations);
            Assert.Less(stopwatch.ElapsedMilliseconds, 500);
        }

        /// <summary>
        /// Its the invoked methods quickly.
        /// </summary>
        [Test]
        public void ItInvokedMethodsQuickly()
        {
            var type = typeof(MyController);
            var method = type.GetMethod("MyStatement");
            var refHandler = (ReflectionStatementDefinition.ReflectionLinkedStatement)new ReflectionStatementDefinition(method, type)
                .Link("My Statement");
            var inst = new MyController() {
                Scope = "The Scope",
                Context = new ContextBundle(),
                Attachments = new ContextBundle()
            };
            const int iterations = 1000000;
            var parameters = new object[0];

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < iterations; ++i)
            {
                refHandler.Execute(inst, parameters);
            }
            stopwatch.Stop();
            Console.WriteLine("Elapsed: " + stopwatch.ElapsedMilliseconds);
            Console.WriteLine("Per invokation: " + ((double)stopwatch.ElapsedMilliseconds)/iterations);
            Assert.Less(stopwatch.ElapsedMilliseconds, 200);
        }
    }
}
