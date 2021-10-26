using CurrencyConverter.Services;
using System.Collections.Generic;
using System.Windows.Input;

namespace CurrencyConverter.ViewModels
{
    public class CurrencyEditorViewModel : ViewModelBase
    {
        private string amount;
        private string currency;
        private IEnumerable<CurrencyDto> currencies;

        public CurrencyEditorViewModel(ICommand convertOnAmountCommand, ICommand convertOnCurrencyCommand)
        {
            this.ConvertOnAmountCommand = convertOnAmountCommand;
            this.ConvertOnCurrencyCommand = convertOnCurrencyCommand;
        }

        public ICommand ConvertOnAmountCommand { get; }

        public ICommand ConvertOnCurrencyCommand { get; }

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

        public IEnumerable<CurrencyDto> Currencies
        {
            get
            {
                return this.currencies;
            }
            set
            {
                if (this.currencies == value)
                {
                    return;
                }

                this.currencies = value;
                this.OnPropertyChanged(nameof(this.Currencies));
            }
        }
    }
}