namespace CurrencyConverter.Services
{
    public class ApplicationSettingsService : IApplicationSettingsService
    {
        public ApplicationSettings Load()
        {
            var applicationSettings = new ApplicationSettings(
                Settings.Default.FromAmount,
                Settings.Default.FromCurrency,
                Settings.Default.ToCurrency);

            return applicationSettings;
        }

        public void Save(ApplicationSettings applicationSettings)
        {
            Settings.Default.FromAmount = applicationSettings.FromAmount;
            Settings.Default.FromCurrency = applicationSettings.FromCurrency;
            Settings.Default.ToCurrency = applicationSettings.ToCurrency;
            Settings.Default.Save();
        }
    }
}