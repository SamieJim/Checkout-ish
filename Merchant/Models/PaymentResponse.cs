using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Merchant.Models
{
    
    [JsonObject]
    public class PaymentResponse
    {
        [Required]
        [JsonProperty]
        public string CardNumber { get; set; }
    
        [Required]
        [JsonProperty]
        public Status Status { get; set; }

        [Required]
        [JsonProperty]
        public string Currency = "£";

        [Required]
        [JsonProperty]
        public double Amount { get; set; } = 0.0;

    }
}