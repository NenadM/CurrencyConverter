﻿using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Input;
using AsyncAwaitBestPractices.MVVM;
using CurrencyConverter.Services;

namespace CurrencyConverter.ViewModels
{
    public class CurrencyConverterViewModel : ViewModelBase
    {
        private readonly FrankfurterApiService frankfurterApiService;

        public CurrencyConverterViewModel()
        {
            this.frankfurterApiService = new FrankfurterApiService();
            this.InitializeOnLoadCommand = new AsyncCommand(this.Initialize);

            var appSettings = Settings.Default;

            var convertCurrencyFromCommand = new AsyncCommand(() => this.ConvertCurrency(this.CurrencyFrom, this.CurrencyTo));
            var convertCurrencyToCommand = new AsyncCommand(() => this.ConvertCurrency(this.CurrencyTo, this.CurrencyFrom));

            this.CurrencyFrom = new CurrencyEditorViewModel("From:", appSettings.FromAmount, appSettings.FromCurrency, convertCurrencyFromCommand, convertCurrencyFromCommand);
            this.CurrencyTo = new CurrencyEditorViewModel("To:", string.Empty, appSettings.ToCurrency, convertCurrencyToCommand, convertCurrencyFromCommand);

        }

        public ICommand InitializeOnLoadCommand { get; }

        public CurrencyEditorViewModel CurrencyFrom { get; }

        public CurrencyEditorViewModel CurrencyTo { get; }

        private async Task Initialize()
        {
            var currencies = await this.frankfurterApiService.GetCurrencies();
            this.CurrencyFrom.Currencies = currencies;
            this.CurrencyTo.Currencies = currencies;
            await ((AsyncCommand)this.CurrencyFrom.ConvertOnAmountCommand).ExecuteAsync();
        }

        private async Task ConvertCurrency(CurrencyEditorViewModel convertCurrencyFrom, CurrencyEditorViewModel convertCurrencyTo)
        {
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
            var currencyConversion = await this.frankfurterApiService.Convert(convertCurrencyFrom.Amount, convertCurrencyFrom.Currency, currencyTo);

            var numberFormatInfo = new NumberFormatInfo { NumberDecimalSeparator = "." };
            convertCurrencyTo.Amount = currencyConversion.Rates[currencyTo].ToString(numberFormatInfo);
            
            return;
        }

        private void SaveSettings()
        {
            Settings.Default.FromAmount = this.CurrencyFrom.Amount;
            Settings.Default.FromCurrency = this.CurrencyFrom.Currency;
            Settings.Default.ToCurrency = this.CurrencyTo.Currency;
            Settings.Default.Save();
        }
    }
}