using assignment.Models;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace assignment.AdminViewModel
{
    /// <summary>
    /// Interaction logic for DepartmentManagementPage.xaml
    /// </summary>
    public partial class DepartmentManagementPage : UserControl
    {
        private readonly ProjectManagementDbContext context = ProjectManagementDbContext.Instance;
        private Department? _selectedDepartment = null;

        public DepartmentManagementPage()
        {
            InitializeComponent();
            resizeToOrigin();
            LoadDepartments();
        }

        private void LoadDepartments(string? keyword = null)
        {
            var departments = context.Departments.AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                departments = departments.Where(d => d.DepartmentName.Contains(keyword));
            }

            dgDepartment.ItemsSource = departments.ToList();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            _selectedDepartment = null;
            setUpForm("Add new Department", "", "", "Add");
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            var department = dgDepartment.SelectedItem as Department;
            if (department == null)
            {
                MessageBox.Show("Please select a department to edit.");
                return;
            }

            _selectedDepartment = department;
            setUpForm("Edit Department", department.DepartmentName, department.Description, "Save");
        }

        private void btnSaveDepartment_Click(object sender, RoutedEventArgs e)
        {
            string name = txtDepartmentName.Text.Trim();
            string desc = txtDescription.Text.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Department name is required.");
                return;
            }

            if (_selectedDepartment == null)
            {
                // Add new
                var department = new Department
                {
                    DepartmentName = name,
                    Description = desc
                };
                context.Departments.Add(department);
                context.SaveChanges();
                MessageBox.Show("Department added.");
            }
            else
            {
                // Update
                _selectedDepartment.DepartmentName = name;
                _selectedDepartment.Description = desc;
                context.Departments.Update(_selectedDepartment);
                context.SaveChanges();
                MessageBox.Show("Department updated.");
            }

            formDepartmentManagement.Visibility = Visibility.Collapsed;
            _selectedDepartment = null;
            resizeToOrigin();
            LoadDepartments();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            LoadDepartments(keyword);
        }

        private void setUpForm(string header, string name, string description, string buttonLabel)
        {
            formDepartmentManagement.Visibility = Visibility.Visible;
            headerOfForm.Text = header;
            txtDepartmentName.Text = name;
            txtDescription.Text = description;
            btnSaveDepartment.Content = buttonLabel;

            var window = Window.GetWindow(this);
            if (window != null)
            {
                window.Height = 550;
            }
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
