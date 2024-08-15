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
    public partial class QuanLyKhach : Form
    {
        public QuanLyKhach()
        {
            InitializeComponent();
            LoadCustomerData();
            dataGridView1.SelectionChanged += DataGridView1_SelectionChanged;
        }

        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridView1.SelectedRows[0];
                guna2TextBox5.Text = selectedRow.Cells["Name"].Value.ToString();
                guna2TextBox1.Text = selectedRow.Cells["Phone"].Value.ToString();
                guna2TextBox2.Text = selectedRow.Cells["Address"].Value.ToString();
            }
        }



        private void LoadCustomerData()
        {
            using (var context = new ShoeStore2Entities4())
            {
                var customers = context.Customers.Select(cus => new
                {
                    cus.CustomerID,
                    cus.Name,
                    cus.Phone,
                    cus.Address
                }).ToList();

                dataGridView1.DataSource = customers;
            }
        }


        private void guna2Button8_Click_1(object sender, EventArgs e)
        {
            OptionsA optionsA = new OptionsA();
            optionsA.Show();
            optionsA.FormClosed += (s, args) => this.Close();
            this.Hide();
        }

        private void guna2Button2_Click_1(object sender, EventArgs e)
        {
            guna2TextBox5.Text = string.Empty;
            guna2TextBox1.Text = string.Empty;
            guna2TextBox2.Text = string.Empty;
            guna2TextBox7.Text = string.Empty;
        }

        private void guna2Button3_Click_1(object sender, EventArgs e)
        {
            string customerName = guna2TextBox5.Text;
            string phone = guna2TextBox1.Text;
            string address = guna2TextBox2.Text;

            if (string.IsNullOrEmpty(customerName) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(address))
            {
                MessageBox.Show("All fields are required.");
                return;
            }

            if (!Regex.IsMatch(customerName, @"^[a-zA-Z\s]+$"))
            {
                MessageBox.Show("Customer name must only contain alphabetic characters.");
                return;
            }

            if (!Regex.IsMatch(phone, @"^\d{10}$"))
            {
                MessageBox.Show("Phone number must be 10 digits.");
                return;
            }

            using (var context = new ShoeStore2Entities4())
            {
                var newCustomer = new Customer
                {
                    Name = customerName,
                    Phone = phone,
                    Address = address
                };

                context.Customers.Add(newCustomer);
                context.SaveChanges();
            }

            LoadCustomerData();
        }

        private void guna2Button4_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a customer to update.");
                return;
            }

            var selectedRow = dataGridView1.SelectedRows[0];
            int customerId = (int)selectedRow.Cells["CustomerID"].Value;

            string customerName = guna2TextBox5.Text;
            string phone = guna2TextBox1.Text;
            string address = guna2TextBox2.Text;

            if (string.IsNullOrEmpty(customerName) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(address))
            {
                MessageBox.Show("All fields are required.");
                return;
            }

            if (!Regex.IsMatch(customerName, @"^[a-zA-Z\s]+$"))
            {
                MessageBox.Show("Customer name must only contain alphabetic characters.");
                return;
            }

            if (!Regex.IsMatch(phone, @"^\d{10}$"))
            {
                MessageBox.Show("Phone number must be 10 digits.");
                return;
            }

            using (var context = new ShoeStore2Entities4())
            {
                var customer = context.Customers.Find(customerId);
                if (customer != null)
                {
                    customer.Name = customerName;
                    customer.Phone = phone;
                    customer.Address = address;
                    context.SaveChanges();
                }
            }

            LoadCustomerData();
        }

        private void guna2Button5_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a customer to delete.");
                return;
            }

            var selectedRow = dataGridView1.SelectedRows[0];
            int customerId = (int)selectedRow.Cells["CustomerID"].Value;

            var result = MessageBox.Show("Are you sure you want to delete this customer?", "Confirmation", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                using (var context = new ShoeStore2Entities4())
                {
                    var customer = context.Customers.Find(customerId);
                    if (customer != null)
                    {
                        context.Customers.Remove(customer);
                        context.SaveChanges();
                    }
                }

                LoadCustomerData();
            }
        }

        private void guna2Button1_Click_1(object sender, EventArgs e)
        {
            string searchValue = guna2TextBox7.Text;

            using (var context = new ShoeStore2Entities4())
            {
                var customers = context.Customers
                    .Where(cus => cus.Name.Contains(searchValue) || cus.Phone.Contains(searchValue))
                    .Select(cus => new
                    {
                        cus.CustomerID,
                        cus.Name,
                        cus.Phone,
                        cus.Address
                    }).ToList();

                dataGridView1.DataSource = customers;

                if (customers.Count == 0)
                {
                    MessageBox.Show("No customers found with the given name or phone number.");
                }
            }
        }
    }
}