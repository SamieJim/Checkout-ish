using System;
using System.Collections.Generic;
using PaymentRetrieve.Models;

namespace PaymentRetrieve.Repositories
{
    public interface IPaymentRepository
    {
        Transaction GetTransactionById(Guid id);
    }
}