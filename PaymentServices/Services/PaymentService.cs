using System;
using PaymentServices.Enums;
using PaymentServices.Interfaces;
using PaymentServices.Types;

namespace PaymentServices.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IDataStoreType dataStore;

        public PaymentService(IDataStoreType dataStore)
        {
            this.dataStore = dataStore;
        }

        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            var result = new MakePaymentResult();

            Account account = dataStore.GetAccount(request.DebtorAccountNumber);

            PaymentScheme paymentScheme = request.PaymentScheme;

            string paymentKey = Enum.GetName(typeof(PaymentScheme), paymentScheme);

            AllowedPaymentSchemes allowedPaymentScheme = (AllowedPaymentSchemes)Enum.Parse(typeof(AllowedPaymentSchemes), paymentKey); 

            if (account == null || !account.AllowedPaymentSchemes.HasFlag(allowedPaymentScheme))
            {
                result.Success = false;

                return result;
            }

            switch (request.PaymentScheme)
            {
                case PaymentScheme.FasterPayments:
                    if (account.Balance < request.Amount)
                    {
                        result.Success = false;
                    }
                    break;

                case PaymentScheme.Chaps:
                    if (account.Status != AccountStatus.Live)
                    {
                        result.Success = false;
                    }
                    break;
            }

            if (result.Success)
            {
                account.Balance -= request.Amount;

                dataStore.UpdateAccount(account);
            }

            return result;
        }
    }
}