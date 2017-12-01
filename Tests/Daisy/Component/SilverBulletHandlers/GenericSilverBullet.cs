using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Tests.Daisy.Component.SilverBulletHandlers
{
    using Ancestry.Daisy.Statements;

    /// <summary>
    /// 
    /// </summary>
    public abstract class GenericSilverBullet : IStatementDefinition
    {
        /// <summary>
        /// Gets the type of the scope.
        /// </summary>
        /// <value>
        /// The type of the scope.
        /// </value>
        public abstract Type ScopeType { get; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public abstract string Name { get; }

        /// <summary>
        /// Gets the transforms scope to.
        /// </summary>
        /// <value>
        /// The transforms scope to.
        /// </value>
        public abstract Type TransformsScopeTo { get; }

        /// <summary>
        /// Links the specified statement.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns></returns>
        public abstract ILinkedStatement Link(string statement);
    }
}
