using PaymentServices.Types;

namespace PaymentServices.Interfaces
{
    public interface IPaymentService
    {
        MakePaymentResult MakePayment(MakePaymentRequest request);
    }
}
