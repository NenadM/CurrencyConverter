using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Input;
using AsyncAwaitBestPractices.MVVM;
using CurrencyConverter.Services;

namespace CurrencyConverter.ViewModels
{
    public class CurrencyConverterViewModel : ViewModelBase
    {
        private readonly ICurrencyConverterApiService currencyConverterApiService;
        private readonly IApplicationSettingsService applicationSettingsService;
        private bool suspendConvertion = false;

        public CurrencyConverterViewModel(ICurrencyConverterApiService currencyConverterApiService, IApplicationSettingsService applicationSettingsService)
        {
            this.currencyConverterApiService = currencyConverterApiService;
            this.applicationSettingsService = applicationSettingsService;
            this.InitializeOnLoadCommand = new AsyncCommand(this.Initialize);

            var convertCurrencyFromCommand = new AsyncCommand(() => this.ConvertCurrencyAsync(this.CurrencyFrom, this.CurrencyTo));
            var convertCurrencyToCommand = new AsyncCommand(() => this.ConvertCurrencyAsync(this.CurrencyTo, this.CurrencyFrom));

            this.CurrencyFrom = new CurrencyEditorViewModel(convertCurrencyFromCommand, convertCurrencyFromCommand);
            this.CurrencyTo = new CurrencyEditorViewModel(convertCurrencyToCommand, convertCurrencyFromCommand);
        }

        public ICommand InitializeOnLoadCommand { get; }

        public ICommand SwitchConversionCommand { get; }

        public CurrencyEditorViewModel CurrencyFrom { get; }

        public CurrencyEditorViewModel CurrencyTo { get; }

        public void SwitchConversion()
        {
            this.suspendConvertion = true;
            var tempCurrency = this.CurrencyTo.Currency;
            this.CurrencyTo.Currency = this.CurrencyFrom.Currency;
            this.suspendConvertion = false;
            this.CurrencyFrom.Currency = tempCurrency;
        }

        private async Task Initialize()
        {
            var applicationSettings = this.applicationSettingsService.Load();
            
            this.CurrencyFrom.Amount = applicationSettings.FromAmount;
            this.CurrencyFrom.Currency = applicationSettings.FromCurrency;
            this.CurrencyTo.Currency= applicationSettings.ToCurrency;

            var currencies = await this.currencyConverterApiService.GetCurrenciesAsync();
            this.CurrencyFrom.Currencies = currencies;
            this.CurrencyTo.Currencies = currencies;
            await ((AsyncCommand)this.CurrencyFrom.ConvertOnAmountCommand).ExecuteAsync();
        }

        private async Task ConvertCurrencyAsync(CurrencyEditorViewModel convertCurrencyFrom, CurrencyEditorViewModel convertCurrencyTo)
        {
            if (this.suspendConvertion)
            {
                return;
            }

            this.SaveSettings();

            if (string.IsNullOrEmpty(convertCurrencyFrom.Amount))
            {
                convertCurrencyTo.Amount = string.Empty;

                return;
            }

            if (convertCurrencyFrom.Currency == convertCurrencyTo.Currency)
            {
                convertCurrencyTo.Amount = convertCurrencyFrom.Amount;

                return;
            }

            var currencyTo = convertCurrencyTo.Currency;
            var currencyConversion = await this.currencyConverterApiService.ConvertAsync(convertCurrencyFrom.Amount, convertCurrencyFrom.Currency, currencyTo);

            var numberFormatInfo = new NumberFormatInfo { NumberDecimalSeparator = "." };
            convertCurrencyTo.Amount = currencyConversion.Rates[currencyTo].ToString(numberFormatInfo);
            
            return;
        }

        private void SaveSettings()
        {
            ApplicationSettings applicationSettings = new(this.CurrencyFrom.Amount, this.CurrencyFrom.Currency, this.CurrencyTo.Currency);
            
            this.applicationSettingsService.Save(applicationSettings);
        }
    }
}