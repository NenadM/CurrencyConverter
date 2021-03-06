using System.Windows;

namespace CurrencyConverter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var bootstrapper = new Bootstrapper();
            var mainWindow = bootstrapper.BootstrapApplication();
            mainWindow.Show();

            base.OnStartup(e);
        }
    }
}