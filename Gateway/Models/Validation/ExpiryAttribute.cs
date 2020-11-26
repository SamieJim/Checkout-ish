using System;
using System.ComponentModel.DataAnnotations;

namespace Gateway.Models {
    public class ExpiryAttribute : ValidationAttribute
    {

        public string GetErrorMessage() =>
            $"Your card has expired.";

        protected override ValidationResult IsValid(object value,
            ValidationContext validationContext)
        {
            var card = (Card)validationContext.ObjectInstance;
            var expiry = (DateTime) value;

            if (expiry <= DateTime.Now)
            {
                return new ValidationResult(GetErrorMessage());
            }

            return ValidationResult.Success;
        }
    }
}