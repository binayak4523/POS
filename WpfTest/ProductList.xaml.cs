using System.Collections.ObjectModel;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WpfTest.Models;

namespace WpfTest
{
    /// <summary>
    /// Interaction logic for ProductList.xaml
    /// </summary>
    public partial class ProductList : UserControl
    {
        public ProductList()
        {
            InitializeComponent();
            DataContext = new ProductListViewModel();  // Set DataContext to the ViewModel instance
        }

        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            // Create a new instance of the AddProduct window
            AddProduct addProductWindow = new AddProduct();

            // Set the owner of the AddProduct window to the main window of the application
            Window mainWindow = Window.GetWindow(this); // Get the parent window of the UserControl
            addProductWindow.Owner = mainWindow;

            // Show the AddProduct window as a modal dialog and get the result
            bool? result = addProductWindow.ShowDialog();
        }
    }

    public class Product
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int StockQuantity { get; set; }
        public string Category { get; set; }
        public string DateAdded { get; set; }
        public BitmapImage ProductImg { get; set; }
        public string ImagePath { get; internal set; }
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Predicate<T> _canExecute;

        public RelayCommand(Action<T> execute, Predicate<T> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}