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
    /// Interaction logic for EmployeeTask.xaml
    /// </summary>
    public partial class EmployeeTask : UserControl
    {
        private ProjectManagementDbContext context = new();
        private Employee? _selectedEmployee = null;
        public EmployeeTask(Employee? selectedEmployee)
        {
            InitializeComponent();
            _selectedEmployee = selectedEmployee;
            LoadTasks();
        }

        private void LoadTasks()
        {
            var tasks = context.TaskAssignments
                    .Where(ta => ta.EmployeeId == _selectedEmployee.EmployeeId)
                    .Join(context.Tasks,
                          ta => ta.TaskId,
                          t => t.TaskId,
                          (ta, t) => new { ta, t })
                    .Join(context.TaskStatuses,
                          temp => temp.t.StatusId,
                          s => s.StatusId,
                          (temp, s) => new { temp.ta, temp.t, Status = s })
                    .Join(context.Projects,
                          temp => temp.t.ProjectId,
                          p => p.ProjectId,
                          (temp, p) => new { temp.ta, temp.t, temp.Status, Project = p })
                    .Join(context.Employees,
                          temp => temp.t.ReporterId,
                          r => r.EmployeeId,
                          (temp, r) => new
                          {
                              temp.t.Title,
                              temp.t.Description,
                              ProjectName = temp.Project.ProjectName,
                              StatusName = temp.Status.StatusName,
                              ReporterName = r.FullName,
                              temp.t.CreatedAt,
                              temp.t.UpdatedAt
                          })
                    .ToList();
            dgTasks.ItemsSource = tasks;
        }
    }
}
