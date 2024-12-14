using System.Windows;
using System.Windows.Controls;
using WpfTest.Models;

namespace WpfTest
{
    /// <summary>
    /// Interaction logic for CustomerList.xaml
    /// </summary>
    public partial class CustomerList : UserControl
    {
        public CustomerList()
        {
            InitializeComponent();
            this.DataContext = new CustomerListViewModel();
        }

        private void AddCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            // Create a new instance of the AddProduct window
            AddCustomer addCustomerWindow = new AddCustomer();

            // Set the owner of the AddProduct window to the main window of the application
            Window mainWindow = Window.GetWindow(this); // Get the parent window of the UserControl
            addCustomerWindow.Owner = mainWindow;

            // Show the AddProduct window as a modal dialog and get the result
            bool? result = addCustomerWindow.ShowDialog();
        }

    }

    public class Customer
    {
        public int CustomerID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string MobileNo { get; set; }
        public string CustomerType { get; set; }
        public string Category { get; set; }
    }
}
