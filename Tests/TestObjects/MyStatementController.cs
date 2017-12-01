namespace Ancestry.Daisy.Tests.TestObjects
{
    using System;
    using System.Linq;

    using Ancestry.Daisy.Statements;

    /// <summary>
    /// 
    /// </summary>
    public class MyStatementController : StatementController<ParentObject>
    {
        /// <summary>
        /// Numbers the is greater than.
        /// </summary>
        /// <param name="greaterThan">The greater than.</param>
        /// <returns></returns>
        [Matches("Number is greater than \\d+")]
        public bool NumberIsGreaterThan(int greaterThan)
        {
            return Scope.Propety1 > greaterThan;
        }

        /// <summary>
        /// Numbers the is less than.
        /// </summary>
        /// <param name="greaterThan">The greater than.</param>
        /// <returns></returns>
        [Matches("Number is less than (\\d+)")]
        public bool NumberIsLessThan(int greaterThan)
        {
            return Scope.Propety1 > greaterThan;
        }

        /// <summary>
        /// Wheres the string.
        /// </summary>
        /// <param name="applyToChildren">The apply to children.</param>
        /// <returns></returns>
        public bool WhereString(Func<string,bool> applyToChildren)
        {
            return applyToChildren(Scope.Property2);
        }

        /// <summary>
        /// Determines whether the specified proceed has objects.
        /// </summary>
        /// <param name="proceed">The proceed.</param>
        /// <returns></returns>
        public bool HasObjects(Func<object,bool> proceed)
        {
            return Scope.Property3.Any(proceed);
        }

        /// <summary>
        /// Determines whether the specified proceed has digit.
        /// </summary>
        /// <param name="proceed">The proceed.</param>
        /// <returns></returns>
        public bool HasDigit(Func<int,bool> proceed)
        {
            return Scope.Propety1
                .ToString()
                .Any(x => proceed(int.Parse("" + x)));
        }
    }
}
