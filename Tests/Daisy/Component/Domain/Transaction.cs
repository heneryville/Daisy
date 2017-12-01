namespace Ancestry.Daisy.Tests.Daisy.Component.Domain
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class Transaction
    {
        /// <summary>
        /// Gets or sets the transaction identifier.
        /// </summary>
        /// <value>
        /// The transaction identifier.
        /// </value>
        public int TransactionId { get; set; }
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public TransactionType Type { get; set; }
        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>
        /// The amount.
        /// </value>
        public decimal Amount { get; set; }
        /// <summary>
        /// Gets or sets the timestamp.
        /// </summary>
        /// <value>
        /// The timestamp.
        /// </value>
        public DateTime Timestamp { get; set; }

        private static int TransactionIdCursor = 0;
        /// <summary>
        /// Deposits the specified amount.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <returns></returns>
        public static Transaction Deposit(decimal amount, DateTime timestamp)
        {
            return new Transaction
                {
                    Amount = amount,
                    Timestamp = timestamp,
                    Type = TransactionType.Deposit,
                    TransactionId = TransactionIdCursor++
                };
        }

        /// <summary>
        /// Withdrawls the specified amount.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <returns></returns>
        public static Transaction Withdrawl(decimal amount, DateTime timestamp)
        {
            return new Transaction
                {
                    Amount = -amount,
                    Timestamp = timestamp,
                    Type = TransactionType.Withdrawl,
                    TransactionId = TransactionIdCursor++
                };
        }
    }
}
