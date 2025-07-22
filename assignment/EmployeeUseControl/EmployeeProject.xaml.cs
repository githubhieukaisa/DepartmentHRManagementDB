using assignment.Models;
using Microsoft.EntityFrameworkCore;
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
                .Include(p => p.Team)
                .ToList();
            dgProjects.ItemsSource = projects;
        }

        private void btnMarkAsDone_Click(object sender, RoutedEventArgs e)
        {
            var project= dgProjects.SelectedItem as Project;
            if(project == null)
            {
                MessageBox.Show("Please select a project to mark as done.");
                return;
            }
            var tasks= context.Tasks
                .Where(t => t.ProjectId == project.ProjectId)
                .Include(t => t.Status)
                .ToList();
            if(tasks.Any(t=>t.Status.StatusName != "Verified"))
            {
                MessageBox.Show("All tasks in the project must be verify before marking the project as done.");
                return;
            }

            var team= context.Teams
                .Include(t => t.Projects)
                .FirstOrDefault(t => t.TeamId == project.TeamId);
            team.DoneAt= DateTime.Now;
            team.Status = "InActive";
            context.Teams.Update(team);
            context.SaveChanges();
            MessageBox.Show("Project marked as done successfully.");
            LoadProject();
        }
    }
}
