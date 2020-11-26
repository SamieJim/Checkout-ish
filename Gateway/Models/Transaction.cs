using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Models
{
    
    public class Transaction
    {
        [Required]
        public Card Card { get; set; }
    
        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Amount must be more than 0!")]

        [Display(Name = "Amount")]
        public double Amount { get; set; } = 0.0;
    }
}