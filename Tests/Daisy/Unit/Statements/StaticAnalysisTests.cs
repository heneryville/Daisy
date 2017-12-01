namespace Ancestry.Daisy.Tests.Daisy.Unit.Statements
{
    using System;

    using Ancestry.Daisy.Statements;

    using NUnit.Framework;

    /// <summary>
    /// 
    /// </summary>
    [TestFixture,Category("Unit")]
    public class StaticAnalysisTests
    {
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
        }

        /// <summary>
        /// 
        /// </summary>
        public class MyStatement2 : StatementSetTests.MyStatement
        {
            /// <summary>
            /// R2s this instance.
            /// </summary>
            /// <returns></returns>
            public bool R2() { return true; }
        }

        /// <summary>
        /// Its the determines if something is a statement controller.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        [TestCase(typeof(StatementController<int>), Result = true)]
        [TestCase(typeof(MyStatement), Result = true)]
        [TestCase(typeof(MyStatement2), Result = true)]
        [TestCase(typeof(string), Result = false)]
        public bool ItDeterminesIfSomethingIsAStatementController(Type type)
        {
            return StaticAnalysis.IsStatementController(type);
        }

        /// <summary>
        /// 
        /// </summary>
        public class MyStatement3 : StatementController<string>
        {
            /// <summary>
            /// R1s the specified proceed.
            /// </summary>
            /// <param name="proceed">The proceed.</param>
            /// <returns></returns>
            public bool R1(Func<int, bool> proceed) { return true; }
            /// <summary>
            /// R2s the specified proceed.
            /// </summary>
            /// <param name="proceed">The proceed.</param>
            /// <returns></returns>
            public bool R2(Func<int, string> proceed) { return true; }
            /// <summary>
            /// R3s this instance.
            /// </summary>
            /// <returns></returns>
            public bool R3() { return true; }
            /// <summary>
            /// R4s the specified i.
            /// </summary>
            /// <param name="i">The i.</param>
            /// <returns></returns>
            public bool R4(int i) { return true; }
            private bool R5(Func<int, bool> proceed) { return true; }
        }

        /// <summary>
        /// Its the determines if something is an aggregate statement.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <returns></returns>
        [TestCase(typeof(MyStatement3), "R3", Result = false, TestName = "Must have parameters")]
        [TestCase(typeof(MyStatement3), "R4", Result = false, TestName = "Must have function")]
        [TestCase(typeof(MyStatement3), "R2", Result = false, TestName = "Func must return bool")]
        [TestCase(typeof(MyStatement3), "R1", Result = true)]
        public bool ItDeterminesIfSomethingIsAnAggregateStatement(Type type, string methodName)
        {
            var method = type.GetMethod(methodName);
            Assert.IsNotNull(method);
            return StaticAnalysis.IsAggregateMethod(method);
        }

        /// <summary>
        /// Its the determines if something is a proceed function.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        [TestCase(typeof(int), Result = false, TestName = "Must be a func")]
        [TestCase(typeof(Tuple<int, bool>), Result = false, TestName = "Must be a func")]
        [TestCase(typeof(Func<string, string>), Result = false, TestName = "Must return bool")]
        [TestCase(typeof(Func<string, int, bool>), Result = false, TestName = "Must accept only one argument")]
        [TestCase(typeof(Func<string, bool>), Result = true)]
        public bool ItDeterminesIfSomethingIsAProceedFunction(Type type)
        {
            return StaticAnalysis.IsProceedFunction(type);
        }

        private int didIt = 0;
        /// <summary>
        /// Its the converts predicate types.
        /// </summary>
        [Test]
        public void ItConvertsPredicateTypes()
        {
            var method = GetType().GetMethod("DoIt");
            Func<object,bool> cont = j => (int)j % 2 == 0;
            var converter = StaticAnalysis.CreateConverter(typeof(int));
            method.Invoke(this, new [] {converter(cont) });
            Assert.AreEqual(1, didIt);
        }

        /// <summary>
        /// Does it.
        /// </summary>
        /// <param name="isEven">The is even.</param>
        public void DoIt(Func<int,bool> isEven)
        {
            Assert.False(isEven(1));
            Assert.True(isEven(2));
            didIt++;
        }
    }
}
