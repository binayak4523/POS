using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace WpfTest.Models
{
    public class SupplierListViewModel
    {
        public ObservableCollection<Supplier> Suppliers { get; set; }
        public ICommand DeleteCommand { get; }
        public ICommand EditCommand { get; }

        public SupplierListViewModel()
        {
            Suppliers = new ObservableCollection<Supplier>();
            LoadSupplier();

            DeleteCommand = new RelayCommand<Supplier>(DeleteSupplier);
            EditCommand = new RelayCommand<Supplier>(EditSupplier);
        }

        private void LoadSupplier()
        {
            string connectionString = "Data Source=pos.db;Version=3;";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT SupplierID, SupplierName, ContactName, Address, City, PostalCode, Country, Phone FROM Suppliers";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Suppliers.Add(new Supplier
                        {
                            SupplierID = reader.GetInt32(0),
                            SupplierName = reader.GetString(1),
                            ContactName = reader.GetString(2),
                            Address = reader.GetString(3),
                            City = reader.GetString(4),
                            PostalCode = reader.GetString(5),
                            Country = reader.GetString(6),
                            PhoneNo = reader.GetString(7)// Changed to match the REAL type in the database
                        });
                    }
                }
            }
        }

        private void EditSupplier(Supplier supplier)
        {
            // Implement edit logic here
        }

        private void DeleteSupplier(Supplier supplier)
        {
            var result = MessageBox.Show($"Are you sure you want to delete {supplier.SupplierName}?", "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                string connectionString = "Data Source=pos.db;Version=3;";
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM Suppliers WHERE Id = @SupplierID";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@SuppliersID", supplier.SupplierID);
                        command.ExecuteNonQuery();
                    }
                }
                Suppliers.Remove(supplier);
            }
        }
    }
}
