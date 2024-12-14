using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Windows;

namespace WpfTest
{
    public partial class AddCustomer : Window
    {
        public ObservableCollection<Category> Categories { get; set; }

        public AddCustomer()
        {
            InitializeComponent();
            LoadCategories(); // Load categories from the database
            DataContext = this; // Set DataContext so XAML can bind to it
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void LoadCategories()
        {
            Categories = new ObservableCollection<Category>();
            string connectionString = "Data Source=pos.db;Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT CategoryID, CategoryName FROM CustomerCategory";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Categories.Add(new Category
                        {
                            CategoryID = reader.GetInt32(0),
                            CategoryName = reader.GetString(1)
                        });
                    }
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Fetch data from input fields
            string customerName = CustomerTextBox.Text.Trim();
            string address = AddressTextBox.Text.Trim();
            string customerType = CustomerTypeComboBox.SelectedValue?.ToString();
            string mobileNo = MobileTextBox.Text.Trim();

            // Input validation
            if (string.IsNullOrEmpty(customerName))
            {
                MessageBox.Show("Customer Name is required.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Define the connection string (adjust the path as needed)
            string connectionString = "Data Source=pos.db;Version=3;";

            // Insert the data into the database
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string insertQuery = "INSERT INTO customers (CustomerName, Address, CustomerType, MobileNo) VALUES (@CustomerName, @Address, @CustomerType, @MobileNo)";

                using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@CustomerName", customerName);
                    command.Parameters.AddWithValue("@Address", address);
                    command.Parameters.AddWithValue("@CustomerType", customerType);
                    command.Parameters.AddWithValue("@MobileNo", mobileNo);

                    try
                    {
                        command.ExecuteNonQuery();
                        MessageBox.Show("Customer saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close(); // Close the window after saving
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred while saving the customer: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}
