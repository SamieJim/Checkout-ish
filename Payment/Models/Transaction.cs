using System.ComponentModel.DataAnnotations;

namespace Payment.Models
{
    public class Transaction
    {
        [Key]
        [Required]
        public System.Guid Id { get; set;}

        [Required]
        public Card Card { get; set; }

        [Required]
        public string Currency = System.Globalization.NumberFormatInfo.CurrentInfo.CurrencySymbol;
    
        [Required]
        [Range(1.0, double.MaxValue, ErrorMessage = "Amount must be more than 0!")]
        public double Amount { get; set; } = 0.0;

        [Required]
        public Status Status { get; set; } = Status.Requested;
    }
}