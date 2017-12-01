using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Tests.TestObjects
{
    using System.Reflection;

    using Ancestry.Daisy.Documentation;

    /// <summary>
    /// 
    /// </summary>
    public class EmptyCommentDocumentation : ICommentDocumentation
    {
        /// <summary>
        /// Fors the method.
        /// </summary>
        /// <param name="methodInfo">The method information.</param>
        /// <returns></returns>
        public MethodDocumentation ForMethod(MethodInfo methodInfo)
        {
            return null;
        }
    }
}
