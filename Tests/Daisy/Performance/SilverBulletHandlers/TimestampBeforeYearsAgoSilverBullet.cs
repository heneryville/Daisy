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

    public class TimestampBeforeYearsAgoSilverBullet : SilverBulletDefinition
    {
        public TimestampBeforeYearsAgoSilverBullet()
            : base(typeof(TransactionController), "TimestampBeforeYearsAgo")
        {
        }

        public override bool Execute(InvokationContext context)
        {
            var value = int.Parse(context.Match.Groups[1].Captures[0].Value);

            return new TransactionController{
                Scope = (Transaction)context.Scope,
                Attachments = context.Attachments,
                Context = context.Context
            }.TimestampBeforeYearsAgo(value);
        }
    }
}
