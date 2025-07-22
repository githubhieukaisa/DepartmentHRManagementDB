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
        public EmployeeTask(Employee? selectedEmployee)
        {
            InitializeComponent();
            _selectedEmployee = selectedEmployee;
            LoadTasks();
        }

        private void LoadTasks()
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
                taskAssignment.canEditStatus = defaultStatuses
                    .Any(s => s.StatusId == taskAssignment.Task.StatusId);
            }
            dgTasks.ItemsSource = taskAssignments;
        }

        private void ComboStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
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
                if (task == null || task.Project == null)
                {
                    MessageBox.Show("Không có thông tin dự án để hiển thị.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                var viewPartner = new ViewPartner(task.TaskId,false);
                viewPartner.ShowDialog();
            }
        }
    }
}
