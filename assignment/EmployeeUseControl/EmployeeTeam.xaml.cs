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
    /// Interaction logic for EmployeeTeam.xaml
    /// </summary>
    public partial class EmployeeTeam : UserControl
    {
        private ProjectManagementDbContext context = ProjectManagementDbContext.Instance;
        private Employee? _selectedEmployee = null;
        public EmployeeTeam(Employee? selectedEmployee)
        {
            InitializeComponent();
            _selectedEmployee = selectedEmployee;
            LoadTeams();
        }

        private void LoadTeams()
        {
            if (_selectedEmployee == null) return;

            var teams = context.TeamMembers
                .Where(tm => tm.EmployeeId == _selectedEmployee.EmployeeId)
                .Join(context.Teams,
                      tm => tm.TeamId,
                      t => t.TeamId,
                      (tm, t) => new
                      {
                          t.TeamName,
                          t.Description,
                          tm.JoinedAt,
                          t.CreatedAt,
                          t.Status,
                          t.DoneAt
                      })
                .ToList();

            dgTeams.ItemsSource = teams;
        }
    }
}
