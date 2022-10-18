using System.Collections.Generic;
using PaymentServices.Types;

namespace PaymentServices.Interfaces
{
    public interface IDataStoreType
    {
        Account GetAccount(string accountNumber);

        void UpdateAccount(Account account);

        List<Account> Accounts { get; set; }
    }
}