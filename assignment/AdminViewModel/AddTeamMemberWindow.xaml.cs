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

namespace assignment.AdminViewModel
{
    /// <summary>
    /// Interaction logic for AddTeamMemberWindow.xaml
    /// </summary>
    public partial class AddTeamMemberWindow : Window
    {
        private readonly ProjectManagementDbContext context = ProjectManagementDbContext.Instance;
        public int? SelectedEmployeeId { get; private set; }
        private List<int?> existingMembersIdsemployees;

        public AddTeamMemberWindow(List<int?> existingMembersIdsemployees)
        {
            InitializeComponent();
            this.existingMembersIdsemployees = existingMembersIdsemployees;
            LoadData();
        }

        private void LoadData()
        {
            cmbDepartments.ItemsSource = context.Departments
                .ToList();
            cmbDepartments.SelectedIndex = 0;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (cmbEmployees.SelectedValue != null)
            {
                SelectedEmployeeId = (int)cmbEmployees.SelectedValue;
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Please select an employee.");
            }
        }

        private void cmbDepartments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbDepartments.SelectedValue == null) return;
            int selectedDepartmentId = (int)cmbDepartments.SelectedValue;
            var busyEmployeeIds = context.TeamMembers
                .Where(tm => tm.Team.DoneAt == null)
                .Select(tm => tm.EmployeeId)
                .Distinct()
                .ToList();
            var employees = context.Employees
                .Where(emp =>
                    emp.DepartmentId == selectedDepartmentId &&
                    !existingMembersIdsemployees.Contains(emp.EmployeeId) &&
                    !busyEmployeeIds.Contains(emp.EmployeeId))
                .ToList();
            cmbEmployees.ItemsSource = employees;
            cmbEmployees.SelectedIndex = 0;
        }
    }
}
