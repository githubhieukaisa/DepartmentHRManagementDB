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
            }
            dgTasks.ItemsSource = taskAssignments;
        }
    }
}
