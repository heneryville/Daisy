using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Tests.Daisy.Performance.SilverBulletHandlers
{
    using Ancestry.Daisy.Statements;
    using Ancestry.Daisy.Tests.Daisy.Component.Controllers;
    using Ancestry.Daisy.Tests.Daisy.Component.Domain;

    public class HasTransactionSilverBullet : SilverBulletDefinition
    {
        public HasTransactionSilverBullet()
            : base(typeof(AccountController), "HasTransaction")
        {
        }

        public override bool Execute(InvokationContext context)
        {
            return new AccountController(){
                Scope = (Account)context.Scope,
                Attachments = context.Attachments,
                Context = context.Context
            }.HasTransaction(context.Proceed);
        }
    }
}
