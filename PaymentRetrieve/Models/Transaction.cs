using System.ComponentModel.DataAnnotations;

namespace PaymentRetrieve.Models
{
    public class Transaction
    {
        [Key]
        [Required]
        public System.Guid Id { get; set;}

        [Required]
        public string CardNumber { get; set; }

        [Required]
        public string Currency = System.Globalization.NumberFormatInfo.CurrentInfo.CurrencySymbol;
    
        [Required]
        public double Amount { get; set; } = 0.0;

        [Required]
        public Status Status { get; set; } = Status.Requested;

        public void MaskCard(){
            this.CardNumber = string.Concat(
                "".PadLeft(12, '*'), 
                CardNumber.Substring(CardNumber.Length - 4)
                );
        }
    }
}