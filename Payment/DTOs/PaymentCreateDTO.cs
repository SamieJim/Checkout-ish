using System.ComponentModel.DataAnnotations;
using Payment.Models;

namespace Payment.DTOs
{
    public class TransactionCreateDTO
    {

        [Required]
        public Card Card { get; set; }
    
        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Amount must be more than 0!")]
        public double Amount { get; set; } = 0.0;
    }
}