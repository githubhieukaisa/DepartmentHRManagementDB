using assignment.Models;
using assignment.Service;
using assignment.utility;
using Microsoft.EntityFrameworkCore;
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
using static assignment.utility.Validation;

namespace assignment.AdminViewModel
{
    /// <summary>
    /// Interaction logic for EmployeeManagementPage.xaml
    /// </summary>
    public partial class EmployeeManagementPage : UserControl
    {
        private ProjectManagementDbContext context = ProjectManagementDbContext.Instance;
        private Employee? _selectedEmployee=null;
        public EmployeeManagementPage()
        {
            InitializeComponent();
            resizeToOrigin();
            LoadEmployees();
        }

        private void LoadEmployees(string? key = null)
        {
            if (key == null || key.Equals(""))
            {
                dgEmployee.ItemsSource = context.Employees
                    .Include(e => e.Role)
                    .Include(e => e.Department)
                    .ToList();
            }
            else
            {
                dgEmployee.ItemsSource = context.Employees
                    .Include(e => e.Role)
                    .Include(e => e.Department)
                    .Where(e => e.FullName.Contains(key) || e.Email.Contains(key))
                    .ToList();
            }
        }

        private void AddEmployee_Click(object sender, RoutedEventArgs e)
        {
            setUpFormManagement();
            cmbRole.SelectedIndex= 0;
            cmbDepartment.SelectedIndex = 0;
        }

        private void EditEmployee_Click(object sender, RoutedEventArgs e)
        {
            var selectedEmployee = dgEmployee.SelectedItem as Employee;
            if(selectedEmployee == null)
            {
                MessageBox.Show("Please select an employee to edit.");
                return;
            }
            _selectedEmployee=selectedEmployee;
            setUpFormManagement("Edit Employee", selectedEmployee.FullName, selectedEmployee.Email,selectedEmployee.PasswordHash,selectedEmployee.RoleId,selectedEmployee.DepartmentId,"Save");
        }

        private void setUpFormManagement(string header="Add new Employee", string? fullname="", string? email="",string? password="", int? roleId=null, int? departmentId=null, string btnContent="Add")
        {
            var window = Window.GetWindow(this);
            if (window != null)
            {
                window.Height = 650;
            }

            formEmployeeManagement.Visibility = Visibility.Visible;
            var roles = context.Roles.Where(r => r.RoleId == 2).ToList();
            cmbRole.ItemsSource = roles;
            if(roleId.HasValue)
            {
                cmbRole.SelectedValue = roleId.Value;
            }
            else
            {
                cmbRole.SelectedIndex = 0;
            }

            var departments = context.Departments
                .Select(d => new {
                    d.DepartmentId,
                    d.DepartmentName
                })
                .ToList();
            cmbDepartment.ItemsSource = departments;
            if(departmentId.HasValue)
            {
                cmbDepartment.SelectedValue = departmentId.Value;
            }
            else
            {
                cmbDepartment.SelectedIndex = 0;
            }

            headerOfForm.Text = header;
            txtFullName.Text = fullname;
            txtEmail.Text = email;
            txtPassword.Text = password;
            btnManagement.Content = btnContent;
        }

        private void DeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            var selectedEmployee = dgEmployee.SelectedItem as Employee;
            if (selectedEmployee == null)
            {
                MessageBox.Show("Please select an employee to deactive.");
                return;
            }

            if(selectedEmployee.Status == "Inactive")
            {
                MessageBox.Show("This employee is already inactive.");
                return;
            }

            var team= context.Teams
                .FirstOrDefault(t=>t.TeamMembers.Any(tm => tm.EmployeeId == selectedEmployee.EmployeeId) && t.DoneAt == null);
            if (team != null)
            {
                MessageBox.Show($"This employee is a member of an active team ({team.TeamName}). Please remove them from the team before deactivating.");
                return;
            }

            if (MessageBox.Show($"Are you sure you want to deactive {selectedEmployee.FullName}?", "Confirm Deactivation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                selectedEmployee.Status = "Inactive";
                context.Employees.Update(selectedEmployee);
                context.SaveChanges();
                MessageBox.Show("Employee deactivated successfully.");
                LoadEmployees();
            }
        }

        private void ActiveEmployee_Click(object sender, RoutedEventArgs e)
        {
            var selectedEmployee = dgEmployee.SelectedItem as Employee;
            if (selectedEmployee == null)
            {
                MessageBox.Show("Please select an employee to activate.");
                return;
            }

            if (selectedEmployee.Status == "Active")
            {
                MessageBox.Show("This employee is already active.");
                return;
            }

            if (MessageBox.Show($"Are you sure you want to activate {selectedEmployee.FullName}?", "Confirm Activation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                selectedEmployee.Status = "Active";
                context.Employees.Update(selectedEmployee);
                context.SaveChanges();
                MessageBox.Show("Employee activated successfully.");
                LoadEmployees();
            }
        }

        private void btnManagement_Click(object sender, RoutedEventArgs e)
        {
            var errorList = new List<string>();
            string fullname;
            if(!Validate(txtFullName.Text.Trim(), s => CapitalizeEachWord(s), 
                new List<ValidationRule<string>>
                {
                    new ValidationRule<string>(s => s.Length >= 10,"Full Name không được ít hơn 10 ký tự"),
                    new ValidationRule<string>(s => s.Length <= 50,"Full Name không được quá 50 ký tự"),
                    new ValidationRule<string>(s => s.All(c => char.IsLetter(c) || c==' '),"Full Name chỉ được chứa chữ cái hoặc space")
                }, 
                out string errorMessage, out fullname, "Full Name"))
            {
                errorList.Add(errorMessage);
            }
            string email;
            if (!Validate(txtEmail.Text.Trim(), s => s, 
                new List<ValidationRule<string>>
                {
                    new ValidationRule<string>(s => s.Length >= 10,"Email không được ít hơn 10 ký tự"),
                    new ValidationRule<string>(s => s.Length <= 50,"Email không được quá 50 ký tự"),
                    new ValidationRule<string>(s => s.Contains("@") && s.Contains("."), "Email phải chứa '@' và '.'")
                }, 
                out errorMessage, out email, "Email"))
            {
                errorList.Add(errorMessage);
            }
            string password;
            if(!Validate(txtPassword.Text.Trim(), s => s, 
                new List<ValidationRule<string>>
                {
                    new ValidationRule<string>(s => s.Length >= 6,"Password không được ít hơn 6 ký tự"),
                    new ValidationRule<string>(s => s.Length <= 20,"Password không được quá 20 ký tự")
                }, 
                out errorMessage, out password, "Password"))
            {
                errorList.Add(errorMessage);
            }

            int roleId = (int)cmbRole.SelectedValue;
            int departmentId = (int)cmbDepartment.SelectedValue;
            if(errorList.Count > 0)
            {
                MessageBox.Show(string.Join("\n", errorList));
                return;
            }
            var service = new EmployeeManagementService(context);
            service.SaveEmployee(_selectedEmployee, btnManagement.Content.ToString(), fullname, email, password, roleId, departmentId, "Active");
            _selectedEmployee = null;
            formEmployeeManagement.Visibility = Visibility.Collapsed;
            resizeToOrigin();
            LoadEmployees();
        }

        private void search_Click(object sender, RoutedEventArgs e)
        {
            var key= txtSearch.Text.Trim();
            LoadEmployees(key);
        }

        private void resizeToOrigin()
        {
            var window = Window.GetWindow(this);
            if (window != null)
            {
                window.Height = 450;
            }
        }

    }
}
