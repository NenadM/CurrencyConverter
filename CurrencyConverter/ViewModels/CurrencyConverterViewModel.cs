using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Input;
using AsyncAwaitBestPractices.MVVM;
using CurrencyConverter.Services;

namespace CurrencyConverter.ViewModels
{
    public class CurrencyConverterViewModel : ViewModelBase
    {
        private readonly IFrankfurterApiService frankfurterApiService;
        private readonly IApplicationSettingsService applicationSettingsService;
        private bool suspendConvertion = false;

        public CurrencyConverterViewModel(IFrankfurterApiService frankfurterApiService, IApplicationSettingsService applicationSettingsService)
        {
            this.frankfurterApiService = frankfurterApiService;
            this.applicationSettingsService = applicationSettingsService;
            this.InitializeOnLoadCommand = new AsyncCommand(this.Initialize);

            var convertCurrencyFromCommand = new AsyncCommand(() => this.ConvertCurrencyAsync(this.CurrencyFrom, this.CurrencyTo));
            var convertCurrencyToCommand = new AsyncCommand(() => this.ConvertCurrencyAsync(this.CurrencyTo, this.CurrencyFrom));

            this.CurrencyFrom = new CurrencyEditorViewModel(this.applicationSettingsService.FromAmount, this.applicationSettingsService.FromCurrency, convertCurrencyFromCommand, convertCurrencyFromCommand);
            this.CurrencyTo = new CurrencyEditorViewModel(string.Empty, this.applicationSettingsService.ToCurrency, convertCurrencyToCommand, convertCurrencyFromCommand);
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
            var currencies = await this.frankfurterApiService.GetCurrenciesAsync();
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
            var currencyConversion = await this.frankfurterApiService.ConvertAsync(convertCurrencyFrom.Amount, convertCurrencyFrom.Currency, currencyTo);

            var numberFormatInfo = new NumberFormatInfo { NumberDecimalSeparator = "." };
            convertCurrencyTo.Amount = currencyConversion.Rates[currencyTo].ToString(numberFormatInfo);
            
            return;
        }

        private void SaveSettings()
        {
            this.applicationSettingsService.FromAmount = this.CurrencyFrom.Amount;
            this.applicationSettingsService.FromCurrency = this.CurrencyFrom.Currency;
            this.applicationSettingsService.ToCurrency = this.CurrencyTo.Currency;
            this.applicationSettingsService.Save();
        }
    }
}