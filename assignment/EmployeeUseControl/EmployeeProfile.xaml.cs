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

namespace assignment.EmployeeUseControl
{
    /// <summary>
    /// Interaction logic for EmployeeProfile.xaml
    /// </summary>
    public partial class EmployeeProfile : UserControl
    {
        private ProjectManagementDbContext context = new ProjectManagementDbContext();
        private Employee? _selectedEmployee = null;
        public EmployeeProfile(Employee? selectedEmployee)
        {
            InitializeComponent();
            _selectedEmployee = selectedEmployee;
            _selectedEmployee.Department = context.Departments.FirstOrDefault(d => d.DepartmentId == _selectedEmployee.DepartmentId);
            _selectedEmployee.Role = context.Roles.FirstOrDefault(r => r.RoleId == _selectedEmployee.RoleId);
            LoadProfile();
        }

        private void LoadProfile()
        {
            txtFullName.Text = _selectedEmployee.FullName;
            txtEmail.Text = _selectedEmployee.Email;
            txtDepartment.Text = _selectedEmployee.Department.DepartmentName;
            txtRole.Text = _selectedEmployee.Role.RoleName;
            txtStatus.Text = _selectedEmployee.Status;
        }
    }
}
