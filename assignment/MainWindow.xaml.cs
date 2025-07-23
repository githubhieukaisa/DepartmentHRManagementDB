using assignment.Models;
using assignment.Service;
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
            var loginService = new LoginService(context);

            try
            {
                var user = loginService.Login(email, password);

                Window dashboard = user.RoleId == ConstantClass.ADMIN_ROLE_ID
                    ? new AdminDashboard(user)
                    : new EmployeeDashboard(user);

                dashboard.Show();
                this.Close();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Invalid email or password.");
            }
        }
    }
}