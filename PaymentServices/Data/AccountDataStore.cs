using System.Collections.Generic;
using System.Linq;
using PaymentServices.Interfaces;
using PaymentServices.Types;

namespace PaymentServices.Data
{
    public class AccountDataStore : IDataStoreType
    {
        public Account GetAccount(string accountNumber)
        {
            var account = Accounts.Where(x => x.AccountNumber == accountNumber).FirstOrDefault();

            return account;
        }

        public void UpdateAccount(Account account) { }

        public List<Account> Accounts { get; set; } = new List<Account>();
    }
}