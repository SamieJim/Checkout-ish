using System;
using System.Collections.Generic;
using Payment.Models;

namespace Payment.Repositories
{
    public interface IPaymentRepo
    {
        bool SaveChanges();
        void CreateTransaction(Transaction obj);
        void UpdateTransactionStatus(Transaction transactionModel);
    }
}