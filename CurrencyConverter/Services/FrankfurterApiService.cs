using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

namespace CurrencyConverter.Services
{
    public class FrankfurterApiService
    {
        public async Task<IEnumerable<CurrencyDto>> GetCurrencies()
        {
            var client = new HttpClient();

            var body = await this.CallWebService("https://api.frankfurter.app/currencies");

            var result = JsonSerializer.Deserialize<Dictionary<string, string>>(body)
                .Select(kvp => new CurrencyDto(kvp.Key, kvp.Value));

            return result;
        }

        public async Task<CurrencyConversionDto> Convert(string amount, string from, string to)
        {
            var uri = $"https://api.frankfurter.app/latest?from={from}&to={to}&amount={amount}";
            var body = await this.CallWebService(uri);
            var result = JsonSerializer.Deserialize<CurrencyConversionDto>(body);

            return result;
        }

        private async Task<string> CallWebService(string uri)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(uri),
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();

                return body;
            }
        }
    }
}