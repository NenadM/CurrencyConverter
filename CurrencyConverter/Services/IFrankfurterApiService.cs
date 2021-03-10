using System.Collections.Generic;
using System.Threading.Tasks;

namespace CurrencyConverter.Services
{
    public interface IFrankfurterApiService
    {
        Task<IEnumerable<CurrencyDto>> GetCurrenciesAsync();

        Task<CurrencyConversionDto> ConvertAsync(string amount, string from, string to);
    }
}