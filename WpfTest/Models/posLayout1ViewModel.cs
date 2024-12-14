using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SQLite;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace WpfTest.Models
{
    public class posLayout1ViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<Category> Categories { get; set; }
        public ObservableCollection<Product> Products { get; set; }
        public ICommand FilterByCategoryCommand { get; set; }
        private string _selectedCategory;

        public posLayout1ViewModel()
        {
            Categories = new ObservableCollection<Category>();
            Products = new ObservableCollection<Product>();

            LoadCategories();
            LoadProducts(); // Initially load all products

            SelectedCategory = "All";

            FilterByCategoryCommand = new RelayCommand<string>(FilterProductsByCategory);

            // Set "All" as the default selected category
            SelectedCategory = "All";
        }

        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged(nameof(SelectedCategory));
                FilterProductsByCategory(_selectedCategory);
            }
        }

        private void LoadCategories()
        {
            using (var connection = new SQLiteConnection("Data Source=pos.db;Version=3;"))
            {
                connection.Open();
                var command = new SQLiteCommand("SELECT CategoryName FROM Category", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Categories.Add(new Category
                        {
                            Name = reader["CategoryName"].ToString()
                        });
                    }
                }
            }
        }

        private void LoadProducts(string category = null)
        {
            Products.Clear();

            using (var connection = new SQLiteConnection("Data Source=pos.db;Version=3;"))
            {
                connection.Open();
                string query = "SELECT Name, Price, ButtonImage FROM Products";
                if (!string.IsNullOrEmpty(category))
                {
                    query += " WHERE Category = @Category";
                }

                var command = new SQLiteCommand(query, connection);
                if (!string.IsNullOrEmpty(category))
                {
                    command.Parameters.AddWithValue("@Category", category);
                }

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Products.Add(new Product
                        {
                            Name = reader["Name"].ToString(),
                            Price = (double)Convert.ToDecimal(reader["Price"]),
                            ProductImg = LoadImage(reader["ButtonImage"] as byte[])
                        });
                    }
                }
            }
        }

        private void FilterProductsByCategory(string category)
        {
            if (category == "All")
            {
                LoadProducts(); // Load all products if "All" is selected
            }
            else
            {
                LoadProducts(category);
            }
        }

        private BitmapImage LoadImage(byte[] imageData)
        {
            if (imageData == null) return null;

            using (var stream = new System.IO.MemoryStream(imageData))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = stream;
                image.EndInit();
                return image;
            }
        }
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class EqualityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null && value.Equals(parameter) ? false : true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class Category
    {
        public string Name { get; set; }
    }
}