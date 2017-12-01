using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Tests.Daisy.Component.SilverBulletHandlers
{
    using Ancestry.Daisy.Statements;
    using Ancestry.Daisy.Tests.Daisy.Component.Controllers;
    using Ancestry.Daisy.Tests.Daisy.Component.Domain;

    /// <summary>
    /// 
    /// </summary>
    public class HasTransactionSilverBulletStatement : IStatementDefinition
    {
        /// <summary>
        /// Gets the type of the scope.
        /// </summary>
        /// <value>
        /// The type of the scope.
        /// </value>
        public Type ScopeType { get { return typeof(Account); } }

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
        public Type TransformsScopeTo { get { return typeof(Transaction); } }

        /// <summary>
        /// Links the specified statement.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        public ILinkedStatement Link(string statement)
        {
            if (statement != "Has Transaction") return null;
            return new Linked(this);
        }

        /// <summary>
        /// 
        /// </summary>
        private class Linked : GenericLink
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Linked"/> class.
            /// </summary>
            /// <param name="def">The definition.</param>
            public Linked(IStatementDefinition def) : base(def) { }

            /// <summary>
            /// Executes the specified context.
            /// </summary>
            /// <param name="context">The context.</param>
            /// <returns></returns>
            public override bool Execute(InvokationContext context)
            {
                var controller = new AccountController();
                Initializer(controller, context);
                return controller.HasTransaction(context.Proceed);
            }
        }
    }
}
