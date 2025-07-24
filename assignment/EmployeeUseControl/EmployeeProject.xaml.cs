using assignment.Models;
using assignment.Service.ExcelService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
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
            var projectDisplays = new List<ProjectDisplay>();
            bool _isQA=false;
            foreach (var project in projects)
            {
                var isQA = context.Departments.Where(d => d.DepartmentId == _selectedEmployee.DepartmentId)
                    .Select(d => d.DepartmentName)
                    .FirstOrDefault() == "QA";
                _isQA= isQA;
                bool isDone = project.Team?.DoneAt != null;

                projectDisplays.Add(new ProjectDisplay
                {
                    Project = project,
                    ShowMarkAsDone = isQA && !isDone,
                    ShowExportProject = isQA
                });
            }
            //hiden columns for non-QA employees
            //if (!_isQA)
            //{
            //    var actionsColumn = dgProjects.Columns
            //    .OfType<DataGridTemplateColumn>()
            //    .FirstOrDefault(c => c.Header.ToString() == "Actions");
            //    var exportColumn = dgProjects.Columns
            //        .OfType<DataGridTemplateColumn>()
            //        .FirstOrDefault(c => c.Header.ToString() == "Export To Excel");

            //        actionsColumn.Visibility = Visibility.Collapsed;
            //        exportColumn.Visibility =Visibility.Collapsed;
            //}
            dgProjects.ItemsSource = projectDisplays;
        }

        private void btnMarkAsDone_Click(object sender, RoutedEventArgs e)
        {
            var projectDisplay = dgProjects.SelectedItem as ProjectDisplay;
            if (projectDisplay == null)
            {
                MessageBox.Show("Please select a project to mark as done.");
                return;
            }
            var project = projectDisplay.Project;
            var tasks = context.Tasks
                .Where(t => t.ProjectId == project.ProjectId)
                .Include(t => t.Status)
                .ToList();
            if (tasks.Any(t => t.Status.StatusName != "Verified"))
            {
                MessageBox.Show("All tasks in the project must be verify before marking the project as done.");
                return;
            }

            var team = context.Teams
                .Include(t => t.Projects)
                .FirstOrDefault(t => t.TeamId == project.TeamId);
            team.DoneAt = DateTime.Now;
            team.Status = "InActive";
            context.Teams.Update(team);
            context.SaveChanges();
            MessageBox.Show("Project marked as done successfully.");
            LoadProject();
        }

        private void btnExportToExcel_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null)
            {
                MessageBox.Show("No button context found.", "Export Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var projectDisplay = button.DataContext as ProjectDisplay;
            if (projectDisplay == null)
            {
                MessageBox.Show("No project selected.", "Export Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var project = projectDisplay.Project;
            // Bước 2: Lấy dữ liệu liên quan (tasks và task assignments)
            var tasks = context.Tasks
                .Include(t => t.Status)
                .Include(t => t.Reporter)
                .Where(t => t.ProjectId == project.ProjectId)
                .ToList();

            var taskAssignments = context.TaskAssignments
                .Include(ta => ta.Employee)
                .Include(ta => ta.Role)
                .Where(ta => ta.TaskId == tasks.Select(t => t.TaskId).FirstOrDefault())
                .ToList();

            // Bước 3: Chuẩn bị dữ liệu cho exporter
            var data = new Dictionary<string, object>
            {
                ["Project"] = project,
                ["Tasks"] = tasks,
                ["TaskAssignments"] = taskAssignments
            };

            // Bước 4: Tạo SaveFileDialog
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*",
                FileName = "Project_" + project.ProjectName.Replace(" ", "_") + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx"
            };

            // Bước 5: Xuất file
            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    var exporter = ExcelExporterFactory.CreateExporter("project");
                    exporter.ExportToExcel(data, saveFileDialog.FileName);
                    MessageBox.Show("Data exported successfully to " + saveFileDialog.FileName, "Export Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error exporting to Excel: " + ex.Message, "Export Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
