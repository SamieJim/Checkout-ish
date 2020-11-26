using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Gateway.Models
{
    
    [JsonObject]
    public class PaymentResponse
    {
        [Required]
        [JsonProperty]
        public Guid Id { get; set; }
    
        [Required]
        [JsonProperty]
        public Status Status { get; set; } = 0.0;
    }
}