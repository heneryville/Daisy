using Ancestry.Daisy.Program;

namespace Ancestry.Daisy.Tests.Daisy.Unit.Statements
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Ancestry.Daisy.Statements;
    using Ancestry.Daisy.Utils;

    using NUnit.Framework;

    /// <summary>
    /// 
    /// </summary>
    [TestFixture,Category("Unit")]
    public class ReflectionStatementHandlerTests
    {
        /// <summary>
        /// Its the normalizes method names.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        [TestCase("IsResidence",Result = "Is\\s+Residence")]
        public string ItNormalizesMethodNames(string name)
        {
            return ReflectionStatementDefinition.NormalizeMethodName(name);
        }

        /// <summary>
        /// 
        /// </summary>
        private class TestStatements : StatementController<int>
        {
            /// <summary>
            /// R1s this instance.
            /// </summary>
            /// <returns></returns>
            public bool R1()
            {
                return true;
            }

            /// <summary>
            /// R2s this instance.
            /// </summary>
            /// <returns></returns>
            public bool R2()
            {
                return Scope == 9;
            }

            /// <summary>
            /// R3s the specified haz.
            /// </summary>
            /// <param name="haz">The haz.</param>
            /// <returns></returns>
            [Matches("I haz (\\d+) cheeseburgers")]
            public bool R3(int haz)
            {
                return haz == Scope;
            }

            /// <summary>
            /// R4s the specified haz.
            /// </summary>
            /// <param name="haz">The haz.</param>
            /// <returns></returns>
            [Matches("I haz (\\d+) cheeseburgers")]
            public bool R4(DateTime haz)
            {
                return false;
            }

            /// <summary>
            /// R5s this instance.
            /// </summary>
            /// <returns></returns>
            [Matches("I haz (\\d+) cheeseburgers")]
            public bool R5()
            {
                return false;
            }

            /// <summary>
            /// R6s this instance.
            /// </summary>
            /// <returns></returns>
            public bool R6()
            {
                return Context != null;
            }

            /// <summary>
            /// R7s the specified value.
            /// </summary>
            /// <param name="value">The value.</param>
            /// <returns></returns>
            [Matches(@"I haz (an a+wesome )?cheeseburgers")]
            public bool R7(string value)
            {
                return value == null;
            }

            /// <summary>
            /// R8s this instance.
            /// </summary>
            /// <returns></returns>
            public bool R8()
            {
                Attachments["controller"] = this;
                return true;
            }

        }

        /// <summary>
        /// Its the matches statements.
        /// </summary>
        /// <param name="statment">The statment.</param>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        [TestCase("R1","R1",Result = true)]
        [TestCase("R1","R1AndSomeMore",Result = false)]
        [TestCase("R1","R2",Result = false)]
        public bool ItMatchesStatements(string statment, string statement)
        {
            var m = GetMethod(statment);
            var load = new ReflectionStatementDefinition(m, typeof(TestStatements));
            var match = load.Matches(new MatchingContext() {
                    Statement = statement
                });
            return match.Let(x => x.Success,false);
        }

        /// <summary>
        /// Its the executes statements.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <param name="rawStatement">The raw statement.</param>
        /// <param name="scope">The scope.</param>
        /// <returns></returns>
        [TestCase("R1","R1",1,Result = true, TestName = "It returns result of statment")]
        [TestCase("R2","R2",9,Result = true, TestName = "It sets scope")]
        [TestCase("R2","R2",8,Result = false, TestName = "It sets scope. Inverse")]
        [TestCase("R3","I haz 2 cheeseburgers",2,Result = true, TestName = "It sets parameter values")]
        [TestCase("R4","I haz 2 cheeseburgers",2,ExpectedException = typeof(CannotLinkStatementException), TestName = "It errors when cannot make parameter")]
        [TestCase("R5","I haz 2 cheeseburgers",2,ExpectedException = typeof(CannotLinkStatementException), TestName = "It errors when cannot make parameter")]
        [TestCase("R6","R6",9,Result = true, TestName = "It sets context")]
        [TestCase("R7","I haz cheeseburgers",1,Result = true, TestName = "It injects null for non-captured groups")]
        public bool ItExecutesStatements(string statement, string rawStatement, int scope)
        {
            var load = new ReflectionStatementDefinition(GetMethod(statement), typeof(TestStatements));
            var linked = load.Link(rawStatement);
            Assert.IsNotNull(linked);
            return linked.Execute(new InvokationContext() {
                    Scope = scope,
                    Context = new ContextBundle(),
                });
        }

        /// <summary>
        /// Its the does not re use controllers.
        /// </summary>
        [Test]
        public void ItDoesNotReUseControllers()
        {
            var load = new ReflectionStatementDefinition(GetMethod("R8"), typeof (TestStatements));
            var linked = load.Link("R8");
            var att1 = new ContextBundle();
            var att2 = new ContextBundle();

            linked.Execute(new InvokationContext() {
                Scope = 9,
                Attachments = att1
            });

            linked.Execute(new InvokationContext() {
                Scope = 9,
                Attachments = att2
            });

            var ctr1 = att1.Get<TestStatements>("controller");
            var ctr2 = att2.Get<TestStatements>("controller");

            Assert.AreNotSame(ctr1,ctr2);
        }

        /// <summary>
        /// Its the executes aggregates.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <param name="rawStatement">The raw statement.</param>
        /// <param name="strScope">The string scope.</param>
        /// <param name="expectedCalls">The expected calls.</param>
        /// <returns></returns>
        [TestCase("R1", "R1", "1,2,3,4", 2, Result = true, TestName = "It returns result of statment")]
        [TestCase("R1", "R1", "1,3,5", 3, Result = false, TestName = "It sets scope. Inverse")]
        [TestCase("R2", "I haz 10 cheeseburgers", "1,2,3",2 , Result = true, TestName = "It sets parameter values")]
        [TestCase("R3", "I haz 10 cheeseburgers", "1", 1, ExpectedException = typeof(CannotLinkStatementException), TestName = "It errors when cannot make parameter")] //too few
        [TestCase("R4", "I haz 10 cheeseburgers", "1", 1, ExpectedException = typeof(CannotLinkStatementException), TestName = "It errors when cannot make parameter")] //too many
        public bool ItExecutesAggregates(string statement, string rawStatement, string strScope, int expectedCalls)
        {
            var scope = strScope.Split(',').Select(int.Parse);
            var load = new ReflectionStatementDefinition(GetAggregateMethod(statement), typeof(TestAggregates));
            var link = load.Link(rawStatement);
            Assert.IsNotNull(link);
            var calls = 0;
            var result = link.Execute(new InvokationContext() {
                Scope = scope,
                Proceed =  o =>
                {
                    calls++;
                    return ((int)o) % 2 == 0;
                }
            });
            Assert.AreEqual(expectedCalls,calls);
            return result;
        }

        /// <summary>
        /// Gets the aggregate method.
        /// </summary>
        /// <param name="statementName">Name of the statement.</param>
        /// <returns></returns>
        private MethodInfo GetAggregateMethod(string statementName)
        {
            return typeof(TestAggregates).GetMethod(statementName);
        }

        /// <summary>
        /// 
        /// </summary>
        private class TestAggregates : StatementController<IEnumerable<int>>
        {
            /// <summary>
            /// R1s the specified proceed.
            /// </summary>
            /// <param name="proceed">The proceed.</param>
            /// <returns></returns>
            public bool R1(Func<int,bool> proceed)
            {
                return Scope.Any(proceed);
            }

            /// <summary>
            /// R2s the specified haz.
            /// </summary>
            /// <param name="haz">The haz.</param>
            /// <param name="proceed">The proceed.</param>
            /// <returns></returns>
            [Matches("I haz (\\d+) cheeseburgers")]
            public bool R2(int haz,Func<int,bool> proceed)
            {
                Assert.AreEqual(10, haz);
                return Scope.Any(proceed);
            }

            /// <summary>
            /// R3s the specified haz.
            /// </summary>
            /// <param name="haz">The haz.</param>
            /// <param name="other">The other.</param>
            /// <param name="proceed">The proceed.</param>
            /// <returns></returns>
            [Matches("I haz (\\d+) cheeseburgers")]
            public bool R3(int haz,int other,Func<int,bool> proceed)
            {
                Assert.Fail();
                return false;
            }

            /// <summary>
            /// R4s the specified proceed.
            /// </summary>
            /// <param name="proceed">The proceed.</param>
            /// <returns></returns>
            [Matches("I haz (\\d+) cheeseburgers")]
            public bool R4(Func<int,bool> proceed)
            {
                Assert.Fail();
                return false;
            }

            /// <summary>
            /// R5s this instance.
            /// </summary>
            /// <returns></returns>
            public bool R5()
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the method.
        /// </summary>
        /// <param name="statementName">Name of the statement.</param>
        /// <returns></returns>
        private MethodInfo GetMethod(string statementName)
        {
            return typeof(TestStatements).GetMethod(statementName);
        }
    }
}
