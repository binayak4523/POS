using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Element;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfTest.Models;

namespace WpfTest
{
    public partial class posLayout1 : Page
    {
        public posLayout1()
        {
            InitializeComponent();
            this.DataContext = new posLayout1ViewModel();
        }

        private void Product_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var product = ((FrameworkElement)sender).DataContext as Product;
            if (product != null)
            {
                var dialog = new ProductQuantityDialog(product.Price);
                if (dialog.ShowDialog() == true)
                {
                    int quantity = dialog.Quantity;
                    double price = dialog.Price;

                    // Add the product to the order list with the entered quantity and price
                    OrderList.Items.Add(new OrderItem
                    {
                        Designation = product.Name,
                        Price = (decimal)price,
                        Quantity = quantity
                    });
                }
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        private void BackofficeButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.Content = new BackOffice();
        }

        private void CheckoutButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the list of ordered items (this should already be populated based on your logic)
            var orderedItems = OrderList.Items.OfType<OrderItem>().ToList();

            if (orderedItems.Count > 0)
            {
                // Create the PDF
                CreateBillPdf(orderedItems);

                // Update product quantities in the database
                UpdateProductQuantities(orderedItems);

                // Clear the order list
                OrderList.Items.Clear();
            }
            else
            {
                MessageBox.Show("No items in the order list.");
            }
        }

