using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfTest
{
    /// <summary>
    /// Interaction logic for ProductQuantityDialog.xaml
    /// </summary>
    public partial class ProductQuantityDialog : Window
    {
        public int Quantity { get; set; }
        public double Price { get; set; }

        public ProductQuantityDialog(double defaultPrice)
        {
            InitializeComponent();
            PriceTextBox.Text = defaultPrice.ToString();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(QuantityTextBox.Text, out int quantity) && double.TryParse(PriceTextBox.Text, out double price))
            {
                Quantity = quantity;
                Price = price;
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Please enter valid quantity and price.");
            }
        }

        internal async Task ShowAsync()
        {
            throw new NotImplementedException();
        }
    }
}
