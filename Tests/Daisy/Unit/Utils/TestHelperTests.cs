namespace Ancestry.Daisy.Tests.Daisy.Unit.Utils
{
    using Ancestry.Daisy.Statements;
    using Ancestry.Daisy.TestHelpers;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using NUnit.Framework;

    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class TestHelperTests
    {
        /// <summary>
        /// 
        /// </summary>
        private class MyStatement1 : StatementController<int>
        {
            [Matches("Good boy")]
            public bool R1()
            {
                Context["didIt"] = true;
                return Scope == 1;
            }
        }

        /// <summary>
        /// Its the invokes functions.
        /// </summary>
        [Test]
        public void ItInvokesFunctions()
        {
            TestHelper.Invoke(typeof(MyStatement1), "R1", 1, "Good boy")
                .AssertMatched()
                .AssertResult(true)
                .AssertContextHas("didIt");
        }

        /// <summary>
        /// Its the asserts match failures.
        /// </summary>
        [Test,NUnit.Framework.ExpectedException(typeof(AssertFailedException))]
        public void ItAssertsMatchFailures()
        {
            TestHelper.Invoke(typeof(MyStatement1), "R1", 1, "Bad boy")
                .AssertMatched()
                ;
        }

        /// <summary>
        /// Its the asserts result wrong.
        /// </summary>
        [Test,NUnit.Framework.ExpectedException(typeof(AssertFailedException))]
        public void ItAssertsResultWrong()
        {
            TestHelper.Invoke(typeof(MyStatement1), "R1", 2, "Bad boy")
                .AssertResult(true)
                ;
        }
    }
}
