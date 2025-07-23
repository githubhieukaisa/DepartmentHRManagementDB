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
using static assignment.utility.ConstantClass;

namespace assignment.EmployeeUseControl
{
    /// <summary>
    /// Interaction logic for EmployeeTask.xaml
    /// </summary>
    public partial class EmployeeTask : UserControl
    {
        private ProjectManagementDbContext context = ProjectManagementDbContext.Instance;
        private Employee? _selectedEmployee = null;
        private int _selectedProjectId = 0;
        private bool _isQA = false;
        private bool _isSuccessProject = false;
        public EmployeeTask(Employee? selectedEmployee)
        {
            InitializeComponent();
            _selectedEmployee = selectedEmployee;
            LoadProject();
            LoadTasks();
        }

        private void LoadProject()
        {
            var projects= context.Projects
                .Where(p=> p.Team.TeamMembers.Any(tm=> tm.EmployeeId== _selectedEmployee.EmployeeId))
                .ToList();
            cbProjects.ItemsSource = projects;
            cbProjects.SelectedIndex = 0;
        }

        private void LoadTasks()
        {
            string roleNames = context.Departments.Where(d=>d.DepartmentId== _selectedEmployee.DepartmentId)
                .Select(d => d.DepartmentName)
                .FirstOrDefault();
            if (roleNames.Equals("QA"))
            {
                _isQA= true;
                LoadTaskOfQA();
            }
            else
            {
                LoadTasksOfDeveloperAndTester();
            }
        }

        private void LoadTaskOfQA()
        {
            var tasks= context.Tasks
                .Where(t => t.ProjectId == _selectedProjectId)
                .Include(t => t.Status)
                .Include(t => t.Reporter)
                .ToList();
            dgTaskAssignments.Visibility= Visibility.Collapsed;
            dgTasks.Visibility = Visibility.Visible;
            btnAddTask.Visibility = Visibility.Visible;
            dgTasks.ItemsSource = tasks;
        }

        private void LoadTasksOfDeveloperAndTester()
        {
            var taskAssignments = context.TaskAssignments
                .Where(ta => ta.EmployeeId == _selectedEmployee.EmployeeId)
                .Include(ta => ta.Task)
                .ThenInclude(t => t.Project)
                .Include(ta => ta.Task)
                .ThenInclude(t => t.Status)
                .ToList();
            foreach (var taskAssignment in taskAssignments)
            {
                taskAssignment.allStatusAvailable = roleStatusMap[context.Roles.FirstOrDefault(r=> r.RoleId == taskAssignment.RoleId).RoleName];
                if (taskAssignment.Task == null)
                {
                    continue;
                }
                if (!taskAssignment.allStatusAvailable.Any(status => status.StatusId == taskAssignment.Task.StatusId))
                {
                    var statusToAdd = context.TaskStatuses.FirstOrDefault(s => s.StatusId == taskAssignment.Task.StatusId);
                    if (statusToAdd != null)
                    {
                        taskAssignment.allStatusAvailable.Add(statusToAdd);
                    }
                }
                taskAssignment.Task.Reporter = context.Employees
                    .FirstOrDefault(e => e.EmployeeId == taskAssignment.Task.ReporterId);
                var defaultStatuses = roleDefaultStatusMap[context.Roles
                    .FirstOrDefault(r => r.RoleId == taskAssignment.RoleId)?.RoleName];
                if (defaultStatuses == null) continue;
                taskAssignment.canEditStatus = !_isSuccessProject &&
                    defaultStatuses.Any(s => s.StatusId == taskAssignment.Task.StatusId);
            }
            dgTaskAssignments.ItemsSource = taskAssignments;
        }

        private void ComboStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isSuccessProject) return;
            if (sender is ComboBox comboBox && comboBox.SelectedValue is int selectedStatusId)
            {
                if (comboBox.DataContext is not TaskAssignment taskAssignment) return;

                var task = taskAssignment.Task;
                if (task == null) return;
                if (task.StatusId == selectedStatusId)
                {
                    return;
                }

                task.StatusId = selectedStatusId;
                task.UpdatedAt = DateTime.Now;

                context.SaveChanges();
                LoadTasks();
                MessageBox.Show("Cập nhật trạng thái thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnViewPartner_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is TaskAssignment taskAssignment)
            {
                var task = taskAssignment.Task;
                redirecToAssignWindow(task, false);
            }
        }

        private void btnViewAssign_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Models.Task task)
            {
                bool canEdit = !_isSuccessProject;
                redirecToAssignWindow(task, canEdit);
            }
        }
        private void redirecToAssignWindow(Models.Task task, bool canEdit)
        {
            if (task == null || task.Project == null)
            {
                MessageBox.Show("Không có thông tin dự án để hiển thị.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var viewPartner = new ViewPartner(task.TaskId, canEdit);
            viewPartner.ShowDialog();
        }

        private void cbProjects_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbProjects.SelectedValue == null) return;
            _selectedProjectId= (int)cbProjects.SelectedValue;
            var team= context.Projects
                .Where(p => p.ProjectId == _selectedProjectId)
                .Select(p => p.Team)
                .FirstOrDefault();
            if(team.DoneAt==null)
            {
                _isSuccessProject = false;
            }
            else
            {
                _isSuccessProject = true;
            }
        }

        private void btnSaveTask_Click(object sender, RoutedEventArgs e)
        {
            string title = txtTaskTitle.Text.Trim();
            string description = txtDescription.Text.Trim();
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(description))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var newTask = new Models.Task
            {
                Title = title,
                Description = description,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                ReporterId = _selectedEmployee.EmployeeId,
                ProjectId = _selectedProjectId,
                StatusId = 1
            };
            context.Tasks.Add(newTask);
            context.SaveChanges();
            MessageBox.Show("Thêm task thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

            formAddTask.Visibility = Visibility.Collapsed;
            txtTaskTitle.Text = "";
            txtDescription.Text = "";
            btnAddTask.Visibility = Visibility.Visible;
            LoadTasks();
        }

        private void btnCancelTask_Click(object sender, RoutedEventArgs e)
        {
            formAddTask.Visibility = Visibility.Collapsed;
            txtTaskTitle.Text = "";
            txtDescription.Text = "";
            btnAddTask.Visibility = Visibility.Visible;
        }

        private void btnAddTask_Click(object sender, RoutedEventArgs e)
        {
            formAddTask.Visibility = Visibility.Visible;
            btnAddTask.Visibility = Visibility.Collapsed;
        }
    }
}
