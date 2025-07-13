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

namespace assignment.AdminViewModel
{
    /// <summary>
    /// Interaction logic for TeamManagementPage.xaml
    /// </summary>
    public partial class TeamManagementPage : UserControl
    {
        private readonly ProjectManagementDbContext context = new();
        private Team? _selectedTeam = null;

        public TeamManagementPage()
        {
            InitializeComponent();
            resizeToOrigin();
            LoadTeams();
        }

        private void LoadTeams(string? keyword = null)
        {
            var teams = context.Teams.Where(t=> t.Status.Equals("Active")).AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                teams = teams.Where(t => t.TeamName.Contains(keyword));
            }

            dgTeam.ItemsSource = teams.ToList();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            _selectedTeam = null;
            setUpForm("Add New Team", "", "", DateTime.Today, "Add");
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            var team = dgTeam.SelectedItem as Team;
            if (team == null)
            {
                MessageBox.Show("Please select a team to edit.");
                return;
            }

            _selectedTeam = team;
            setUpForm("Edit Team", team.TeamName, team.Description, team.CreatedAt ?? DateTime.Today, "Save");
        }

        private void btnSaveTeam_Click(object sender, RoutedEventArgs e)
        {
            string name = txtTeamName.Text.Trim();
            string desc = txtDescription.Text.Trim();
            DateTime? created = dpCreatedAt.SelectedDate;

            if (string.IsNullOrWhiteSpace(name) || created == null)
            {
                MessageBox.Show("Team name and created date are required.");
                return;
            }

            if (_selectedTeam == null)
            {
                // Add
                var team = new Team
                {
                    TeamName = name,
                    Description = desc,
                    CreatedAt = created
                };
                context.Teams.Add(team);
                context.SaveChanges();
                MessageBox.Show("Team added.");
            }
            else
            {
                // Update
                _selectedTeam.TeamName = name;
                _selectedTeam.Description = desc;
                _selectedTeam.CreatedAt = created;
                context.Teams.Update(_selectedTeam);
                context.SaveChanges();
                MessageBox.Show("Team updated.");
            }

            formTeamManagement.Visibility = Visibility.Collapsed;
            _selectedTeam = null;
            resizeToOrigin();
            LoadTeams();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string key = txtSearch.Text.Trim();
            LoadTeams(key);
        }

        private void setUpForm(string header, string? name, string? desc, DateTime? createdAt, string buttonLabel)
        {
            formTeamManagement.Visibility = Visibility.Visible;
            headerOfForm.Text = header;
            txtTeamName.Text = name;
            txtDescription.Text = desc;
            dpCreatedAt.SelectedDate = createdAt;
            btnSaveTeam.Content = buttonLabel;

            var window = Window.GetWindow(this);
            if (window != null)
            {
                window.Height = 550;
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
        private void dgTeam_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var team = dgTeam.SelectedItem as Team;
            _selectedTeam = team;
            if (team == null)
            {
                return;
            }
            var teamMembers = context.TeamMembers.Where(tm=>tm.TeamId==team.TeamId).Include(tm=>tm.Employee).ToList();
            dgTeamMembers.ItemsSource = teamMembers;
        }

        private void btnAddMember_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedTeam == null)
            {
                MessageBox.Show("Please select a team first.");
                return;
            }

            var existingMembersIds= context.TeamMembers
                .Where(tm => tm.TeamId == _selectedTeam.TeamId)
                .Select(tm => tm.EmployeeId)
                .ToList();

            var addWindow = new AddTeamMemberWindow(existingMembersIds);
            addWindow.Owner = Window.GetWindow(this);

            if (addWindow.ShowDialog() == true)
            {
                int seelctedEmployeeId = addWindow.SelectedEmployeeId ?? 0;
                var newMember = new TeamMember
                {
                    TeamId = _selectedTeam.TeamId,
                    EmployeeId = seelctedEmployeeId,
                    JoinedAt = DateTime.Now
                };
                context.TeamMembers.Add(newMember);
                context.SaveChanges();
                MessageBox.Show("Member added to team.");
                dgTeam_SelectionChanged(null, null);
            }
        }

        private void btnRemoveMember_Click(object sender, RoutedEventArgs e)
        {
            var teamMember = dgTeamMembers.SelectedItem as TeamMember;
            if (teamMember == null)
            {
                MessageBox.Show("Please select a team member to remove.");
                return;
            }
            if (MessageBox.Show("Are you sure you want to remove this member from the team?", "Confirm Removal", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                context.TeamMembers.Remove(teamMember);
                context.SaveChanges();
                MessageBox.Show("Member removed from team.");
                dgTeam_SelectionChanged(null, null);
            }
        }

    }
}
