namespace CurrencyConverter.Services
{
    public class CurrencyDto
    {
        public CurrencyDto(string symbol, string fullName)
        {
            this.Symbol = symbol;
            this.FullName = fullName;
        }

        public string Symbol { get; }

        public string FullName { get; }
    }
}