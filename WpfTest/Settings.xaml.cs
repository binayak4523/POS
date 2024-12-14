using System.Windows.Controls;
using WpfTest.Models;

namespace WpfTest
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : UserControl
    {
        public Settings()
        {
            InitializeComponent();
            this.DataContext = new SettingsViewModel();

            CurrentPasswordBox.PasswordChanged += (s, e) =>
            {
                var viewModel = (SettingsViewModel)this.DataContext;
                viewModel.Company.CurrentPassword = CurrentPasswordBox.Password;
            };
        }
    }
}