        private void CreateBillPdf(List<OrderItem> orderedItems)
        {
            string pdfPath = "D:\\pos\\Bill.pdf"; // Adjust the path as needed

            // Fetch company details from the database
            Company company = GetCompanyDetails();

            using (var writer = new PdfWriter(pdfPath))
            {
                var pdfDocument = new PdfDocument(writer);

                // Setting the page size to resemble a receipt
                var pageSize = new PageSize(220, 800);  // Width is smaller to fit receipt style
                var document = new Document(pdfDocument, new iText.Kernel.Geom.PageSize(pageSize));

                // Add company details at the top
                document.Add(new Paragraph(company.StoreName).SetFontSize(12).SetBold().SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
                document.Add(new Paragraph(company.Location).SetFontSize(10).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
                document.Add(new Paragraph("VAT TIN: 72451285236").SetFontSize(10).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
                document.Add(new Paragraph(" ")); // Empty line for spacing

                // Add a line separator
                document.Add(new LineSeparator(new SolidLine()));

                // Add receipt details like Bill No, Date, Time, etc.
                var billDetailsTable = new Table(new float[] { 1, 1 }).UseAllAvailableWidth();
                billDetailsTable.SetBorder(iText.Layout.Borders.Border.NO_BORDER); // Make the table border disappear
                billDetailsTable.AddCell(new Cell().Add(new Paragraph("Bill No: 6").SetFontSize(10)).SetBorder(iText.Layout.Borders.Border.NO_BORDER));
                billDetailsTable.AddCell(new Cell().Add(new Paragraph("Time: " + DateTime.Now.ToString("HH:mm")).SetFontSize(10).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT)).SetBorder(iText.Layout.Borders.Border.NO_BORDER));
                billDetailsTable.AddCell(new Cell().Add(new Paragraph("Date: " + DateTime.Now.ToString("dd-MM-yyyy")).SetFontSize(10)).SetBorder(iText.Layout.Borders.Border.NO_BORDER));
                billDetailsTable.AddCell(new Cell().Add(new Paragraph("User: A").SetFontSize(10).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT)).SetBorder(iText.Layout.Borders.Border.NO_BORDER));
                document.Add(billDetailsTable);
                document.Add(new Paragraph(" ")); // Empty line for spacing
                
                // Add another line separator
                document.Add(new LineSeparator(new SolidLine()));

                // Add Item Table Header
                var itemsTable = new Table(new float[] { 3, 1, 1, 1 }).UseAllAvailableWidth();
                itemsTable.SetBorder(iText.Layout.Borders.Border.NO_BORDER); // Make the table border disappear
                itemsTable.AddHeaderCell(new Cell().Add(new Paragraph("Description").SetFontSize(10).SetBold()).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetBorder(iText.Layout.Borders.Border.NO_BORDER));
                itemsTable.AddHeaderCell(new Cell().Add(new Paragraph("Qty").SetFontSize(10).SetBold()).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetBorder(iText.Layout.Borders.Border.NO_BORDER));
                itemsTable.AddHeaderCell(new Cell().Add(new Paragraph("Rate").SetFontSize(10).SetBold()).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetBorder(iText.Layout.Borders.Border.NO_BORDER));
                itemsTable.AddHeaderCell(new Cell().Add(new Paragraph("Amount").SetFontSize(10).SetBold()).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetBorder(iText.Layout.Borders.Border.NO_BORDER));

                // Add items to the table
                foreach (var item in orderedItems)
                {
                    itemsTable.AddCell(new Cell().Add(new Paragraph(item.Designation).SetFontSize(10)).SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT).SetBorder(iText.Layout.Borders.Border.NO_BORDER));
                    itemsTable.AddCell(new Cell().Add(new Paragraph(item.Quantity.ToString()).SetFontSize(10).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)).SetBorder(iText.Layout.Borders.Border.NO_BORDER));
                    itemsTable.AddCell(new Cell().Add(new Paragraph(item.Price.ToString("C2")).SetFontSize(10).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)).SetBorder(iText.Layout.Borders.Border.NO_BORDER));
                    itemsTable.AddCell(new Cell().Add(new Paragraph((item.Quantity * item.Price).ToString("C2")).SetFontSize(10).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)).SetBorder(iText.Layout.Borders.Border.NO_BORDER));
                }

                // Calculate totals and add to the document
                document.Add(itemsTable);
                document.Add(new Paragraph(" ")); // Empty line for spacing

                // Add another line separator for the total
                document.Add(new LineSeparator(new SolidLine()));

                // Total amount with currency formatting
                float totalAmount = (float)orderedItems.Sum(i => i.Quantity * i.Price);
                document.Add(new Paragraph($"Total: {totalAmount:C2}").SetFontSize(12).SetBold().SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));

                // Additional payment details
                document.Add(new Paragraph("Cash Tendered: 600.00").SetFontSize(10).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
                document.Add(new Paragraph("Balance: 28.00").SetFontSize(10).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));

                // Footer with a thank you message
                document.Add(new LineSeparator(new SolidLine()));
                document.Add(new Paragraph("Thank You").SetFontSize(10).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
                document.Add(new Paragraph("Visit Again").SetFontSize(10).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));

                document.Close();
            }

            MessageBox.Show($"Bill generated at {pdfPath}");
        }

        // Method to get company details from the database
        private Company GetCompanyDetails()
        {
            Company company = new Company();

            // Sample code to retrieve data from SQLite database
            string connectionString = "Data Source=pos.db;Version=3;"; // Adjust path as needed

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT StoreName, Location FROM Company WHERE CompanyID = 1"; // Example: fetching the first company
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            company.StoreName = reader["StoreName"].ToString();
                            company.Location = reader["Location"].ToString();
                        }
                    }
                }
            }
            return company;
        }

        private void UpdateProductQuantities(List<OrderItem> orderedItems)
        {
            using (var connection = new SQLiteConnection("Data Source=pos.db;Version=3;"))
            {
                connection.Open();

                foreach (var item in orderedItems)
                {
                    var command = new SQLiteCommand("UPDATE Products SET StockQuantity = StockQuantity - @Quantity WHERE Name = @ProductName", connection);
                    command.Parameters.AddWithValue("@Quantity", item.Quantity);
                    command.Parameters.AddWithValue("@ProductName", item.Designation);

                    command.ExecuteNonQuery();
                }
            }
        }
    }

    public class OrderItem
    {
        public string Designation { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }

}
