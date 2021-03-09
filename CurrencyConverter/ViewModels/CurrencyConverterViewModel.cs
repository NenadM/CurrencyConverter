using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Input;
using AsyncAwaitBestPractices.MVVM;
using CurrencyConverter.Services;

namespace CurrencyConverter.ViewModels
{
    public class CurrencyConverterViewModel : ViewModelBase
    {
        private readonly FrankfurterApiService frankfurterApiService;
        private CurrencyEditorViewModel currencyFrom;
        private CurrencyEditorViewModel currencyTo;

        public CurrencyConverterViewModel()
        {
            this.frankfurterApiService = new FrankfurterApiService();
            this.InitializeOnLoadCommand = new AsyncCommand(this.Initialize);
        }

        public ICommand InitializeOnLoadCommand { get; }

        public CurrencyEditorViewModel CurrencyFrom 
        {
            get 
            {
                return this.currencyFrom;
            }
            set 
            {
                if (this.currencyFrom == value)
                {
                    return;
                }

                this.currencyFrom = value;
                this.OnPropertyChanged(nameof(this.CurrencyFrom));
            }
        }

        public CurrencyEditorViewModel CurrencyTo
        {
            get
            {
                return this.currencyTo;
            }
            set
            {
                if (this.currencyTo == value)
                {
                    return;
                }

                this.currencyTo = value;
                this.OnPropertyChanged(nameof(this.CurrencyTo));
            }
        }

        private async Task Initialize()
        {
            var fromAmount = Settings.Default.FromAmount;
            var fromCurrency = Settings.Default.FromCurrency;
            var toCurrency = Settings.Default.ToCurrency;

            var currencies = await this.frankfurterApiService.GetCurrencies();
            var convertCurrencyFromCommand = new AsyncCommand(() => this.ConvertCurrency(this.currencyFrom, this.currencyTo));
            var convertCurrencyToCommand = new AsyncCommand(() => this.ConvertCurrency(this.currencyTo, this.currencyFrom));

            this.CurrencyFrom = new CurrencyEditorViewModel("From:", fromAmount, fromCurrency, currencies, convertCurrencyFromCommand, convertCurrencyFromCommand);
            this.CurrencyTo = new CurrencyEditorViewModel("To:", string.Empty, toCurrency, currencies, convertCurrencyToCommand, convertCurrencyFromCommand);

            await convertCurrencyFromCommand.ExecuteAsync();
        }

        private async Task ConvertCurrency(CurrencyEditorViewModel convertCurrencyFrom, CurrencyEditorViewModel convertCurrencyTo)
        {
            Settings.Default.FromAmount = this.currencyFrom.Amount;
            Settings.Default.FromCurrency = this.currencyFrom.Currency;
            Settings.Default.ToCurrency = this.currencyTo.Currency;
            Settings.Default.Save();

            if(string.IsNullOrEmpty(convertCurrencyFrom.Amount))
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
            var currencyConversion = await this.frankfurterApiService.Convert(convertCurrencyFrom.Amount, convertCurrencyFrom.Currency, currencyTo);

            var numberFormatInfo = new NumberFormatInfo { NumberDecimalSeparator = "." };
            convertCurrencyTo.Amount = currencyConversion.Rates[currencyTo].ToString(numberFormatInfo);
            
            return;
        }
    }
}