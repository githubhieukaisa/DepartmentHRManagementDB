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
        private readonly int _currentEmployeeId;
        private List<TaskAssignment> _partners = new();
        public ViewPartner(int taskId, bool canEdit)
        {
            InitializeComponent();
            _taskId = taskId;
            _canEdit = canEdit;
            btnAdd.IsEnabled = _canEdit;
            btnEdit.IsEnabled = _canEdit;
            btnDelete.IsEnabled = _canEdit;
            LoadData();
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
    }
}
