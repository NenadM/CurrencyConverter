using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CurrencyConverter.Services
{
    public class CurrencyConversionDto
    {
        [JsonPropertyName("base")]
        public string Base { get; set; }
        
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }
        
        [JsonPropertyName("rates")]
        public Dictionary<string, decimal> Rates { get; set; }
    }
}