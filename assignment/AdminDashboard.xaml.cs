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
using assignment.AdminViewModel;
using assignment.Models;
using Microsoft.EntityFrameworkCore;

namespace assignment
{
    /// <summary>
    /// Interaction logic for AdminDashboard.xaml
    /// </summary>
    public partial class AdminDashboard : Window
    {
        private Employee? _selectedEmployee = null;
        public AdminDashboard(Employee? selectedEmployee)
        {
            InitializeComponent();
            LoadInitialContent();
            _selectedEmployee = selectedEmployee;
        }

        private void LoadInitialContent()
        {
            tabControl.SelectedItem = tabControl.Items[1];
            MainContent.Content = new EmployeeManagementPage();
        }

        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tabControl.SelectedItem is TabItem selectedTab)
            {
                switch (selectedTab.Header.ToString())
                {
                    case "Quản lý phòng ban":
                        MainContent.Content = new DepartmentManagementPage();
                        break;
                    case "Quản lý nhân viên":
                        MainContent.Content = new EmployeeManagementPage();
                        break;
                    case "Quản lý đội nhóm":
                        MainContent.Content = new TeamManagementPage();
                        break;
                    case "Quản lý dự án":
                        MainContent.Content = new ProjectManagementPage();
                        break;
                    case "Quản lý nhiệm vụ":
                        MainContent.Content = new TaskManagementPage();
                        break;
                }
            }
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
