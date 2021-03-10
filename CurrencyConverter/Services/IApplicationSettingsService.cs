namespace CurrencyConverter.Services
{
    public interface IApplicationSettingsService
    {
        string FromAmount { get; set; }

        string FromCurrency { get; set; }

        string ToCurrency { get; set; }

        void Save();
    }
}