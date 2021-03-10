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
            var currencyConverterViewModel = new CurrencyConverterViewModel(frankfurterApiService);
            var mainWindow = new MainView { DataContext = currencyConverterViewModel };

            return mainWindow;
        }
    }
}