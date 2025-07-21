using assignment.EmployeeUseControl;
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
using System.Windows.Shapes;

namespace assignment
{
    /// <summary>
    /// Interaction logic for EmployeeDashboard.xaml
    /// </summary>
    public partial class EmployeeDashboard : Window
    {
        private Employee? _selectedEmployee = null;
        public EmployeeDashboard(Employee? selectedEmployee)
        {
            InitializeComponent();
            _selectedEmployee = selectedEmployee;
            LoadInitialContent();
        }

        private void LoadInitialContent()
        {
            tabControl.SelectedItem = tabControl.Items[0];
            MainContent.Content = new EmployeeProfile(_selectedEmployee);
        }

        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(tabControl.SelectedItem is TabItem selectedTab)
            {
                switch (selectedTab.Header.ToString())
                {
                    case "Profile":
                        MainContent.Content = new EmployeeProfile(_selectedEmployee);
                        break;
                    case "Project":
                        MainContent.Content = new EmployeeProject(_selectedEmployee);
                        break;
                    case "Task":
                        MainContent.Content = new EmployeeTask(_selectedEmployee);
                        break;
                    case "Team":
                        MainContent.Content = new EmployeeTeam(_selectedEmployee);
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
