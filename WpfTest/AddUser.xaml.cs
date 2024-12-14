using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Windows;

namespace WpfTest
{
    /// <summary>
    /// Interaction logic for AddUser.xaml
    /// </summary>
    public partial class AddUser : Window
    {
        public ObservableCollection<Role> Roles { get; set; }
        public AddUser()
        {
            InitializeComponent();
        }
        private void LoadCategories()
        {
            Roles = new ObservableCollection<Role>();
            string connectionString = "Data Source=pos.db;Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT RoleID, RoleName FROM UserRole";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Roles.Add(new Role
                        {
                            RoleID = reader.GetInt32(0),
                            RoleName = reader.GetString(1)
                        });
                    }
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Get user input
            string userName = UserTextBox.Text;
            string staffName = StaffTextBox.Text;
            string address = AddressTextBox.Text;
            Role selectedRole = RoleComboBox.SelectedItem as Role;
            string salaryText = SalaryTextBox.Text;
            string password = PasswordTextBox.Text;

            // Validate inputs
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password) || selectedRole == null)
            {
                MessageBox.Show("Username, Password, and Role are required.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!decimal.TryParse(salaryText, out decimal salary))
            {
                MessageBox.Show("Please enter a valid salary.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Insert into SQLite
            string connectionString = "Data Source=pos.db;Version=3;";
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO Users (UserName, StaffName, Address, RoleID, Salary, Password) VALUES (@UserName, @StaffName, @Address, @RoleID, @Salary, @Password)";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserName", userName);
                        cmd.Parameters.AddWithValue("@StaffName", staffName);
                        cmd.Parameters.AddWithValue("@Address", address);
                        cmd.Parameters.AddWithValue("@RoleID", selectedRole.RoleID); // Using the selected role ID
                        cmd.Parameters.AddWithValue("@Salary", salary);
                        cmd.Parameters.AddWithValue("@Password", password);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("User added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.DialogResult = true; // Close window after saving
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
    public class Role
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }
    }
}
