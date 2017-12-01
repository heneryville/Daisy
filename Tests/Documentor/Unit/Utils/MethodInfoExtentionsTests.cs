using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Tests.Documentor.Unit.Utils
{
    using Ancestry.Daisy.Documentation.Utils;
    using Ancestry.Daisy.Tests.Daisy.Component.Controllers;

    using NUnit.Framework;

    /// <summary>
    /// 
    /// </summary>
    [TestFixture,Category("Unit")]
    public class DocSignatureExtentionsTests
    {
        /// <summary>
        /// Its the gets signatures.
        /// </summary>
        /// <param name="clas">The clas.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <returns></returns>
        [TestCase(typeof(AccountController),"Type", Result = "Ancestry.Daisy.Tests.Daisy.Component.Controllers.AccountController.Type(System.String)")]
        [TestCase(typeof(AccountController), "IsBalanced", Result = "Ancestry.Daisy.Tests.Daisy.Component.Controllers.AccountController.IsBalanced")]
        [TestCase(typeof(AccountController), "BalanceBetween", Result = "Ancestry.Daisy.Tests.Daisy.Component.Controllers.AccountController.BalanceBetween(System.Int32,System.Int32)")]
        [TestCase(typeof(AccountController), "HasTransaction", Result = "Ancestry.Daisy.Tests.Daisy.Component.Controllers.AccountController.HasTransaction(System.Func{Ancestry.Daisy.Tests.Daisy.Component.Domain.Transaction,System.Boolean})")]
        public string ItGetsSignatures(Type clas,string methodName)
        {
            return clas.GetMethod(methodName).GetDocStyleSignature();
        }

        /// <summary>
        /// Its the gets type signatures.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        [TestCase(typeof(AccountController), Result = "Ancestry.Daisy.Tests.Daisy.Component.Controllers.AccountController")]
        [TestCase(typeof(bool), Result = "System.Boolean")]
        [TestCase(typeof(Func<bool>), Result = "System.Func{System.Boolean}")]
        public string ItGetsTypeSignatures(Type type)
        {
            return type.GetDocStyleSignature();
        }
    }
}
