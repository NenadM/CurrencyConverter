using System.Windows;
using CurrencyConverter.Views;
using CurrencyConverter.ViewModels;
using CurrencyConverter.Services;

namespace CurrencyConverter
{
    public class Bootstrapper
    {
        public Window BootstrapApplication()
        {
            var frankfurterApiService = new FrankfurterApiService();
            var applicationSettingsService = new ApplicationSettingsService();
            var currencyConverterViewModel = new CurrencyConverterViewModel(frankfurterApiService, applicationSettingsService);
            var mainWindow = new MainView { DataContext = currencyConverterViewModel };

            return mainWindow;
        }
    }
}