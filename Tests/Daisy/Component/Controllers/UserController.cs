namespace Ancestry.Daisy.Tests.Daisy.Component.Controllers
{
    using System;
    using System.Linq;

    using Ancestry.Daisy.Statements;
    using Ancestry.Daisy.Tests.Daisy.Component.Domain;

    /// <summary>
    /// 
    /// </summary>
    public class UserController : StatementController<User>
    {
        /// <summary>
        /// Determines whether the specified procced has account.
        /// </summary>
        /// <param name="procced">The procced.</param>
        /// <returns></returns>
        public bool HasAccount(Func<Account,bool> procced)
        {
            Trace("Has {0} accounts", Scope.Accounts.Count);
            return Scope.Accounts.Any(procced);
        }

        /// <summary>
        /// Alls the accounts.
        /// </summary>
        /// <param name="procced">The procced.</param>
        /// <returns></returns>
        public bool AllAccounts(Func<Account,bool> procced)
        {
            return Scope.Accounts.Any(procced);
        }

        /// <summary>
        /// Determines whether this instance is active.
        /// </summary>
        /// <returns></returns>
        public bool IsActive()
        {
            return Scope.IsActive;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "User";
        }
    }
}
