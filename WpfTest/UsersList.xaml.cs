using System.Windows;
using System.Windows.Controls;
using WpfTest.Models;

namespace WpfTest
{
    /// <summary>
    /// Interaction logic for UsersList.xaml
    /// </summary>
    public partial class UsersList : UserControl
    {
        public UsersList()
        {
            InitializeComponent();
            DataContext = new UserListViewModel(); // Corrected instantiation of the ViewModel
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // Create a new instance of the AddProduct window
            AddUser addUserWindow = new AddUser();

            // Set the owner of the AddProduct window to the main window of the application
            Window mainWindow = Window.GetWindow(this); // Get the parent window of the UserControl
            addUserWindow.Owner = mainWindow;

            // Show the AddProduct window as a modal dialog and get the result
            bool? result = addUserWindow.ShowDialog();
        }
    }

    public class User
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string StaffName { get; set; }
        public string Address { get; set; }
        public string Role { get; set; }
        public double Salary { get; set; }
    }
}