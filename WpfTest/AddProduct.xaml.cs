using Microsoft.Win32;
using System;
using System.IO;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Controls;

namespace WpfTest
{
    /// <summary>
    /// Interaction logic for AddProduct.xaml
    /// </summary>
    public partial class AddProduct : Window
    {
        public ObservableCollection<Category> Categories { get; set; }
        public AddProduct()
        {
            InitializeComponent();
            LoadCategories();
            this.DataContext = this;
        }

        private void LoadCategories()
        {
            Categories = new ObservableCollection<Category>();
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
                        Categories.Add(new Category
                        {
                            CategoryID = reader.GetInt32(0),
                            CategoryName = reader.GetString(1)
                        });
                    }
                }
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string productName = ProductTextBox.Text;
            int stockQuantity;
            if (!int.TryParse(StockTextBox.Text, out stockQuantity))
            {
                MessageBox.Show("Please enter a valid stock quantity.");
                return;
            }
            string category = CategoryComboBox.Text;
            string buttonName = ButtonNameTextBox.Text;
            double price;
            if (!double.TryParse(PriceTextBox.Text, out price))
            {
                MessageBox.Show("Please enter a valid price.");
                return;
            }
            byte[] buttonImage = GetImageBytes();

            string connectionString = "Data Source=pos.db;Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Products (Name, Price, StockQuantity, Category, DateAdded, ButtonImage, Disabled) " +
                               "VALUES (@Name, @Price, @StockQuantity, @Category, @DateAdded, @ButtonImage, @Disabled)";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", productName);
                    command.Parameters.AddWithValue("@Price", price);
                    command.Parameters.AddWithValue("@StockQuantity", stockQuantity);
                    command.Parameters.AddWithValue("@Category", category);
                    command.Parameters.AddWithValue("@DateAdded", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    command.Parameters.AddWithValue("@ButtonImage", buttonImage);
                    command.Parameters.AddWithValue("@Disabled", 0); // Default value for Disabled column

                    command.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Product saved successfully!");
        }

        private byte[] GetImageBytes()
        {
            if (SelectedImageControl.Source == null)
                return null;

            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(SelectedImageControl.Source.ToString());
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();

            using (MemoryStream ms = new MemoryStream())
            {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmap));
                encoder.Save(ms);
                return ms.ToArray();
            }
        }

        private void SelectImageButton_Click(object sender, RoutedEventArgs e)
        {
            // Create an instance of OpenFileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Set filter options
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";

            // Show the dialog and get the selected file
            if (openFileDialog.ShowDialog() == true)
            {
                // Get the selected file's path
                string selectedFilePath = openFileDialog.FileName;

                // Load the image and display it in the Image control
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(selectedFilePath);
                bitmap.EndInit();

                // Assign the bitmap to the Image control
                SelectedImageControl.Source = bitmap;
            }
        }
    }
    public class Category
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
    }

}
