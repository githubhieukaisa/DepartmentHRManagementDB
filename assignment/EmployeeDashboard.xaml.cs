using assignment.EmployeeUseControl;
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

namespace assignment
{
    /// <summary>
    /// Interaction logic for EmployeeDashboard.xaml
    /// </summary>
    public partial class EmployeeDashboard : Window
    {
        public EmployeeDashboard()
        {
            InitializeComponent();
            LoadInitialContent();
        }

        private void LoadInitialContent()
        {
            tabControl.SelectedItem = tabControl.Items[0];
            MainContent.Content = new EmployeeProfile();
        }

        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(tabControl.SelectedItem is TabItem selectedTab)
            {
                switch (selectedTab.Header.ToString())
                {
                    case "Profile":
                        MainContent.Content = new EmployeeProfile();
                        break;
                    case "Project":
                        MainContent.Content = new EmployeeProject();
                        break;
                    case "Task":
                        MainContent.Content = new EmployeeTask();
                        break;
                    case "Team":
                        MainContent.Content = new EmployeeTeam();
                        break;
                }
            }
        }
    }
}
