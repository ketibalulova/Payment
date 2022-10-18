using System;
using NUnit.Framework;
using PaymentServices.Data;
using PaymentServices.Enums;
using PaymentServices.Services;
using PaymentServices.Types;

namespace PaymentServices.Tests
{
    public class PaymentServiceTests
    {
        private Account debtorAccountBacs;
        private Account debtorAccountFasterPayments;
        private Account debtorAccountChaps;
        private Account creditorAccount;
        private AccountDataStore accountDataStore;
        private PaymentService service;

        [SetUp]
        public void Setup()
        {
            debtorAccountBacs = new Account()
            {
                AccountNumber = "1234",
                Balance = 100m,
                AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs,
                Status = AccountStatus.Live,
            };

            debtorAccountFasterPayments = new Account()
            {
                AccountNumber = "4321",
                Balance = 100m,
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments,
                Status = AccountStatus.Live,
            };

            debtorAccountChaps = new Account()
            {
                AccountNumber = "2341",
                Balance = 100m,
                AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps,
                Status = AccountStatus.Live,
            };

            creditorAccount = new Account()
            {
                AccountNumber = "5678",
                Balance = 50m,
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments,
                Status = AccountStatus.Live,
            };

            accountDataStore = new AccountDataStore();

            service = new PaymentService(accountDataStore);

            accountDataStore.Accounts
                .AddRange(new Account[] { debtorAccountBacs, debtorAccountFasterPayments, debtorAccountChaps, creditorAccount });
        }

        [Test]
        public void ShouldFailsWhenUserIsNull()
        {
            MakePaymentRequest request = new MakePaymentRequest()
            {
                Amount = 10m,
                CreditorAccountNumber = "111111",
                DebtorAccountNumber = "222222",
                PaymentDate = DateTime.Now,
                PaymentScheme = PaymentScheme.Chaps,
            };

            MakePaymentResult result = service.MakePayment(request);

            Assert.IsFalse(result.Success);
        }

        [Test]
        public void ShouldFailsWhenPaymentIsNotAllowed()
        {
            MakePaymentRequest request = new MakePaymentRequest()
            {
                Amount = 10m,
                CreditorAccountNumber = creditorAccount.AccountNumber,
                DebtorAccountNumber = debtorAccountBacs.AccountNumber,
                PaymentDate = DateTime.Now,
                PaymentScheme = PaymentScheme.FasterPayments,
            };

            MakePaymentResult result = service.MakePayment(request);

            Assert.IsFalse(result.Success);
        }

        [Test]
        [TestCase(PaymentScheme.Chaps, 5.0d, AccountStatus.Disabled)]
        [TestCase(PaymentScheme.FasterPayments, 60.0d, AccountStatus.Live)]
        public void ShouldFailsWhenConditionsAreFalse(PaymentScheme scheme, decimal amount, AccountStatus status)
        {
            debtorAccountBacs.Status = status;

            MakePaymentRequest request = new MakePaymentRequest()
            {
                Amount = amount,
                CreditorAccountNumber = creditorAccount.AccountNumber,
                DebtorAccountNumber = debtorAccountBacs.AccountNumber,
                PaymentDate = DateTime.Now,
                PaymentScheme = scheme
            };

            MakePaymentResult result = service.MakePayment(request);

            Assert.IsFalse(result.Success);
        }

        [Test]
        public void PaymentBacsShouldBeSuccessful()
        {
            MakePaymentRequest request = new MakePaymentRequest()
            {
                Amount = 10m,
                CreditorAccountNumber = creditorAccount.AccountNumber,
                DebtorAccountNumber = debtorAccountBacs.AccountNumber,
                PaymentDate = DateTime.Now,
                PaymentScheme = PaymentScheme.Bacs,
            };

            MakePaymentResult result = service.MakePayment(request);

            Assert.IsTrue(result.Success);
        }

        [Test]
        public void PaymentFasterShouldBeSuccessful()
        {
            MakePaymentRequest request = new MakePaymentRequest()
            {
                Amount = 10m,
                CreditorAccountNumber = creditorAccount.AccountNumber,
                DebtorAccountNumber = debtorAccountFasterPayments.AccountNumber,
                PaymentDate = DateTime.Now,
                PaymentScheme = PaymentScheme.FasterPayments,
            };

            MakePaymentResult result = service.MakePayment(request);

            Assert.IsTrue(result.Success);
        }

        [Test]
        public void PaymentChapsShouldBeSuccessful()
        {
            MakePaymentRequest request = new MakePaymentRequest()
            {
                Amount = 10m,
                CreditorAccountNumber = creditorAccount.AccountNumber,
                DebtorAccountNumber = debtorAccountChaps.AccountNumber,
                PaymentDate = DateTime.Now,
                PaymentScheme = PaymentScheme.Chaps,
            };

            MakePaymentResult result = service.MakePayment(request);

            Assert.IsTrue(result.Success);
        }
    }
}