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
using System.Data.SQLite;
using System.IO;
using System.Data;
using System.Net.Http;
using Newtonsoft.Json;

namespace WpfTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string databasePath = "pos.db";
        private string Role { get; set; }
        private Button _selectedButton;

        public MainWindow()
        {
            InitializeComponent();
            CheckAndCreateDatabase();
            CheckRegistration();
        }
        /* Check the online database if returned OK then Registered else Demo */
        private void CheckRegistration()
        {
            App.RegistrationType = "Demo";
        }

        private void CheckAndCreateDatabase()
        {
            // Check if the database file exists
            if (!File.Exists(databasePath))
            {
                // Create the database if it doesn't exist
                SQLiteConnection.CreateFile(databasePath);
                MessageBox.Show("Database created successfully!");

                // Create tables and initial setup
                using (var connection = new SQLiteConnection($"Data Source={databasePath};Version=3;"))
                {
                    connection.Open();

                    string createTableQuery = @"
                        CREATE TABLE IF NOT EXISTS Users (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Username TEXT NOT NULL,
                            Password TEXT NOT NULL,
                            Role TEXT NOT NULL);
                        CREATE TABLE IF NOT EXISTS Category (
                            CategoryID INTEGER PRIMARY KEY AUTOINCREMENT,
                            CategoryName TEXT NOT NULL);
                        CREATE TABLE IF NOT EXISTS Users (
                            CompanyID INTEGER PRIMARY KEY AUTOINCREMENT,
                            StoreName TEXT NOT NULL,
                            Location TEXT NOT NULL,
                            Language TEXT NOT NULL,
                            Currency TEXT NOT NULL,
                            Role TEXT NOT NULL);
                        ";

                    using (var command = new SQLiteCommand(createTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Tables created successfully!");
                }
            }
        }
        // This code will execute when the Enter Button is clicked
        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            string password = PasswordBox.Password;

            if (string.IsNullOrEmpty(Role) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please select a user role and enter a password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // This will send the Role and Password for Authentication
            bool isAuthenticated = AuthenticateUser(Role, password);

            if (isAuthenticated)
            {
                MessageBox.Show("Login successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                App.UserRole = Role;
                posLayout1 posPage = new posLayout1();  // Assuming PosPage is the class for pos.xaml
                this.Content = posPage; // Replace current content with pos.xaml content
            }
            else
            {
                MessageBox.Show("Invalid username or password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // This will check if the username and password is correct or not
        private bool AuthenticateUser(string role, string password)
        {
            string connectionString = "Data Source=pos.db;Version=3;";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(1) FROM Users WHERE Role = @Username AND Password = @Password";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", role);
                    command.Parameters.AddWithValue("@Password", password);

                    int result = Convert.ToInt32(command.ExecuteScalar());
                    return result > 0;
                }
            }
        }

        // Keypad button operations
        private void KeypadButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                PasswordBox.Password += button.Content.ToString();
            }
        }

        // This code clears the password if someone miss typed something
        private void ClearEntry_Click(object sender, RoutedEventArgs e)
        {
            PasswordBox.Password = string.Empty;
        }

        // This code closes the applaction
        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            // Close the application or navigate away
            Application.Current.Shutdown();
        }

        //This code stores the selected user
        private void UserRoleButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                Role = button.Content.ToString();
                Button clickedButton = sender as Button;

                // Optionally, show the selected username somewhere in the UI
                //MessageBox.Show($"Selected User Role: {Role}", "User Role", MessageBoxButton.OK, MessageBoxImage.Information);

                // Reset previous selected button style
                if (_selectedButton != null)
                {
                    _selectedButton.ClearValue(Button.BackgroundProperty);
                    _selectedButton.ClearValue(Button.ForegroundProperty);
                }

                // Highlight the selected button
                if (clickedButton != null)
                {
                    _selectedButton = clickedButton;
                    _selectedButton.Background = new SolidColorBrush(Colors.OrangeRed); // Set the highlight color
                    _selectedButton.Foreground=new SolidColorBrush(Colors.White);
                }
            }
        }
    }
}
