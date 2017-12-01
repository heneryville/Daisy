namespace Ancestry.Daisy.Tests.Daisy.Component.Domain
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the accounts.
        /// </summary>
        /// <value>
        /// The accounts.
        /// </value>
        public IList<Account> Accounts { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the member since.
        /// </summary>
        /// <value>
        /// The member since.
        /// </value>
        public DateTime MemberSince { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive { get; set; }
    }
}
