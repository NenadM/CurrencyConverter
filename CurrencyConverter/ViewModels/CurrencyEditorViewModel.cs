using CurrencyConverter.Services;
using System.Collections.Generic;
using System.Windows.Input;

namespace CurrencyConverter.ViewModels
{
    public class CurrencyEditorViewModel : ViewModelBase
    {
        private string amount;
        private string currency;

        public CurrencyEditorViewModel(string label, string amount, string currency, IEnumerable<CurrencyDto> currencies, ICommand convertOnAmountCommand, ICommand convertOnCurrencyCommand)
        {
            this.Label = label;
            this.Currencies = currencies;
            this.amount = amount;
            this.currency = currency;
            this.ConvertOnAmountCommand = convertOnAmountCommand;
            this.ConvertOnCurrencyCommand = convertOnCurrencyCommand;
        }

        public ICommand ConvertOnAmountCommand { get; }

        public ICommand ConvertOnCurrencyCommand { get; }

        public IEnumerable<CurrencyDto> Currencies { get; set; }

        public string Label { get; }

        public string Amount
        {
            get
            {
                return this.amount;
            }
            set
            {
                if (this.amount == value)
                {
                    return;
                }

                this.amount = value;
                this.OnPropertyChanged(nameof(this.Amount));
            }
        }

        public string Currency
        {
            get
            {
                return this.currency;
            }
            set
            {
                if (this.currency == value)
                {
                    return;
                }

                this.currency = value;
                this.OnPropertyChanged(nameof(this.Currency));
            }
        }
    }
}