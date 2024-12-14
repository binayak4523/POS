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
    /// Interaction logic for AddProductCategory.xaml
    /// </summary>
    public partial class AddProductCategory : Window
    {
        public AddProductCategory()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Fetch the category name from the TextBox
            string categoryName = CategoryTextBox.Text.Trim();

            // Input validation
            if (string.IsNullOrEmpty(categoryName))
            {
                MessageBox.Show("Category Name is required.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Define the connection string (adjust the path as needed)
            string connectionString = "Data Source=pos.db;Version=3;";

            // Insert the category into the database
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string insertQuery = "INSERT INTO Category (CategoryName) VALUES (@CategoryName)";

                using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@CategoryName", categoryName);

                    try
                    {
                        command.ExecuteNonQuery();
                        MessageBox.Show("Category saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.DialogResult = true; // Close the window after saving
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred while saving the category: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
