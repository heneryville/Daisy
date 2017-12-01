namespace Ancestry.Daisy.Tests.Daisy.Component
{
    using System;
    using System.Collections.Generic;

    using Ancestry.Daisy.Tests.Daisy.Component.Domain;

    /// <summary>
    /// 
    /// </summary>
    public static class TestData
    {
        /// <summary>
        /// User Ben
        /// </summary>
        public static User Ben = new User() {
                UserId = 1,
                FirstName = "Ben",
                LastName = "Sellers",
                IsActive = true,
                MemberSince = DateTime.Now.AddYears(-1),
                Accounts = new List<Account>() {
                        new Account() {
                                AccountId = 1,
                                Balance = 100.23M,
                                Type = AccountType.Checking,
                                Transactions = new List<Transaction>()
                                    {
                                        Transaction.Deposit(50, DateTime.Now.AddYears(-1)),
                                        Transaction.Deposit(50, DateTime.Now.AddMonths(-11)),
                                        Transaction.Deposit(50, DateTime.Now.AddMonths(-10)),
                                        Transaction.Deposit(50, DateTime.Now.AddMonths(-9)),
                                        Transaction.Withdrawl(78, DateTime.Now.AddMonths(-8)),
                                        Transaction.Withdrawl(21.77M, DateTime.Now.AddMonths(-7)),
                                    }
                            },
                        new Account() {
                                AccountId = 2,
                                Balance = -49.77M,
                                Type = AccountType.Checking,
                                Transactions = new List<Transaction>() {
                                        Transaction.Deposit(50, DateTime.Now.AddYears(-1)),
                                        Transaction.Withdrawl(78, DateTime.Now.AddMonths(-8)),
                                        Transaction.Withdrawl(21.77M, DateTime.Now.AddMonths(-7)),
                                    }
                            },
                        new Account() {
                                AccountId = 3,
                                Balance = 0,
                                Type = AccountType.MoneyMarket,
                                Transactions = new List<Transaction>() { }
                            }
                    }
            };
    }
}
