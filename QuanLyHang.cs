using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Du_an__1
{
    public partial class QuanLyHang : Form
    {
        public QuanLyHang()
        {
            InitializeComponent();
            LoadProductData();
            dataGridView1.SelectionChanged += DataGridView1_SelectionChanged;
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            // Clear the text boxes
            guna2TextBox5.Text = string.Empty;
            guna2TextBox1.Text = string.Empty;
            guna2TextBox2.Text = string.Empty;
            guna2TextBox3.Text = string.Empty;
            guna2TextBox4.Text = string.Empty; // Size
            guna2TextBox6.Text = string.Empty; // Color
            guna2TextBox7.Text = string.Empty;
        }

        private void LoadProductData()
        {
            using (var context = new ShoeStore2Entities4())
            {
                var products = context.Products.Select(prod => new
                {
                    prod.ProductID,
                    prod.ProductName,
                    prod.Descriptions,
                    prod.Price,
                    prod.Stock,
                    prod.Size,
                    prod.Color
                }).ToList();

                dataGridView1.DataSource = products;
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            string productName = guna2TextBox5.Text;
            string descriptions = guna2TextBox1.Text;
            if (!decimal.TryParse(guna2TextBox2.Text, out decimal price))
            {
                MessageBox.Show("Invalid price. It should only contain numbers.");
                return;
            }
            if (!int.TryParse(guna2TextBox3.Text, out int stock))
            {
                MessageBox.Show("Invalid stock. It should only contain numbers.");
                return;
            }
            string size = guna2TextBox4.Text;
            string color = guna2TextBox6.Text;

            // Validate input
            if (string.IsNullOrEmpty(productName) || string.IsNullOrEmpty(descriptions) || string.IsNullOrEmpty(size) || string.IsNullOrEmpty(color))
            {
                MessageBox.Show("All fields are required.");
                return;
            }

            if (!Regex.IsMatch(productName, @"^[a-zA-Z\s]+$"))
            {
                MessageBox.Show("Product name can only contain letters and spaces.");
                return;
            }

            if (!Regex.IsMatch(color, @"^[a-zA-Z\s]+$"))
            {
                MessageBox.Show("Color can only contain letters and spaces.");
                return;
            }

            if (!Regex.IsMatch(size, @"^\d+$"))
            {
                MessageBox.Show("Size should only contain numbers.");
                return;
            }

            using (var context = new ShoeStore2Entities4())
            {
                var newProduct = new Product
                {
                    ProductName = productName,
                    Descriptions = descriptions,
                    Price = price,
                    Stock = stock,
                    Size = size,
                    Color = color
                };

                context.Products.Add(newProduct);
                context.SaveChanges();
            }

            // Refresh DataGridView
            LoadProductData();
        }


        private void guna2Button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a product to update.");
                return;
            }

            var selectedRow = dataGridView1.SelectedRows[0];
            int productId = (int)selectedRow.Cells["ProductID"].Value;

            string productName = guna2TextBox5.Text;
            string descriptions = guna2TextBox1.Text;
            if (!decimal.TryParse(guna2TextBox2.Text, out decimal price))
            {
                MessageBox.Show("Invalid price. It should only contain numbers.");
                return;
            }
            if (!int.TryParse(guna2TextBox3.Text, out int stock))
            {
                MessageBox.Show("Invalid stock. It should only contain numbers.");
                return;
            }
            string size = guna2TextBox4.Text;
            string color = guna2TextBox6.Text;

            // Validate input
            if (string.IsNullOrEmpty(productName) || string.IsNullOrEmpty(descriptions) || string.IsNullOrEmpty(size) || string.IsNullOrEmpty(color))
            {
                MessageBox.Show("All fields are required.");
                return;
            }

            if (!Regex.IsMatch(productName, @"^[a-zA-Z\s]+$"))
            {
                MessageBox.Show("Product name can only contain letters and spaces.");
                return;
            }

            if (!Regex.IsMatch(color, @"^[a-zA-Z\s]+$"))
            {
                MessageBox.Show("Color can only contain letters and spaces.");
                return;
            }

            if (!Regex.IsMatch(size, @"^\d+$"))
            {
                MessageBox.Show("Size should only contain numbers.");
                return;
            }

            using (var context = new ShoeStore2Entities4())
            {
                var product = context.Products.Find(productId);
                if (product != null)
                {
                    product.ProductName = productName;
                    product.Descriptions = descriptions;
                    product.Price = price;
                    product.Stock = stock;
                    product.Size = size;
                    product.Color = color;
                    context.SaveChanges();
                }
            }

            // Refresh DataGridView
            LoadProductData();
        }


        private void guna2Button5_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a product to delete.");
                return;
            }

            var selectedRow = dataGridView1.SelectedRows[0];
            int productId = (int)selectedRow.Cells["ProductID"].Value;

            var result = MessageBox.Show("Are you sure you want to delete this product?", "Confirmation", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                using (var context = new ShoeStore2Entities4())
                {
                    var product = context.Products.Find(productId);
                    if (product != null)
                    {
                        context.Products.Remove(product);
                        context.SaveChanges();
                    }
                }


                LoadProductData();
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string searchValue = guna2TextBox7.Text;

            using (var context = new ShoeStore2Entities4())
            {
                var products = context.Products
                    .Where(prod => prod.ProductName.Contains(searchValue))
                    .Select(prod => new
                    {
                        prod.ProductID,
                        prod.ProductName,
                        prod.Descriptions,
                        prod.Price,
                        prod.Stock,
                        prod.Size,
                        prod.Color
                    }).ToList();

                dataGridView1.DataSource = products;

                if (products.Count == 0)
                {
                    MessageBox.Show("No products found with the given name.");
                }
            }
        }

        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridView1.SelectedRows[0];
                guna2TextBox5.Text = selectedRow.Cells["ProductName"].Value.ToString();
                guna2TextBox1.Text = selectedRow.Cells["Descriptions"].Value.ToString();
                guna2TextBox2.Text = selectedRow.Cells["Price"].Value.ToString();
                guna2TextBox3.Text = selectedRow.Cells["Stock"].Value.ToString();
                guna2TextBox4.Text = selectedRow.Cells["Size"].Value.ToString(); 
                guna2TextBox6.Text = selectedRow.Cells["Color"].Value.ToString(); 
            }
        }

        private void guna2Button8_Click(object sender, EventArgs e)
        {
            OptionsA optionsA = new OptionsA();
            optionsA.Show();
            optionsA.FormClosed += (s, args) => this.Close();
            this.Hide();
        }
    }
}