using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Tests.TestObjects
{
    using System.Text.RegularExpressions;

    using Ancestry.Daisy.Statements;

    /// <summary>
    /// 
    /// </summary>
    public class MoqStatement : IStatementDefinition
    {
        /// <summary>
        /// The test
        /// </summary>
        private readonly string test = null;

        /// <summary>
        /// The predicate
        /// </summary>
        private readonly Func<int, bool> predicate = null;

        /// <summary>
        /// Gets or sets the type of the scope.
        /// </summary>
        /// <value>
        /// The type of the scope.
        /// </value>
        public Type ScopeType { get; set; }

        /// <summary>
        /// Matcheses the specified matching context.
        /// </summary>
        /// <param name="matchingContext">The matching context.</param>
        /// <returns></returns>
        public Match Matches(MatchingContext matchingContext)
        {
            return new Regex("^" + test + "$", RegexOptions.IgnoreCase).Match(matchingContext.Statement);
        }

        /// <summary>
        /// Executes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public bool Execute(InvokationContext context)
        {
            return predicate((int)context.Scope);
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the transforms scope to.
        /// </summary>
        /// <value>
        /// The transforms scope to.
        /// </value>
        public Type TransformsScopeTo { get; set; }

        /// <summary>
        /// Links the specified statement.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        public ILinkedStatement Link(string statement)
        {
            return null;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class FakeStatement : IStatementDefinition
    {
        private readonly string test;

        private readonly Func<int, bool> predicate;

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeStatement"/> class.
        /// </summary>
        /// <param name="test">The test.</param>
        /// <param name="predicate">The predicate.</param>
        public FakeStatement(string test, Func<int,bool> predicate)
        {
            this.test = test;
            this.predicate = predicate;
            Name = test;
        }

        /// <summary>
        /// Gets the type of the scope.
        /// </summary>
        /// <value>
        /// The type of the scope.
        /// </value>
        public Type ScopeType { get { return typeof(int); } }

        /// <summary>
        /// Matcheses the specified matching context.
        /// </summary>
        /// <param name="matchingContext">The matching context.</param>
        /// <returns></returns>
        public Match Matches(MatchingContext matchingContext)
        {
            return new Regex("^" + test + "$", RegexOptions.IgnoreCase).Match(matchingContext.Statement);
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets the transforms scope to.
        /// </summary>
        /// <value>
        /// The transforms scope to.
        /// </value>
        public Type TransformsScopeTo { get; set; }

        /// <summary>
        /// Links the specified statement.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        public ILinkedStatement Link(string statement)
        {
            return new FakeLinkedStatement(predicate,this);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class FakeLinkedStatement : ILinkedStatement
    {
        private readonly Func<int, bool> predicate;

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeLinkedStatement"/> class.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="def">The definition.</param>
        public FakeLinkedStatement(Func<int, bool> predicate, IStatementDefinition def)
        {
            this.predicate = predicate;
            Definition = def;
        }

        /// <summary>
        /// Gets the definition.
        /// </summary>
        /// <value>
        /// The definition.
        /// </value>
        public IStatementDefinition Definition { get; private set; }

        /// <summary>
        /// Executes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public bool Execute(InvokationContext context)
        {
            return predicate((int)context.Scope);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="F"></typeparam>
    /// <typeparam name="T"></typeparam>
    public class FakeAggregate<F,T> : IStatementDefinition
    {
        private readonly string test;

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeAggregate{F, T}"/> class.
        /// </summary>
        /// <param name="test">The test.</param>
        public FakeAggregate(string test)
        {
            this.test = test;
            Name = test;
            TransformsScopeTo = typeof(T);
        }

        /// <summary>
        /// Gets the type of the scope.
        /// </summary>
        /// <value>
        /// The type of the scope.
        /// </value>
        public Type ScopeType { get { return typeof(int); } }

        /// <summary>
        /// Matcheses the specified matching context.
        /// </summary>
        /// <param name="matchingContext">The matching context.</param>
        /// <returns></returns>
        public Match Matches(MatchingContext matchingContext)
        {
            return new Regex("^" + test + "$", RegexOptions.IgnoreCase).Match(matchingContext.Statement);
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the transforms scope to.
        /// </summary>
        /// <value>
        /// The transforms scope to.
        /// </value>
        public Type TransformsScopeTo { get; private set; }

        /// <summary>
        /// Links the specified statement.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        public ILinkedStatement Link(string statement)
        {
            return new FakeLinkedStatement(this);
        }

        /// <summary>
        /// 
        /// </summary>
        private class FakeLinkedStatement : ILinkedStatement
        {
            /// <summary>
            /// The parent
            /// </summary>
            private FakeAggregate<F,T> parent;

            /// <summary>
            /// Initializes a new instance of the <see cref="FakeLinkedStatement"/> class.
            /// </summary>
            /// <param name="parent">The parent.</param>
            public FakeLinkedStatement(FakeAggregate<F,T> parent)
            {
                this.parent = parent;
                Definition = parent;
            }

            /// <summary>
            /// Gets or sets the definition.
            /// </summary>
            /// <value>
            /// The definition.
            /// </value>
            public IStatementDefinition Definition { get; private set; }

            /// <summary>
            /// Executes the specified context.
            /// </summary>
            /// <param name="context">The context.</param>
            /// <returns></returns>
            public bool Execute(InvokationContext context)
            {
                return ((IEnumerable<F>)context.Scope).Any(x => context.Proceed(x));
            }
        }
    }
}
