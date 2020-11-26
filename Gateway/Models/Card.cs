using System;
using System.ComponentModel.DataAnnotations;


namespace Gateway.Models
{
    public class Card
    {
        [Key]
        [Required]
        [RegularExpression(@"^(?:(4[0-9]{12}(?:[0-9]{3})?)|(5[1-5][0-9]{14})|↵
                        (6(?:011|5[0-9]{2})[0-9]{12})|(3[47][0-9]{13})|(3(?:0[0-5]|[68][0-9])↵
                        [0-9]{11})|((?:2131|1800|35[0-9]{3})[0-9]{11}))$",
            ErrorMessage="Invalid card number format.")]
        [Display(Name = "Card number")]
        public string CardNumber { get; set; }

        [Required]
        [RegularExpression(@"^((?:[A-Za-z]+ ?){1,3})$",
            ErrorMessage="Please enter a valid name.")]
        [Display(Name = "Cardholder name")]
        public string NameOnCard { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{3}$",
        ErrorMessage="Please enter a valid CVV.")]
        [Display(Name = "CVV")]
        public int Cvv { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Expiry date")]
        [Expiry]
        public DateTime ExpiryDate {get; set;}
    }
}