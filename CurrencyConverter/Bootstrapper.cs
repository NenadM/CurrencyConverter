using System.Windows;
using CurrencyConverter.Views;
using CurrencyConverter.ViewModels;

namespace CurrencyConverter
{
    public class Bootstrapper
    {
        public Window BootstrapApplication()
        {
            var currencyConverterViewModel = new CurrencyConverterViewModel();
            var mainWindow = new MainView(currencyConverterViewModel);

            return mainWindow;

            //mainWindow.Show();
        }
    }
}