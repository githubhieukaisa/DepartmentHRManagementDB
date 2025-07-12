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
using assignment.Models;
using Microsoft.EntityFrameworkCore;

namespace assignment
{
    /// <summary>
    /// Interaction logic for AdminDashboard.xaml
    /// </summary>
    public partial class AdminDashboard : Window
    {
        private DepartmentHrmanagementDbContext context = new DepartmentHrmanagementDbContext();
        public AdminDashboard()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            cbDepartment.ItemsSource = context.Departments.ToList();
            dgEmployee.ItemsSource = context.Employees
                  .Include(e => e.Department)
                  .Include(e => e.Role)
                  .ToList();
        }

        private void cbDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var department = cbDepartment.SelectedItem as Department;

            if (department != null)
            {
                tbDescription.Text = department.Description ?? string.Empty;
                tbManager.Text = department.Manager?.FullName ?? string.Empty;
                dgEmployee.ItemsSource = context.Employees
                    .Where(emp => emp.DepartmentId == department.DepartmentId)
                    .Include(emp => emp.Role)
                    .ToList();
            }
            else
            {
                tbDescription.Text = string.Empty;
                tbManager.Text = string.Empty;
                dgEmployee.ItemsSource = null;
            }
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void btnManageEmployees_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnManageDepartments_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnManageTeams_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnManageProjects_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnManageTasks_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
