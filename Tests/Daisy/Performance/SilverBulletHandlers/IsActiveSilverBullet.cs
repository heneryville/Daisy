using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Tests.Daisy.Performance.SilverBulletHandlers
{
    using System.Reflection;

    using Ancestry.Daisy.Statements;
    using Ancestry.Daisy.Tests.Daisy.Component.Controllers;
    using Ancestry.Daisy.Tests.Daisy.Component.Domain;

    public class IsActiveSilverBullet : SilverBulletDefinition
    {
        public IsActiveSilverBullet()
            : base(typeof(UserController), "IsActive")
        {
        }

        public override bool Execute(InvokationContext context)
        {
            return new UserController(){
                Scope = (User)context.Scope,
                Attachments = context.Attachments,
                Context = context.Context
            }.IsActive();
        }
    }
}
