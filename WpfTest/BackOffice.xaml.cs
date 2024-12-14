using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfTest
{
    /// <summary>
    /// Interaction logic for BackOffice.xaml
    /// </summary>
    public partial class BackOffice : Page
    {
        public BackOffice()
        {
            InitializeComponent();

        }
        private void BackOffice_Unloaded(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
        private void POSButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.Content = new posLayout1();
        }
        private void Products_Click(object sender, RoutedEventArgs e)
        {
            Multi.Children.Clear();

            ProductList Pages = new ProductList();
            Multi.Children.Add(Pages);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Multi.Children.Clear();

            Settings Page = new Settings();
            Multi.Children.Add(Page);
        }

        private void UserButton_Click(object sender, RoutedEventArgs e)
        {
            Multi.Children.Clear();

            UsersList Page = new UsersList();
            Multi.Children.Add(Page);
        }

        private void CustomerButton_Click(object sender, RoutedEventArgs e)
        {
            Multi.Children.Clear();

            CustomerList Page = new CustomerList();
            Multi.Children.Add(Page);
        }
        private void PCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            Multi.Children.Clear();

            ProductCatagoryList Page = new ProductCatagoryList();
            Multi.Children.Add(Page);
        }
        private void SupplierButton_Click(object sender, RoutedEventArgs e)
        {
            Multi.Children.Clear();

            Suppliers Page = new Suppliers();
            Multi.Children.Add(Page);
        }
        private void StockButton_Click(object sender, RoutedEventArgs e)
        {
            Multi.Children.Clear();

            StockEntry Page = new StockEntry();
            Multi.Children.Add(Page);
        }
    }
}
