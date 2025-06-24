using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using assignment.Models;

namespace assignment
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DepartmentHrmanagementDbContext context= new DepartmentHrmanagementDbContext();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string email = tbEmail.Text;
            string password = pbPassword.Password;
            if(string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both email and password.");
                return;
            }
            var user = context.Employees.FirstOrDefault(emp => emp.Email == email && emp.PasswordHash == password);
            if(user == null)
            {
                MessageBox.Show("Invalid email or password.");
                return;
            }
            if (user.RoleId == 1) // Assuming 1 is the role ID for Admin
            {
                AdminDashboard adminDashboard = new AdminDashboard();
                adminDashboard.Show();
                this.Close();
            }
            else if (user.RoleId == 2) // Assuming 2 is the role ID for Department Manager
            {
                ManagerDashboard managerDashboard = new ManagerDashboard();
                managerDashboard.Show();
                this.Close();
            }
            else  // Assuming 3 is the role ID for Employee
            {
                EmployeeDashboard employeeDashboard= new EmployeeDashboard();
                employeeDashboard.Show();
                this.Close();
            }
        }
    }
}