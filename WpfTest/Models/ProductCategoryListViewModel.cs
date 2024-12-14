using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTest.Models
{
    public class ProductCategoryListViewModel
    {
        public ObservableCollection<ProductCategory> ProductCategory { get; set; }

        public ProductCategoryListViewModel()
        {
            ProductCategory = new ObservableCollection<ProductCategory>();
            LoadCategories();
        }

        private void LoadCategories()
        {
            string connectionString = "Data Source=pos.db;Version=3;";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT CategoryID, CategoryName FROM Category";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ProductCategory.Add(new ProductCategory
                        {
                            CategoryID = reader.GetInt32(0),
                            Name = reader.GetString(1)
                        });
                    }
                }
            }
        }
    }

    public class ProductCategory
    {
        public int CategoryID { get; set; }
        public string Name { get; set; }
    }
}
