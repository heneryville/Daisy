using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Tests.Documentor.Unit
{
    using Ancestry.Daisy.Documentation;
    using Ancestry.Daisy.Tests.Daisy.Component.Controllers;

    using Monads.NET;

    using NUnit.Framework;

    /// <summary>
    /// 
    /// </summary>
    [TestFixture,Category("Unit")]
    public class CommentDocumentationTest
    {
        /// <summary>
        /// Its the parses summaries.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="summary">The summary.</param>
        [TestCase(typeof(AccountController),"Type","True when the Account is of the given type.")]
        [TestCase(typeof(AccountController), "IsBalanced", "True when the Account has a <strong>non-negative</strong> balance")]
        [TestCase(typeof(AccountController), "BalanceBetween", "True when the Account has a balance between two values. <br/> Bounds are exclusive.")]
        public void ItParsesSummaries(Type type, string methodName, string summary)
        {
            var docs = CommentDocumentation.ParseFile("Ancestry.Daisy.Tests.XML");
            var methodDoc = docs.ForMethod(type.GetMethod(methodName));
            Assert.IsNotNull(methodDoc);
            Assert.AreEqual(summary, methodDoc.Summary);
        }

        /// <summary>
        /// Its the parses parameters.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="paramDescription">The parameter description.</param>
        [TestCase(typeof(AccountController), "BalanceBetween","lowerEnd", "The lowest value a balance may have")]
        [TestCase(typeof(AccountController), "BalanceBetween","higherEnd", "The highest value a balance may have")]
        public void ItParsesParameters(Type type, string methodName, string paramName, string paramDescription)
        {
            var docs = CommentDocumentation.ParseFile("Ancestry.Daisy.Tests.XML");
            var paramDoc = docs.ForMethod(type.GetMethod(methodName))
                .Parameters.With(paramName);
            Assert.IsNotNull(paramDoc);
            Assert.AreEqual(paramDescription, paramDoc);
        }
    }
}
