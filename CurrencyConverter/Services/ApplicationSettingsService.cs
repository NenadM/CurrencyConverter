namespace CurrencyConverter.Services
{
    public class ApplicationSettingsService : IApplicationSettingsService
    {
        public ApplicationSettingsService()
        {
            this.FromAmount = Settings.Default.FromAmount;
            this.FromCurrency = Settings.Default.FromCurrency;
            this.ToCurrency = Settings.Default.ToCurrency;
        }

        public string FromAmount { get; set; }

        public string FromCurrency { get; set; }

        public string ToCurrency { get; set; }

        public void Save()
        {
            Settings.Default.FromAmount = this.FromAmount;
            Settings.Default.FromCurrency = this.FromCurrency;
            Settings.Default.ToCurrency = this.ToCurrency;
            Settings.Default.Save();
        }
    }
}