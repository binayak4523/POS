using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows;
using WpfTest;

public class ProductListViewModel
{
    public ObservableCollection<Product> Products { get; set; }
    public ICommand DeleteCommand { get; }
    public ICommand EditCommand { get; }

    public ProductListViewModel()
    {
        Products = new ObservableCollection<Product>();
        LoadProducts();

        DeleteCommand = new RelayCommand<Product>(DeleteProduct);
        EditCommand = new RelayCommand<Product>(EditProduct);
    }

    private void LoadProducts()
    {
        string connectionString = "Data Source=pos.db;Version=3;";
        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT ProductID, Name, Price, StockQuantity, Category, DateAdded, ButtonImage FROM Products WHERE Disabled = 0";

            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Products.Add(new Product
                    {
                        ProductID = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Price = reader.GetDouble(2),
                        StockQuantity = reader.GetInt32(3),
                        Category = reader.GetString(4),
                        DateAdded = reader.GetString(5),
                        ProductImg = LoadImage(reader["ButtonImage"] as byte[])
                    });
                }
            }
        }
    }
    private void EditProduct(Product product)
    {
        // Create a new instance of the AddProduct window
        AddProduct addProductWindow = new AddProduct();

        // Set the DataContext of the AddProduct window to the selected product
        addProductWindow.DataContext = product;

        // Show the AddProduct window as a modal dialog
        bool? result = addProductWindow.ShowDialog();
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

    private void DeleteProduct(Product product)
    {
        var result = MessageBox.Show($"Are you sure you want to delete {product.Name}?", "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
        if (result == MessageBoxResult.Yes)
        {
            string connectionString = "Data Source=pos.db;Version=3;";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Products WHERE ProductID = @ProductID";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductID", product.ProductID);
                    command.ExecuteNonQuery();
                }
            }
            Products.Remove(product);
        }
    }
}