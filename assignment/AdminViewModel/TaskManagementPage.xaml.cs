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
        private readonly ProjectManagementDbContext context = new();
        private Task? _selectedTask = null;

        public TaskManagementPage()
        {
            InitializeComponent();
            resizeToOrigin();
            LoadTasks();
        }

        private void LoadTasks(string? keyword = null)
        {
            var tasks = context.Tasks
                .Include(t => t.Project)
                .Include(t => t.Reporter)
                .Include(t => t.Status)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                tasks = tasks.Where(t => t.Title.Contains(keyword));
            }

            dgTask.ItemsSource = tasks.ToList();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            _selectedTask = null;
            setUpForm("Add New Task", "", "", null, null, null, "Add");
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            var task = dgTask.SelectedItem as Task;
            if (task == null)
            {
                MessageBox.Show("Please select a task to edit.");
                return;
            }

            _selectedTask = task;
            setUpForm("Edit Task", task.Title, task.Description, task.ProjectId, task.StatusId, task.ReporterId, "Save");
        }

        private void btnSaveTask_Click(object sender, RoutedEventArgs e)
        {
            string title = txtTitle.Text.Trim();
            string description = txtDescription.Text.Trim();
            int? projectId = (int?)cmbProject.SelectedValue;
            int? statusId = (int?)cmbStatus.SelectedValue;
            int? reporterId = (int?)cmbReporter.SelectedValue;

            if (string.IsNullOrWhiteSpace(title) || projectId == null || statusId == null || reporterId == null)
            {
                MessageBox.Show("Please fill all required fields.");
                return;
            }

            if (_selectedTask == null)
            {
                var task = new Task
                {
                    Title = title,
                    Description = description,
                    ProjectId = projectId,
                    StatusId = statusId,
                    ReporterId = reporterId,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                context.Tasks.Add(task);
                context.SaveChanges();
                MessageBox.Show("Task added.");
            }
            else
            {
                _selectedTask.Title = title;
                _selectedTask.Description = description;
                _selectedTask.ProjectId = projectId;
                _selectedTask.StatusId = statusId;
                _selectedTask.ReporterId = reporterId;
                _selectedTask.UpdatedAt = DateTime.Now;

                context.Tasks.Update(_selectedTask);
                context.SaveChanges();
                MessageBox.Show("Task updated.");
            }

            formTaskManagement.Visibility = Visibility.Collapsed;
            _selectedTask = null;
            resizeToOrigin();
            LoadTasks();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            LoadTasks(keyword);
        }

        private void setUpForm(string header, string? title, string? description,
                               int? projectId, int? statusId, int? reporterId,
                               string buttonLabel)
        {
            formTaskManagement.Visibility = Visibility.Visible;
            headerOfForm.Text = header;
            txtTitle.Text = title;
            txtDescription.Text = description;
            btnSaveTask.Content = buttonLabel;

            cmbProject.ItemsSource = context.Projects.ToList();
            cmbStatus.ItemsSource = context.TaskStatuses.ToList();
            cmbReporter.ItemsSource = context.Employees.ToList();

            cmbProject.SelectedValue = projectId ?? -1;
            cmbStatus.SelectedValue = statusId ?? -1;
            cmbReporter.SelectedValue = reporterId ?? -1;

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
