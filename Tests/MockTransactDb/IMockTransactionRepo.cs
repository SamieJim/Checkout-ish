using Payment.Repositories;
using PaymentRetrieve.Repositories;
namespace Tests.MockTransactDb
{
    public interface IMockTransactionRepo : IPaymentRepository, IPaymentRepo
    {}
}