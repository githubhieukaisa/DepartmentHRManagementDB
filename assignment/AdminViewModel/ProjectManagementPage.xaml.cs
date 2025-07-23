using assignment.Models;
using assignment.Service;
using assignment.utility;
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
using static assignment.utility.Validation;

namespace assignment.AdminViewModel
{
    /// <summary>
    /// Interaction logic for ProjectManagementPage.xaml
    /// </summary>
    public partial class ProjectManagementPage : UserControl
    {
        private readonly ProjectManagementDbContext context = ProjectManagementDbContext.Instance;
        private Project? _projectToAssignTeam = null;
        private Project? _selectedProject = null;

        public ProjectManagementPage()
        {
            InitializeComponent();
            resizeToOrigin();
            LoadProjects();
        }

        private void LoadProjects(string? keyword = null)
        {
            var projectsQuery = context.Projects
                .Include(p => p.Team)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                projectsQuery = projectsQuery.Where(p => p.ProjectName.Contains(keyword));
            }
            var projects = projectsQuery.ToList();
            foreach (var project in projects)
            {
                var team= context.Teams.FirstOrDefault(t => t.TeamId == project.TeamId);
                if (team == null)
                {
                    project.isSuccessful = false;
                }
                else
                {
                    project.isSuccessful = team.DoneAt != null;
                }
            }

            dgProject.ItemsSource = projects.ToList();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            _selectedProject = null;
            setUpForm("Add New Project", "", "", null, null, "Add");
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            var project = dgProject.SelectedItem as Project;
            if (project == null)
            {
                MessageBox.Show("Please select a project to edit.");
                return;
            }

            _selectedProject = project;
            setUpForm("Edit Project", project.ProjectName, project.Description,
                      project.StartDate?.ToDateTime(TimeOnly.MinValue),
                      project.EndDate?.ToDateTime(TimeOnly.MinValue),
                      "Save");
        }

        private void btnSaveProject_Click(object sender, RoutedEventArgs e)
        {
            var errorList = new List<string>();
            string name;
            if (!Validate(txtProjectName.Text.Trim(), s => s,
                new List<ValidationRule<string>>
                {
                    new ValidationRule<string>(s => s.Length > 20, "Name phải nhiểu hơn 20 kí tự.")
                },
                out string errorMessage, out name, "Name"))
            {
                errorList.Add(errorMessage);
            }
            string desc;
            if(!Validate(txtDescription.Text.Trim(), s => s,
                new List<ValidationRule<string>>
                {
                    new ValidationRule<string>(s => s.Length > 30, "Description phải nhiểu hơn 30 kí tự.")
                },
                out errorMessage, out desc, "Description"))
            {
                errorList.Add(errorMessage);
            }
            DateTime start;
            if(!Validate(dpStartDate.SelectedDate?.ToString(), s => DateTime.Parse(s),
                new List<ValidationRule<DateTime>>
                {
                    new ValidationRule<DateTime>(d => d.Date>=DateTime.Today, "Start date phải trước ngày hiện tại.")
                },
                out errorMessage, out start, "Start Date"))
            {
                errorList.Add(errorMessage);
            }
            DateTime end;
            if(!Validate(dpEndDate.SelectedDate?.ToString(), s => DateTime.Parse(s),
                new List<ValidationRule<DateTime>>
                {
                    new ValidationRule<DateTime>(d => d.Date>=DateTime.Today, "End date phải sau ngày hiện tại."),
                    new ValidationRule<DateTime>(d => start == null || d.Date>= start.Date, "End date phải sau start date.")
                },
                out errorMessage, out end, "End Date"))
            {
                errorList.Add(errorMessage);
            }
            if(errorList.Count > 0)
            {
                MessageBox.Show(string.Join("\n", errorList), "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var service = new ProjectService(context);
            service.SaveProject(_selectedProject, name, desc, start, end);
            MessageBox.Show(_selectedProject == null ? "Project added." : "Project updated.");

            formProjectManagement.Visibility = Visibility.Collapsed;
            _selectedProject = null;
            resizeToOrigin();
            LoadProjects();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string key = txtSearch.Text.Trim();
            LoadProjects(key);
        }

        private void setUpForm(string header, string? name, string? desc, DateTime? start, DateTime? end, string btnLabel)
        {
            formProjectManagement.Visibility = Visibility.Visible;
            headerOfForm.Text = header;
            txtProjectName.Text = name;
            txtDescription.Text = desc;
            if(start == null)
            {
                start = DateTime.Today;
            }
            dpStartDate.SelectedDate = start;
            if(end == null)
            {
                end = DateTime.Today.AddDays(7);
            }
            dpEndDate.SelectedDate = end;
            btnSaveProject.Content = btnLabel;

            var window = Window.GetWindow(this);
            if (window != null)
            {
                window.Height = 650;
            }
        }

        private void btnAddTeam_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var project = button?.DataContext as Project;

            if (project == null)
            {
                MessageBox.Show("Không thể xác định project.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (project.TeamId != null)
            {
                MessageBox.Show("Project này đã có team.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            _projectToAssignTeam = project;
            formAddTeam.Visibility = Visibility.Visible;

            // Load danh sách team chưa được assign
            var availableTeams = context.Teams
                .Where(t => !context.Projects.Any(p => p.TeamId == t.TeamId))
                .ToList();

            cmbAssignTeam.ItemsSource = availableTeams;
            cmbAssignTeam.SelectedIndex = 0;
        }
        private void btnSaveAssignTeam_Click(object sender, RoutedEventArgs e)
        {
            if (_projectToAssignTeam == null || cmbAssignTeam.SelectedValue == null)
                return;

            int selectedTeamId = (int)cmbAssignTeam.SelectedValue;
            _projectToAssignTeam.TeamId = selectedTeamId;

            context.Projects.Update(_projectToAssignTeam);
            context.SaveChanges();

            MessageBox.Show("Team assigned to project.");
            formAddTeam.Visibility = Visibility.Collapsed;
            _projectToAssignTeam = null;

            LoadProjects();
        }
        private void resizeToOrigin()
        {
            var window = Window.GetWindow(this);
            if (window != null)
            {
                window.Height = 450;
            }
        }
    }
}
