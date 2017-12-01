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
    public abstract class GenericLink : ILinkedStatement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenericLink"/> class.
        /// </summary>
        /// <param name="def">The definition.</param>
        protected GenericLink(IStatementDefinition def)
        {
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
        public abstract bool Execute(InvokationContext context);

        /// <summary>
        /// Initializers the specified controller.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller">The controller.</param>
        /// <param name="context">The context.</param>
        protected void Initializer<T>(StatementController<T> controller, InvokationContext context)
        {
            controller.Attachments = context.Attachments;
            controller.Context = context.Context;
            controller.Scope = (T)context.Scope;
            controller.Tracer = context.Tracer;
        }
    }
}
