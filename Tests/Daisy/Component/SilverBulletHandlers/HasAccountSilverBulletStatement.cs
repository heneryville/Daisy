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
    public class HasAccountSilverBulletStatement : IStatementDefinition
    {
        /// <summary>
        /// Gets the type of the scope.
        /// </summary>
        /// <value>
        /// The type of the scope.
        /// </value>
        public Type ScopeType { get { return typeof(User); } }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get { return "IsActive"; } }

        /// <summary>
        /// Gets the transforms scope to.
        /// </summary>
        /// <value>
        /// The transforms scope to.
        /// </value>
        public Type TransformsScopeTo { get { return typeof(Account); } }

        /// <summary>
        /// Links the specified statement.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        public ILinkedStatement Link(string statement)
        {
            if (statement != "Has Account") return null;
            return new Linked(this);
        }

        private class Linked : GenericLink
        {
            public Linked(IStatementDefinition def) : base(def) { }

            public override bool Execute(InvokationContext context)
            {
                var controller = new UserController();
                Initializer(controller, context);
                return controller.HasAccount(context.Proceed);
            }
        }
    }
}
