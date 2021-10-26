namespace CurrencyConverter.Services
{
    public interface IApplicationSettingsService
    {
        ApplicationSettings Load();

        void Save(ApplicationSettings applicationSettings);
    }
}