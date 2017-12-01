using Ancestry.Daisy.Program;

namespace Ancestry.Daisy.TestHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text.RegularExpressions;

    using Ancestry.Daisy.Statements;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public class TestHelper
    {

        public static bool Matches(Type controller, string methodName, string statement)
        {
            var method = GetMethod(controller, methodName);
            var statementHandler = new ReflectionStatementDefinition(method, controller);
            return statementHandler.Matches(new MatchingContext() {
                    Statement = statement,
                }).Success;
        }

        private static MethodInfo GetMethod(Type controller, string methodName)
        {
            var method = controller.GetMethod(methodName);
            if(method == null) throw new ArgumentException(string.Format("Cannot find method {0} on {1}", methodName, controller.GetType().Name));
            return method;
        }

        public static InvokationResult Invoke<T>(Type controller, string methodName, T scope,
            string statement, ContextBundle context = null, Func<object, bool> proceed = null)
        {
            if(proceed == null)
                proceed = o => false;
            context = context ?? new ContextBundle();
            var method = GetMethod(controller, methodName);
            var statementHandler = new ReflectionStatementDefinition(method, controller);
            var invokationResult = new InvokationResult()
                {
                    Context = context,
                    Statement = statement,
                    MatchingCriteria = statementHandler.GetMatchingCriteria(),
                    Attachments = new ContextBundle()
                };
            invokationResult.Match = statementHandler.Matches(new MatchingContext() {
                    Statement = statement,
                    ScopeType = typeof(T)
                });
            if (!invokationResult.Matched) return invokationResult;
            var link = statementHandler.Link(statement);
            if (link == null) throw new Exception("If matched, should have linked, but didn't");
            invokationResult.Result = link.Execute(new InvokationContext() {
                Context = invokationResult.Context,
                Attachments = invokationResult.Attachments,
                Scope = scope,
                Proceed = proceed
            });
            return invokationResult;
        }

        public class InvokationResult
        {
            public bool Matched { get { return Match != null && Match.Success; } }

            public Match Match { get; set; }

            public ContextBundle Context { get; set; }

            public ContextBundle Attachments { get; set; }

            public bool Result { get; set; }

            public string Statement { get; set; }

            public Regex MatchingCriteria { get; set; }

            public InvokationResult AssertMatched()
            {
                Assert.IsTrue(Matched,string.Format("Expected {0} to match {1}.", Statement, MatchingCriteria));
                return this;
            }

            public InvokationResult AssertMatched(bool matched)
            {
                if (matched) return AssertMatched();
                else return AssertNotMatched();
            }

            public InvokationResult AssertNotMatched()
            {
                Assert.IsFalse(Matched,string.Format("Expected {0} to not match {1}, but it did.", Statement, MatchingCriteria));
                return this;
            }

            public InvokationResult AssertResult(bool expected)
            {
                Assert.AreEqual(expected,Result,string.Format("Expected result to be {0}, but was {1}", expected, Result));
                return this;
            }

            public InvokationResult AssertContextHas(string property)
            {
                if (!((IDictionary<String, object>)Context).ContainsKey(property))
                    Assert.Fail("Expected Context.{0} to be set, but was not", property);
                return this;
            }
        }
    }
}
