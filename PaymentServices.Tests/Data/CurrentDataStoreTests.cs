using NUnit.Framework;
using PaymentServices.Data;

namespace PaymentServices.Tests.Data
{
    public class CurrentDataStoreTests
    {
        [Test]
        public void ShouldGetDataStore()
        {
            var dataStore = new CurrentDataStoreAccount();

            var result = dataStore.CreateDataStoreAccountType();

            Assert.IsNotNull(result);
        }
    }
}
