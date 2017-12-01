namespace Ancestry.Daisy.Tests.Daisy.Unit.Statements
{
    using System;
    using System.Linq;

    using Ancestry.Daisy.Statements;

    using NUnit.Framework;

    /// <summary>
    /// 
    /// </summary>
    [TestFixture,Category("Unit")]
    public class StatementSetTests
    {
        /// <summary>
        /// Its the adds from assembly.
        /// </summary>
        [Test]
        public void ItAddsFromAssembly()
        {
            var load = new StatementSet();
            load.FromAssemblyOf(typeof(StatementSetTests));
            Assert.That(load.Statements.OfType<ReflectionStatementDefinition>().Any(x => x.Name == "R1"));
        }

        /// <summary>
        /// Its the type of the adds from.
        /// </summary>
        [Test]
        public void ItAddsFromType()
        {
            var load = new StatementSet();
            load.FromController(typeof(MyStatement));
            Assert.That(load.Statements.OfType<ReflectionStatementDefinition>().Any(x => x.Name == "R1"));
        }

        /// <summary>
        /// 
        /// </summary>
        public class MyStatement : StatementController<string>
        {
            /// <summary>
            /// R1s this instance.
            /// </summary>
            /// <returns></returns>
            public bool R1() { return true; }
            /// <summary>
            /// R2s the specified proceed.
            /// </summary>
            /// <param name="proceed">The proceed.</param>
            /// <returns></returns>
            public bool R2(Func<int,bool> proceed) { return true; }
        }
    }
}
