using Guna.UI2.WinForms;
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
    public partial class QuanLyNV : Form
    {
        public QuanLyNV()
        {
            InitializeComponent();
            LoadEmployeeData();
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;



        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {

            guna2TextBox1.Text = string.Empty;
            guna2TextBox2.Text = string.Empty;
            guna2TextBox3.Text = string.Empty;
            guna2TextBox5.Text = string.Empty;
            guna2TextBox6.Text = string.Empty;
            guna2TextBox7.Text = string.Empty;
        }

        private void LoadEmployeeData()
        {
            using (var context = new ShoeStore2Entities4())
            {
                var employees = context.Employees.Select(emp => new
                {
                    emp.EmployeeID,
                    emp.EmployeeName,
                    emp.EUserName,
                    emp.Email,
                    emp.EPhone,
                    emp.EHireDate
                }).ToList();

                dataGridView1.DataSource = employees;
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridView1.SelectedRows[0];

                guna2TextBox7.Text = selectedRow.Cells["EmployeeID"].Value.ToString();
                guna2TextBox5.Text = selectedRow.Cells["EmployeeName"].Value.ToString();
                guna2TextBox1.Text = selectedRow.Cells["EUserName"].Value.ToString();
                guna2TextBox2.Text = selectedRow.Cells["Email"].Value.ToString();
                guna2TextBox3.Text = selectedRow.Cells["EPhone"].Value.ToString();
                guna2DateTimePicker1.Value = Convert.ToDateTime(selectedRow.Cells["EHireDate"].Value);

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
            guna2TextBox1.Text = string.Empty;
            guna2TextBox2.Text = string.Empty;
            guna2TextBox3.Text = string.Empty;
            guna2TextBox5.Text = string.Empty;
            guna2TextBox6.Text = string.Empty;
            guna2TextBox7.Text = string.Empty;
        }

        private void guna2Button3_Click_1(object sender, EventArgs e)
        {
            string employeeName = guna2TextBox5.Text;
            string username = guna2TextBox1.Text;
            string email = guna2TextBox2.Text;
            string phone = guna2TextBox3.Text;
            DateTime hireDate = guna2DateTimePicker1.Value;
            string password = guna2TextBox6.Text;

            if (string.IsNullOrEmpty(employeeName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("All fields are required.");
                return;
            }

            if (!Regex.IsMatch(employeeName, @"^[a-zA-Z\s]+$"))
            {
                MessageBox.Show("Employee name can only contain letters and spaces.");
                return;
            }

            if (!email.Contains("@"))
            {
                MessageBox.Show("Email must contain '@'.");
                return;
            }

            if (!Regex.IsMatch(phone, @"^\d{10}$"))
            {
                MessageBox.Show("Phone number must be 10 digits.");
                return;
            }

            using (var context = new ShoeStore2Entities4())
            {
                var newEmployee = new Employee
                {
                    EmployeeName = employeeName,
                    EUserName = username,
                    Email = email,
                    EPhone = phone,
                    EHireDate = hireDate,
                    EPassword = password
                };

                context.Employees.Add(newEmployee);
                context.SaveChanges();
            }

            LoadEmployeeData();
            MessageBox.Show("Employee added successfully.");
        }


        private void guna2Button4_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an employee to update.");
                return;
            }

            var selectedRow = dataGridView1.SelectedRows[0];
            int employeeId = (int)selectedRow.Cells["EmployeeID"].Value;

            string employeeName = guna2TextBox5.Text;
            string username = guna2TextBox1.Text;
            string email = guna2TextBox2.Text;
            string phone = guna2TextBox3.Text;
            DateTime hireDate = guna2DateTimePicker1.Value;
            string password = guna2TextBox6.Text;

            if (string.IsNullOrEmpty(employeeName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("All fields are required.");
                return;
            }

            if (!Regex.IsMatch(employeeName, @"^[a-zA-Z\s]+$"))
            {
                MessageBox.Show("Employee name can only contain letters and spaces.");
                return;
            }

            if (!email.Contains("@"))
            {
                MessageBox.Show("Email must contain '@'.");
                return;
            }

            if (!Regex.IsMatch(phone, @"^\d{10}$"))
            {
                MessageBox.Show("Phone number must be 10 digits.");
                return;
            }

            using (var context = new ShoeStore2Entities4())
            {
                var employee = context.Employees.Find(employeeId);
                if (employee != null)
                {
                    employee.EmployeeName = employeeName;
                    employee.EUserName = username;
                    employee.Email = email;
                    employee.EPhone = phone;
                    employee.EHireDate = hireDate;
                    employee.EPassword = password;
                    context.SaveChanges();
                }
            }

            LoadEmployeeData();
            MessageBox.Show("Employee updated successfully.");
        }


        private void guna2Button5_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an employee to delete.");
                return;
            }

            var selectedRow = dataGridView1.SelectedRows[0];
            int employeeId = (int)selectedRow.Cells["EmployeeID"].Value;

            var result = MessageBox.Show("Are you sure you want to delete this employee?", "Confirmation", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                using (var context = new ShoeStore2Entities4())
                {
                    var employee = context.Employees.Find(employeeId);
                    if (employee != null)
                    {
                        context.Employees.Remove(employee);
                        context.SaveChanges();
                    }
                }

                LoadEmployeeData();
                MessageBox.Show("Employee deleted successfully.");
            }
        }

        private void guna2Button1_Click_1(object sender, EventArgs e)
        {
            string searchValue = guna2TextBox7.Text;

            using (var context = new ShoeStore2Entities4())
            {
                var employees = context.Employees
                    .Where(emp => emp.EmployeeName.Contains(searchValue) || emp.EUserName.Contains(searchValue))
                    .Select(emp => new
                    {
                        emp.EmployeeID,
                        emp.EmployeeName,
                        emp.EUserName,
                        emp.Email,
                        emp.EPhone,
                        emp.EHireDate
                    }).ToList();

                dataGridView1.DataSource = employees;

                if (employees.Count == 0)
                {
                    MessageBox.Show("No employees found with the given name or username.");
                }
            }
        }
    }
}