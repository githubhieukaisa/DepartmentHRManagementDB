using assignment.Models;
using assignment.utility;
using System.Reflection.Metadata;
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

namespace assignment
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ProjectManagementDbContext context = ProjectManagementDbContext.Instance;
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
            var user = context.Employees.FirstOrDefault(emp => emp.Email == email && emp.PasswordHash == password && emp.Status.Equals("Active"));
            if(user == null)
            {
                MessageBox.Show("Invalid email or password.");
                return;
            }
            if (user.RoleId == ConstantClass.ADMIN_ROLE_ID) // Assuming 1 is the role ID for Admin
            {
                AdminDashboard adminDashboard = new AdminDashboard(user);
                adminDashboard.Show();
                this.Close();
            }
            else
            {
                EmployeeDashboard employeeDashboard= new EmployeeDashboard(user);
                employeeDashboard.Show();
                this.Close();
            }
        }
    }
}