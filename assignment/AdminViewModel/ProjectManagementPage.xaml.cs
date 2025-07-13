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

namespace assignment.AdminViewModel
{
    /// <summary>
    /// Interaction logic for ProjectManagementPage.xaml
    /// </summary>
    public partial class ProjectManagementPage : UserControl
    {
        private readonly ProjectManagementDbContext context = new();
        private Project? _selectedProject = null;

        public ProjectManagementPage()
        {
            InitializeComponent();
            resizeToOrigin();
            LoadProjects();
        }

        private void LoadProjects(string? keyword = null)
        {
            var projects = context.Projects
                .Include(p => p.Team)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                projects = projects.Where(p => p.ProjectName.Contains(keyword));
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
            string name = txtProjectName.Text.Trim();
            string desc = txtDescription.Text.Trim();
            DateTime? start = dpStartDate.SelectedDate;
            DateTime? end = dpEndDate.SelectedDate;
            int? teamId = (int?)cmbTeam.SelectedValue;

            if (string.IsNullOrWhiteSpace(name) || start == null || end == null)
            {
                MessageBox.Show("Please fill required fields (name, start date, end date).");
                return;
            }

            if (start > end)
            {
                MessageBox.Show("End date must be after start date.");
                return;
            }

            if (_selectedProject == null)
            {
                var project = new Project
                {
                    ProjectName = name,
                    Description = desc,
                    StartDate = DateOnly.FromDateTime(start.Value),
                    EndDate = DateOnly.FromDateTime(end.Value),
                    TeamId = teamId
                };
                context.Projects.Add(project);
                context.SaveChanges();
                MessageBox.Show("Project added.");
            }
            else
            {
                _selectedProject.ProjectName = name;
                _selectedProject.Description = desc;
                _selectedProject.StartDate = DateOnly.FromDateTime(start.Value);
                _selectedProject.EndDate = DateOnly.FromDateTime(end.Value);
                _selectedProject.TeamId = teamId;

                context.Projects.Update(_selectedProject);
                context.SaveChanges();
                MessageBox.Show("Project updated.");
            }

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
            dpStartDate.SelectedDate = start;
            dpEndDate.SelectedDate = end;
            btnSaveProject.Content = btnLabel;

            // Load team list
            cmbTeam.ItemsSource = context.Teams.ToList();

            if (_selectedProject?.TeamId != null)
            {
                cmbTeam.SelectedValue = _selectedProject.TeamId;
            }
            else
            {
                cmbTeam.SelectedIndex = 0;
            }

            var window = Window.GetWindow(this);
            if (window != null)
            {
                window.Height = 650;
            }
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
