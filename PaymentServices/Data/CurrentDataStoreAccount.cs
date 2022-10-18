using System.Configuration;
using PaymentServices.Common;
using PaymentServices.Interfaces;

namespace PaymentServices.Data
{
    public class CurrentDataStoreAccount
    {
        public IDataStoreType CreateDataStoreAccountType()
        {
            var dataStoreType = ConfigurationManager.AppSettings[Constants.DATA_STORE_TYPE];

            if (dataStoreType == Constants.DATA_STORE_BACKUP)
            {
                return new BackupAccountDataStore();
            }
            else
            {
                return new AccountDataStore();
            }
        }
    }
}