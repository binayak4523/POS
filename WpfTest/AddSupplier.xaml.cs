using System;
using System.Collections.Generic;
using System.Data.SQLite;
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
using System.Windows.Shapes;

namespace WpfTest
{
    /// <summary>
    /// Interaction logic for AddSupplier.xaml
    /// </summary>
    public partial class AddSupplier : Window
    {
        public AddSupplier()
        {
            InitializeComponent();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve input from text boxes
            string supplierName = SuppliersTextBox.Text;
            string contactName = ContactTextBox.Text;
            string address = AddressTextBox.Text;
            string city = CityTextBox.Text;
            string postalCode = PostalTextBox.Text;
            string country = CountryTextBox.Text;
            string phone = PhoneTextBox.Text;

            // Define SQLite connection string
            string connectionString = "Data Source=pos.db;Version=3;";

            // SQL query to insert supplier data
            string insertQuery = "INSERT INTO Suppliers (SupplierName, ContactName, Address, City, PostalCode, Country, Phone) " +
                                 "VALUES (@SupplierName, @ContactName, @Address, @City, @PostalCode, @Country, @Phone);";

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
                    {
                        // Use parameters to prevent SQL injection
                        command.Parameters.AddWithValue("@SupplierName", supplierName);
                        command.Parameters.AddWithValue("@ContactName", contactName);
                        command.Parameters.AddWithValue("@Address", address);
                        command.Parameters.AddWithValue("@City", city);
                        command.Parameters.AddWithValue("@PostalCode", postalCode);
                        command.Parameters.AddWithValue("@Country", country);
                        command.Parameters.AddWithValue("@Phone", phone);

                        // Execute SQL command
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }

                MessageBox.Show("Supplier information saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close(); // Close the window after saving
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
