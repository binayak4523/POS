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
using WpfTest.Models;

namespace WpfTest
{
    /// <summary>
    /// Interaction logic for Suppliers.xaml
    /// </summary>
    public partial class Suppliers : UserControl
    {
        public Suppliers()
        {
            InitializeComponent();
            DataContext = new SupplierListViewModel();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // Create a new instance of the AddProduct window
            AddSupplier addSupplierWindow = new AddSupplier();

            // Set the owner of the AddProduct window to the main window of the application
            Window mainWindow = Window.GetWindow(this); // Get the parent window of the UserControl
            addSupplierWindow.Owner = mainWindow;

            // Show the AddProduct window as a modal dialog and get the result
            bool? result = addSupplierWindow.ShowDialog();
        }
    }

    public class Supplier
    {
        public int SupplierID { get; set; }
        public string SupplierName { get; set; }
        public string ContactName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string PhoneNo { get; set; }
    }
}
