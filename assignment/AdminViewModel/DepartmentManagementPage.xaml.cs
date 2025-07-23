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

namespace assignment.AdminViewModel
{
    /// <summary>
    /// Interaction logic for DepartmentManagementPage.xaml
    /// </summary>
    public partial class DepartmentManagementPage : UserControl
    {
        private readonly ProjectManagementDbContext context = ProjectManagementDbContext.Instance;
        private Department? _selectedDepartment = null;

        public DepartmentManagementPage()
        {
            InitializeComponent();
            resizeToOrigin();
            LoadDepartments();
        }

        private void LoadDepartments(string? keyword = null)
        {
            var departments = context.Departments.AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                departments = departments.Where(d => d.DepartmentName.Contains(keyword));
            }

            dgDepartment.ItemsSource = departments.ToList();
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
