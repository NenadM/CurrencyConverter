using System.Windows;

namespace CurrencyConverter.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView(object mainViewDataContext)
        {            
            InitializeComponent();
            this.DataContext = mainViewDataContext;
        }
    }
}