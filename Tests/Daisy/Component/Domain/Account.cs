namespace Ancestry.Daisy.Tests.Daisy.Component.Domain
{
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class Account
    {
        /// <summary>
        /// Gets or sets the account identifier.
        /// </summary>
        /// <value>
        /// The account identifier.
        /// </value>
        public int AccountId { get; set; }

        /// <summary>
        /// Gets or sets the balance.
        /// </summary>
        /// <value>
        /// The balance.
        /// </value>
        public decimal Balance { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public AccountType Type { get; set; }

        /// <summary>
        /// Gets or sets the transactions.
        /// </summary>
        /// <value>
        /// The transactions.
        /// </value>
        public IList<Transaction> Transactions { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "AccountId: " + AccountId + ",Balance: " + Balance + ",Type: " + Type;
        }
    }
}
