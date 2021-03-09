using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace CurrencyConverter
{
    public class MaskedTextBox : TextBox
    {
        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[0-9]+(\\.[0-9]{0,4})?$");

            e.Handled = !regex.IsMatch(Text.Insert(SelectionStart, e.Text));

            base.OnPreviewTextInput(e);
        }
    }
}