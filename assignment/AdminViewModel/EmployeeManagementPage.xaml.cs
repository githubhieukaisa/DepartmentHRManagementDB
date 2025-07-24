using assignment.Models;
using assignment.Service;
using assignment.utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
using static assignment.utility.Validation;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace assignment.AdminViewModel
{
    /// <summary>
    /// Interaction logic for EmployeeManagementPage.xaml
    /// </summary>
    public partial class EmployeeManagementPage : UserControl
    {
        private ProjectManagementDbContext context = ProjectManagementDbContext.Instance;
        private Employee? _selectedEmployee=null;
        public EmployeeManagementPage()
        {
            InitializeComponent();
            resizeToOrigin();
            LoadEmployees();
        }

        private void LoadEmployees(string? key = null)
        {
            if (key == null || key.Equals(""))
            {
                dgEmployee.ItemsSource = context.Employees
                    .Include(e => e.Role)
                    .Include(e => e.Department)
                    .ToList();
            }
            else
            {
                dgEmployee.ItemsSource = context.Employees
                    .Include(e => e.Role)
                    .Include(e => e.Department)
                    .Where(e => e.FullName.Contains(key) || e.Email.Contains(key))
                    .ToList();
            }
        }

        private void AddEmployee_Click(object sender, RoutedEventArgs e)
        {
            setUpFormManagement();
            cmbRole.SelectedIndex= 0;
            cmbDepartment.SelectedIndex = 0;
        }

        private void EditEmployee_Click(object sender, RoutedEventArgs e)
        {
            var selectedEmployee = dgEmployee.SelectedItem as Employee;
            if(selectedEmployee == null)
            {
                MessageBox.Show("Please select an employee to edit.");
                return;
            }
            _selectedEmployee=selectedEmployee;
            setUpFormManagement("Edit Employee", selectedEmployee.FullName, selectedEmployee.Email,selectedEmployee.PasswordHash,selectedEmployee.RoleId,selectedEmployee.DepartmentId,"Save");
        }

        private void setUpFormManagement(string header="Add new Employee", string? fullname="", string? email="",string? password="", int? roleId=null, int? departmentId=null, string btnContent="Add")
        {
            var window = Window.GetWindow(this);
            if (window != null)
            {
                window.Height = 700;
            }

            formEmployeeManagement.Visibility = Visibility.Visible;
            var roles = context.Roles.Where(r => r.RoleId == 2).ToList();
            cmbRole.ItemsSource = roles;
            if(roleId.HasValue)
            {
                cmbRole.SelectedValue = roleId.Value;
            }
            else
            {
                cmbRole.SelectedIndex = 0;
            }

            var departments = context.Departments
                .Select(d => new {
                    d.DepartmentId,
                    d.DepartmentName
                })
                .ToList();
            cmbDepartment.ItemsSource = departments;
            if(departmentId.HasValue)
            {
                cmbDepartment.SelectedValue = departmentId.Value;
            }
            else
            {
                cmbDepartment.SelectedIndex = 0;
            }

            headerOfForm.Text = header;
            txtFullName.Text = fullname;
            txtEmail.Text = email;
            txtPassword.Text = password;
            btnManagement.Content = btnContent;
        }

        private void DeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            var selectedEmployee = dgEmployee.SelectedItem as Employee;
            if (selectedEmployee == null)
            {
                MessageBox.Show("Please select an employee to deactive.");
                return;
            }

            if(selectedEmployee.Status == "Inactive")
            {
                MessageBox.Show("This employee is already inactive.");
                return;
            }

            var team= context.Teams
                .FirstOrDefault(t=>t.TeamMembers.Any(tm => tm.EmployeeId == selectedEmployee.EmployeeId) && t.DoneAt == null);
            if (team != null)
            {
                MessageBox.Show($"This employee is a member of an active team ({team.TeamName}). Please remove them from the team before deactivating.");
                return;
            }

            if (MessageBox.Show($"Are you sure you want to deactive {selectedEmployee.FullName}?", "Confirm Deactivation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                selectedEmployee.Status = "Inactive";
                context.Employees.Update(selectedEmployee);
                context.SaveChanges();
                MessageBox.Show("Employee deactivated successfully.");
                LoadEmployees();
            }
        }

        private void ActiveEmployee_Click(object sender, RoutedEventArgs e)
        {
            var selectedEmployee = dgEmployee.SelectedItem as Employee;
            if (selectedEmployee == null)
            {
                MessageBox.Show("Please select an employee to activate.");
                return;
            }

            if (selectedEmployee.Status == "Active")
            {
                MessageBox.Show("This employee is already active.");
                return;
            }

            if (MessageBox.Show($"Are you sure you want to activate {selectedEmployee.FullName}?", "Confirm Activation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                selectedEmployee.Status = "Active";
                context.Employees.Update(selectedEmployee);
                context.SaveChanges();
                MessageBox.Show("Employee activated successfully.");
                LoadEmployees();
            }
        }

        private void btnManagement_Click(object sender, RoutedEventArgs e)
        {
            var errorList = new List<string>();
            string fullname;
            if(!Validate(txtFullName.Text.Trim(), s => CapitalizeEachWord(s), 
                new List<ValidationRule<string>>
                {
                    new ValidationRule<string>(s => s.Length >= 10,"Full Name không được ít hơn 10 ký tự"),
                    new ValidationRule<string>(s => s.Length <= 50,"Full Name không được quá 50 ký tự"),
                    new ValidationRule<string>(s => s.All(c => char.IsLetter(c) || c==' '),"Full Name chỉ được chứa chữ cái hoặc space")
                }, 
                out string errorMessage, out fullname, "Full Name"))
            {
                errorList.Add(errorMessage);
            }
            string email;
            if (!Validate(txtEmail.Text.Trim(), s => s, 
                new List<ValidationRule<string>>
                {
                    new ValidationRule<string>(s => s.Length >= 10,"Email không được ít hơn 10 ký tự"),
                    new ValidationRule<string>(s => s.Length <= 50,"Email không được quá 50 ký tự"),
                    new ValidationRule<string>(s => s.Contains("@") && s.Contains("."), "Email phải chứa '@' và '.'")
                }, 
                out errorMessage, out email, "Email"))
            {
                errorList.Add(errorMessage);
            }
            string password;
            if(!Validate(txtPassword.Text.Trim(), s => s, 
                new List<ValidationRule<string>>
                {
                    new ValidationRule<string>(s => s.Length >= 6,"Password không được ít hơn 6 ký tự"),
                    new ValidationRule<string>(s => s.Length <= 20,"Password không được quá 20 ký tự")
                }, 
                out errorMessage, out password, "Password"))
            {
                errorList.Add(errorMessage);
            }

            int roleId = (int)cmbRole.SelectedValue;
            int departmentId = (int)cmbDepartment.SelectedValue;
            if(errorList.Count > 0)
            {
                MessageBox.Show(string.Join("\n", errorList));
                return;
            }
            var service = new EmployeeManagementService(context);
            service.SaveEmployee(_selectedEmployee, btnManagement.Content.ToString(), fullname, email, password, roleId, departmentId, "Active");
            _selectedEmployee = null;
            formEmployeeManagement.Visibility = Visibility.Collapsed;
            resizeToOrigin();
            LoadEmployees();
        }

        private void search_Click(object sender, RoutedEventArgs e)
        {
            var key= txtSearch.Text.Trim();
            LoadEmployees(key);
        }

        private void resizeToOrigin()
        {
            var window = Window.GetWindow(this);
            if (window != null)
            {
                window.Height = 500;
            }
        }

        private void btnExportToExcel_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra nếu DataGrid có dữ liệu
            if (dgEmployee.ItemsSource == null || !(dgEmployee.ItemsSource is List<Employee> employees) || employees.Count == 0)
            {
                MessageBox.Show("No data to export.", "Export Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Cấu hình EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Tạo SaveFileDialog để người dùng chọn vị trí lưu file
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*",
                FileName = "EmployeeManagement_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    using (var package = new ExcelPackage())
                    {
                        var worksheet = package.Workbook.Worksheets.Add("Employees");
                        var headerRow = new string[] { "Full Name", "Email", "Role", "Department", "Status" };

                        // Định dạng tiêu đề
                        var headerRange = worksheet.Cells[1, 1, 1, headerRow.Length];
                        headerRange.Style.Font.Bold = true;
                        headerRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        headerRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                        headerRange.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        headerRange.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        for (int i = 0; i < headerRow.Length; i++)
                        {
                            worksheet.Cells[1, i + 1].Value = headerRow[i];
                        }

                        // Ghi dữ liệu từ DataGrid
                        for (int i = 0; i < employees.Count; i++)
                        {
                            var employee = employees[i];
                            worksheet.Cells[i + 2, 1].Value = employee.FullName;
                            worksheet.Cells[i + 2, 2].Value = employee.Email;
                            worksheet.Cells[i + 2, 3].Value = employee.Role?.RoleName;
                            worksheet.Cells[i + 2, 4].Value = employee.Department?.DepartmentName;
                            worksheet.Cells[i + 2, 5].Value = employee.Status;

                            // Định dạng dữ liệu
                            var dataRange = worksheet.Cells[i + 2, 1, i + 2, 5];
                            dataRange.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        }

                        // Tự động điều chỉnh cột
                        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                        // Thêm tiêu đề trang
                        worksheet.Cells[1, 1, 1, 5].Merge = true;
                        worksheet.Cells[1, 1].Value = "Employee Management Report";
                        worksheet.Cells[1, 1].Style.Font.Bold = true;
                        worksheet.Cells[1, 1].Style.Font.Size = 16;
                        worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                        // Thêm ngày xuất
                        worksheet.Cells[2, 1, 2, 5].Merge = true;
                        worksheet.Cells[2, 1].Value = $"Export Date: {DateTime.Now:dd/MM/yyyy HH:mm:ss}";
                        worksheet.Cells[2, 1].Style.Font.Italic = true;
                        worksheet.Cells[2, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                        // Lưu file
                        File.WriteAllBytes(saveFileDialog.FileName, package.GetAsByteArray());
                        MessageBox.Show("Data exported successfully to " + saveFileDialog.FileName, "Export Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error exporting to Excel: " + ex.Message, "Export Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
