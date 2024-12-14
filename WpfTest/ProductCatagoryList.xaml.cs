using System.Windows;
using System.Windows.Controls;
using WpfTest.Models;

namespace WpfTest
{
    /// <summary>
    /// Interaction logic for ProductCatagoryList.xaml
    /// </summary>
    public partial class ProductCatagoryList : UserControl
    {
        public ProductCatagoryList()
        {
            InitializeComponent();
            this.DataContext = new ProductCategoryListViewModel();
        }

        private void AddCatagoryButton_Click(object sender, RoutedEventArgs e)
        {
            // Create a new instance of the AddProduct window
            AddProductCategory AddProductCategoryWindow = new AddProductCategory();

            // Set the owner of the AddProduct window to the main window of the application
            Window mainWindow = Window.GetWindow(this); // Get the parent window of the UserControl
            AddProductCategoryWindow.Owner = mainWindow;

            // Show the AddProduct window as a modal dialog and get the result
            bool? result = AddProductCategoryWindow.ShowDialog();
        }
        private void EditCategory(ProductCategory category)
        {
            // Edit category logic (e.g., open an edit dialog)
        }

        private void DeleteCategory(ProductCategory category)
        {
            // Delete category logic (e.g., remove from database and collection)
        }
    }
}
