using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTest.Models
{
    public class CustomerListViewModel
    {
        public ObservableCollection<Customer> Customers { get; set; }

        public CustomerListViewModel()
        {
            Customers = new ObservableCollection<Customer>();
            LoadCustomers();
        }

        private void LoadCustomers()
        {
            string connectionString = "Data Source=pos.db;Version=3;";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT CustomerID, CustomerName, Address, CustomerType, MobileNo FROM Customers";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Customers.Add(new Customer
                        {
                            CustomerID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Address = reader.GetString(2),
                            MobileNo = reader.GetString(3),
                            CustomerType = reader.GetString(4),
                        });
                    }
                }
            }
        }
    }

}
