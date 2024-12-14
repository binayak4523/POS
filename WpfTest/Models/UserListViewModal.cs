using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Input;

namespace WpfTest
{
    public class UserListViewModel
    {
        public ObservableCollection<User> Users { get; set; }
        public ICommand DeleteCommand { get; }
        public ICommand EditCommand { get; }

        public UserListViewModel()
        {
            Users = new ObservableCollection<User>();
            LoadUsers();

            DeleteCommand = new RelayCommand<User>(DeleteUser);
            EditCommand = new RelayCommand<User>(EditUser);
        }

        private void LoadUsers()
        {
            string connectionString = "Data Source=pos.db;Version=3;";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Id, UserName, StaffName, Address, Role, Salary FROM Users";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Users.Add(new User
                        {
                            UserID = reader.GetInt32(0),
                            UserName = reader.GetString(1),
                            StaffName = reader.GetString(2),
                            Address = reader.GetString(3),
                            Role = reader.GetString(4),
                            Salary = reader.GetDouble(5) // Changed to GetDouble to match the REAL type in the database
                        });
                    }
                }
            }
        }

        private void EditUser(User user)
        {
            // Implement edit logic here
        }

        private void DeleteUser(User user)
        {
            var result = MessageBox.Show($"Are you sure you want to delete {user.UserName}?", "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                string connectionString = "Data Source=pos.db;Version=3;";
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM Users WHERE Id = @UserID";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", user.UserID);
                        command.ExecuteNonQuery();
                    }
                }
                Users.Remove(user);
            }
        }
    }
}
