using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PaymentRetrieve.Models;

namespace PaymentRetrieve.Repositories
{
    public class SqlPaymentRepo : IPaymentRepository
    {
        private readonly PaymentContext _context;

        public SqlPaymentRepo(PaymentContext context)
        {
            _context = context;
        }

        public Transaction GetTransactionById(Guid id)
        {
            Transaction t = _context.Transaction.FirstOrDefault(p => p.Id == id);
            t.MaskCard();
            return t;
        }

    }
}