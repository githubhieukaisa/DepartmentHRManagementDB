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
using Task = assignment.Models.Task;

namespace assignment.AdminViewModel
{
    /// <summary>
    /// Interaction logic for TaskManagementPage.xaml
    /// </summary>
    public partial class TaskManagementPage : UserControl
    {
        private readonly ProjectManagementDbContext context = ProjectManagementDbContext.Instance;

        public TaskManagementPage()
        {
            InitializeComponent();
            LoadProjects();
            resizeToOrigin();
        }
        private void LoadProjects()
        {
            var projects = context.Projects
            .Include(p => p.Team)
            .ToList();
            cbProject.ItemsSource = projects;
            cbProject.SelectedIndex = 0;
        }

        private void resizeToOrigin()
        {
            var window = Window.GetWindow(this);
            if (window != null)
            {
                window.Height = 450;
            }
        }

        private void cbProject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbProject.SelectedValue == null)
                return;
            int selectedProjectId = (int)cbProject.SelectedValue;
            LoadTasksByProjectId(selectedProjectId);
        }
        private void LoadTasksByProjectId(int projectId)
        {
            var tasks = context.Tasks
            .Where(t => t.ProjectId == projectId)
            .Include(t => t.Project)
            .Include(t => t.Reporter)
            .Include(t => t.Status)
            .ToList();
            dgTask.ItemsSource = tasks;
        }
    }
}
