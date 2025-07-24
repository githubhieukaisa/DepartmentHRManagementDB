using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using assignment.Models;
using OfficeOpenXml;
using System.IO;
using System.Drawing;

namespace assignment.Service.ExcelService
{
    public class EmployeeExcelExporter : IExcelExporter
    {
        public void ExportToExcel(Dictionary<string, object> datas, string filePath)
        {
            if (datas == null || !datas.Any())
            {
                throw new ArgumentException("Data cannot be null or empty", nameof(datas));
            }
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var employees = datas["Employees"] as List<Employee>;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Employees");
                var headerRow = new string[] { "Full Name", "Email", "Role", "Department", "Status" };

                // Định dạng tiêu đề
                var headerRange = worksheet.Cells[1, 1, 1, headerRow.Length];
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                headerRange.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                headerRange.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                headerRange.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                for (int i = 0; i < headerRow.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headerRow[i];
                }

                // Ghi dữ liệu
                for (int i = 0; i < employees.Count; i++)
                {
                    var employee = employees[i];
                    worksheet.Cells[i + 2, 1].Value = employee.FullName;
                    worksheet.Cells[i + 2, 2].Value = employee.Email;
                    worksheet.Cells[i + 2, 3].Value = employee.Role?.RoleName;
                    worksheet.Cells[i + 2, 4].Value = employee.Department?.DepartmentName;
                    worksheet.Cells[i + 2, 5].Value = employee.Status;

                    var dataRange = worksheet.Cells[i + 2, 1, i + 2, 5];
                    dataRange.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                }

                // Tự động điều chỉnh cột
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // Thêm tiêu đề và ngày xuất
                worksheet.Cells[1, 1, 1, 5].Merge = true;
                worksheet.Cells[1, 1].Value = "Employee Management Report";
                worksheet.Cells[1, 1].Style.Font.Bold = true;
                worksheet.Cells[1, 1].Style.Font.Size = 16;
                worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                worksheet.Cells[2, 1, 2, 5].Merge = true;
                worksheet.Cells[2, 1].Value = $"Export Date: {DateTime.Now:dd/MM/yyyy HH:mm:ss}";
                worksheet.Cells[2, 1].Style.Font.Italic = true;
                worksheet.Cells[2, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                // Lưu file
                File.WriteAllBytes(filePath, package.GetAsByteArray());
            }
        }
    }
}
