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
using System.Windows.Shapes;

namespace assignment.EmployeeUseControl
{
    /// <summary>
    /// Interaction logic for ViewPartner.xaml
    /// </summary>
    public partial class ViewPartner : Window
    {
        private readonly ProjectManagementDbContext context = ProjectManagementDbContext.Instance;
        private readonly int _taskId;
        private readonly bool _canEdit;
        private List<TaskAssignment> _partners = new();
        public ViewPartner(int taskId, bool canEdit)
        {
            InitializeComponent();
            _taskId = taskId;
            _canEdit = canEdit;
            btnAdd.Visibility = _canEdit? Visibility.Visible : Visibility.Collapsed;
            btnDelete.Visibility = _canEdit ? Visibility.Visible : Visibility.Collapsed;
            if (_canEdit)
            {
                LoadCbEmployee();
            }
            LoadData();
        }

        private void LoadCbEmployee()
        {
            var teamId = context.Tasks
                .Where(t => t.TaskId == _taskId)
                .Include(t => t.Project)
                .ThenInclude(p => p.Team)
                .Select(t => t.Project.TeamId)
                .FirstOrDefault();
            if (teamId == 0)
            {
                MessageBox.Show("Không tìm thấy team của task.");
                return;
            }

            // Bước 2: Lấy danh sách thành viên trong team
            var teamMemberIds = context.TeamMembers
                .Where(tm => tm.TeamId == teamId)
                .Select(tm => tm.EmployeeId)
                .ToList();

            // Bước 3: Những người đã được gán vào task này
            var assignedEmployeeIds = context.TaskAssignments
                .Where(ta => ta.TaskId == _taskId)
                .Select(ta => ta.EmployeeId)
                .ToList();

            // Bước 4: Lấy danh sách còn lại (chưa bị gán), bao gồm Department
            var availableEmployees = context.Employees
                .Where(e => teamMemberIds.Contains(e.EmployeeId) && !assignedEmployeeIds.Contains(e.EmployeeId))
                .Include(e => e.Department)
                .Where(e=>!e.Department.DepartmentName.Equals("QA"))
                .ToList();

            // Bước 5: Map thành danh sách hiển thị
            var displayList = availableEmployees.Select(e => new EmployeeDisplayItem
            {
                EmployeeId = e.EmployeeId,
                DisplayText = $"{e.FullName} ({e.Department?.DepartmentName ?? "Không rõ"})",
                DepartmentName = e.Department?.DepartmentName ?? ""
            }).ToList();

            // Bước 6: Gán cho ComboBox
            cbTeamMember.ItemsSource = displayList;
            cbTeamMember.DisplayMemberPath = "DisplayText";
            cbTeamMember.SelectedValuePath = "EmployeeId";
        }

        private void LoadData()
        {
            // Lấy danh sách partner (TeamMembers gắn với Task)
            _partners = context.TaskAssignments
                .Where(ta => ta.TaskId == _taskId)
                .Include(ta => ta.Employee)
                .Include(ta=>ta.Role)
                .ToList();

            dgPartners.ItemsSource = _partners;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var selectedEmployee = cbTeamMember.SelectedItem as EmployeeDisplayItem;
            
            if(selectedEmployee== null)
            {
                MessageBox.Show("Vui lòng chọn một partner để thêm.");
                return;
            }

            var departmentRoleNme= selectedEmployee.DepartmentName.Trim();
            var role= context.Roles
                .FirstOrDefault(r => r.RoleName.Equals(departmentRoleNme));
            var taskAssignment = new TaskAssignment
            {
                TaskId = _taskId,
                EmployeeId = selectedEmployee.EmployeeId,
                RoleId = role?.RoleId,
                AssignedAt = DateTime.Now
            };
            context.TaskAssignments.Add(taskAssignment);
            context.SaveChanges();

            MessageBox.Show("Đã thêm partner vào task thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            LoadCbEmployee();
            LoadData();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var taskAssignment = dgPartners.SelectedItem as TaskAssignment;
            if(taskAssignment == null)
            {
                MessageBox.Show("Vui lòng chọn một partner để xóa.");
                return;
            }
            context.TaskAssignments.Remove(taskAssignment);
            context.SaveChanges();
            MessageBox.Show("Đã xóa partner khỏi task thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            LoadCbEmployee();
            LoadData();
        }
    }
}
