namespace CurrencyConverter.Services
{
    public class ApplicationSettings
    {
        public ApplicationSettings(string fromAmount, string fromCurrency, string toCurrency)
        {
            this.FromAmount = fromAmount;
            this.FromCurrency = fromCurrency;
            this.ToCurrency = toCurrency;
        }

        public string FromAmount { get; }

        public string FromCurrency { get; }

        public string ToCurrency { get; }
    }
}