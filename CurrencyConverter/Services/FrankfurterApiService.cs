using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;

namespace CurrencyConverter.Services
{
    public class FrankfurterApiService : IFrankfurterApiService
    {
        private readonly HttpClient httpClient = new HttpClient();

        public async Task<IEnumerable<CurrencyDto>> GetCurrenciesAsync()
        {
            var result = await this.httpClient.GetFromJsonAsync<Dictionary<string, string>>("https://api.frankfurter.app/currencies");
            return result.Select(kvp => new CurrencyDto(kvp.Key, kvp.Value));
        }

        public async Task<CurrencyConversionDto> ConvertAsync(string amount, string from, string to)
        {
            return await this.httpClient.GetFromJsonAsync<CurrencyConversionDto>($"https://api.frankfurter.app/latest?from={from}&to={to}&amount={amount}");
        }
    }
}