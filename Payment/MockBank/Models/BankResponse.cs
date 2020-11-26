using System.ComponentModel.DataAnnotations;

namespace MockBank.Models{
    public class BankResponse{
        [Required]
        public System.Guid Id {get; set;}

        // Status will map to our status enum {2 = Success, 3 = Failure}
        public int Status {get; set;}
        
    }
}