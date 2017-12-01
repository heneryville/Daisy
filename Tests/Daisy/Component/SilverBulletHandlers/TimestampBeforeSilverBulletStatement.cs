using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Tests.Daisy.Component.SilverBulletHandlers
{
    using System.Text.RegularExpressions;

    using Ancestry.Daisy.Statements;
    using Ancestry.Daisy.Tests.Daisy.Component.Controllers;
    using Ancestry.Daisy.Tests.Daisy.Component.Domain;

    /// <summary>
    /// 
    /// </summary>
    public class TimestampBeforeSilverBulletStatement : IStatementDefinition
    {
        /// <summary>
        /// Gets the type of the scope.
        /// </summary>
        /// <value>
        /// The type of the scope.
        /// </value>
        public Type ScopeType { get { return typeof(Transaction); } }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get { return "HasTransaction"; } }

        /// <summary>
        /// Gets the transforms scope to.
        /// </summary>
        /// <value>
        /// The transforms scope to.
        /// </value>
        public Type TransformsScopeTo { get { return null; } }

        /// <summary>
        /// Links the specified statement.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        public ILinkedStatement Link(string statement)
        {
            var match = Regex.Match(statement, @"Timestamp before (\d*) year ago");
            if (!match.Success) return null;
            return new Linked(this, int.Parse(match.Groups[1].Value));
        }

        /// <summary>
        /// 
        /// </summary>
        private class Linked : GenericLink
        {
            /// <summary>
            /// Gets or sets the years.
            /// </summary>
            /// <value>
            /// The years.
            /// </value>
            public int Years { get; set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="Linked"/> class.
            /// </summary>
            /// <param name="def">The definition.</param>
            /// <param name="years">The years.</param>
            public Linked(IStatementDefinition def, int years) : base(def)
            {
                Years = years;
            }

            /// <summary>
            /// Executes the specified context.
            /// </summary>
            /// <param name="context">The context.</param>
            /// <returns></returns>
            public override bool Execute(InvokationContext context)
            {
                var controller = new TransactionController();
                Initializer(controller, context);
                return controller.TimestampBeforeYearsAgo(Years);
            }
        }
    }
}
