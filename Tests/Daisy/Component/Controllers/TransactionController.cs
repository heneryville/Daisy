namespace Ancestry.Daisy.Tests.Daisy.Component.Controllers
{
    using System;

    using Ancestry.Daisy.Statements;
    using Ancestry.Daisy.Tests.Daisy.Component.Domain;

    /// <summary>
    /// 
    /// </summary>
    public class TransactionController : StatementController<Transaction>
    {
        /// <summary>
        /// Timestamps the before years ago.
        /// </summary>
        /// <param name="yearsAgo">The years ago.</param>
        /// <returns></returns>
        [Matches("Timestamp before (\\d+) years? ago")]
        public bool TimestampBeforeYearsAgo(int yearsAgo)
        {
            return DateTime.Now.AddYears(-yearsAgo) > Scope.Timestamp;
        }

        /// <summary>
        /// Amounts the is greater than.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        [Matches("Amount is greater than (\\d+)")]
        public bool AmountIsGreaterThan(int value)
        {
            return Scope.Amount > value;
        }
    }
}
