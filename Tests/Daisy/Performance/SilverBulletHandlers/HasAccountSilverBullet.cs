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

    public class HasAccountSilverBullet : SilverBulletDefinition
    {
        public HasAccountSilverBullet()
            : base(typeof(UserController), "HasAccount")
        {
        }

        public override bool Execute(InvokationContext context)
        {
            return new UserController(){
                Scope = (User)context.Scope,
                Attachments = context.Attachments,
                Context = context.Context
            }.HasAccount(context.Proceed);
        }
    }
}
