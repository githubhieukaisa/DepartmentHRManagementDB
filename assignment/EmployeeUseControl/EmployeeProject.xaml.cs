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
    /// Interaction logic for EmployeeProject.xaml
    /// </summary>
    public partial class EmployeeProject : UserControl
    {
        private ProjectManagementDbContext context = ProjectManagementDbContext.Instance;
        private Employee? _selectedEmployee = null;
        public EmployeeProject(Employee? employee)
        {
            InitializeComponent();
            _selectedEmployee = employee;
            LoadProject();
        }

        private void LoadProject()
        {
            var teamIds = context.TeamMembers
                .Where(tm => tm.EmployeeId == _selectedEmployee.EmployeeId)
                .Select(tm => tm.TeamId)
                .ToList();

            var projects = context.Projects
                .Where(p => teamIds.Contains(p.TeamId))
                .Select(p => new
                {
                    p.ProjectName,
                    p.Description,
                    p.StartDate,
                    p.EndDate,
                    TeamName = p.Team.TeamName
                })
                .ToList();
            dgProjects.ItemsSource = projects;
        }
    }
}
