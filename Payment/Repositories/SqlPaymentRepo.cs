using System;
using System.Collections.Generic;
using System.Linq;
using Payment.Models;

namespace Payment.Repositories
{
    public class SqlPaymentRepo : IPaymentRepo
    {
        private readonly PaymentContext _context;

        public SqlPaymentRepo(PaymentContext context)
        {
            _context = context;
        }

        public void CreateTransaction(Transaction obj)
        {
            if(obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
            // Update if transaction on existing card - add if not.
            if (_context.Transaction.Any(t => t.Card.CardNumber == obj.Card.CardNumber))
                _context.Transaction.Update(obj);
            else
                _context.Transaction.Add(obj);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void UpdateTransactionStatus(Transaction obj)
        {
            if(obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
            _context.Transaction.Update(obj);
        }
    }
}