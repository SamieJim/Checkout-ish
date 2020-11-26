using System.ComponentModel.DataAnnotations;
using PaymentRetrieve.Models;

namespace PaymentRetrieve.DTOs
{
    public class TransactionReadDTO
    {
        [Key]
        [Required]
        public System.Guid Id { get; set; }

        [Required]
        public string CardNumber { get; set; }

        [Required]
        public string Currency = System.Globalization.NumberFormatInfo.CurrentInfo.CurrencySymbol;
    
        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Amount must be more than 0!")]
        public double Amount { get; set; } = 0.0;

        [Required]
        public Status Status { get; set; }
    }
}