using assignment.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment.Service.ExcelService
{
    public class ProjectExcelExporter : IExcelExporter
    {
        public void ExportToExcel(Dictionary<string, object> data, string filePath)
        {
            if (data == null || !data.Any())
            {
                throw new ArgumentException("Data cannot be null or empty", nameof(data));
            }

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var project = data["Project"] as Project;
            var tasks = data["Tasks"] as List<Models.Task>;
            var taskAssignments = data["TaskAssignments"] as List<TaskAssignment>;

            if (project == null || tasks == null || taskAssignments == null)
            {
                throw new ArgumentException("Invalid data format: 'Project', 'Tasks', or 'TaskAssignments' key is missing or invalid.");
            }

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("ProjectDetails");

                // Phần 1: Thông tin Project
                worksheet.Cells[1, 1].Value = "Project Details";
                worksheet.Cells[1, 1, 1, 5].Merge = true;
                worksheet.Cells[1, 1].Style.Font.Bold = true;
                worksheet.Cells[1, 1].Style.Font.Size = 16;
                worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                worksheet.Cells[2, 1].Value = "Project Name";
                worksheet.Cells[2, 2].Value = project.ProjectName;
                worksheet.Cells[2, 3].Value = "Team";
                worksheet.Cells[2, 4].Value = project.Team?.TeamName;
                worksheet.Cells[2, 5].Value = "Start Date";
                worksheet.Cells[2, 6].Value = project.StartDate?.ToString("yyyy-MM-dd");
                worksheet.Cells[3, 1].Value = "Description";
                worksheet.Cells[3, 2].Value = project.Description;
                worksheet.Cells[3, 3].Value = "End Date";
                worksheet.Cells[3, 4].Value = project.EndDate?.ToString("yyyy-MM-dd");
                worksheet.Cells[3, 5].Value = "Status";
                worksheet.Cells[3, 6].Value = "Active"; // Có thể thêm logic lấy status từ model nếu có

                // Định dạng phần Project
                var projectRange = worksheet.Cells[2, 1, 3, 6];
                projectRange.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                // Phần 2: Tiêu đề Tasks
                worksheet.Cells[5, 1].Value = "Tasks";
                worksheet.Cells[5, 1, 5, 5].Merge = true;
                worksheet.Cells[5, 1].Style.Font.Bold = true;
                worksheet.Cells[5, 1].Style.Font.Size = 14;
                worksheet.Cells[5, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                var taskHeaderRow = new string[] { "Task Title", "Description", "Status", "Reporter", "Created At" };
                for (int i = 0; i < taskHeaderRow.Length; i++)
                {
                    worksheet.Cells[6, i + 1].Value = taskHeaderRow[i];
                }
                var taskHeaderRange = worksheet.Cells[6, 1, 6, taskHeaderRow.Length];
                taskHeaderRange.Style.Font.Bold = true;
                taskHeaderRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                taskHeaderRange.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                taskHeaderRange.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                taskHeaderRange.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                // Phần 3: Dữ liệu Tasks
                for (int i = 0; i < tasks.Count; i++)
                {
                    var task = tasks[i];
                    worksheet.Cells[i + 7, 1].Value = task.Title;
                    worksheet.Cells[i + 7, 2].Value = task.Description;
                    worksheet.Cells[i + 7, 3].Value = task.Status.StatusName;
                    worksheet.Cells[i + 7, 4].Value = task.Reporter.FullName;
                    worksheet.Cells[i + 7, 5].Value = task.CreatedAt?.ToString("yyyy-MM-dd HH:mm:ss");

                    var taskDataRange = worksheet.Cells[i + 7, 1, i + 7, 5];
                    taskDataRange.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                }

                // Phần 4: Tiêu đề Task Assignments
                worksheet.Cells[8 + tasks.Count, 1].Value = "Task Assignments";
                worksheet.Cells[8 + tasks.Count, 1, 8 + tasks.Count, 5].Merge = true;
                worksheet.Cells[8 + tasks.Count, 1].Style.Font.Bold = true;
                worksheet.Cells[8 + tasks.Count, 1].Style.Font.Size = 14;
                worksheet.Cells[8 + tasks.Count, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                var assignmentHeaderRow = new string[] { "Task Title", "Assignee", "Role", "Assigned At" };
                for (int i = 0; i < assignmentHeaderRow.Length; i++)
                {
                    worksheet.Cells[9 + tasks.Count, i + 1].Value = assignmentHeaderRow[i];
                }
                var assignmentHeaderRange = worksheet.Cells[9 + tasks.Count, 1, 9 + tasks.Count, assignmentHeaderRow.Length];
                assignmentHeaderRange.Style.Font.Bold = true;
                assignmentHeaderRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                assignmentHeaderRange.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                assignmentHeaderRange.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                assignmentHeaderRange.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                // Phần 5: Dữ liệu Task Assignments
                for (int i = 0; i < taskAssignments.Count; i++)
                {
                    var assignment = taskAssignments[i];
                    var task = tasks.FirstOrDefault(t => t.TaskId == assignment.TaskId);
                    worksheet.Cells[10 + tasks.Count + i, 1].Value = task?.Title ?? "N/A";
                    worksheet.Cells[10 + tasks.Count + i, 2].Value = assignment.Employee?.FullName ?? "N/A";
                    worksheet.Cells[10 + tasks.Count + i, 3].Value = assignment.Role?.RoleName ?? "N/A";
                    worksheet.Cells[10 + tasks.Count + i, 4].Value = assignment.AssignedAt?.ToString("yyyy-MM-dd HH:mm:ss");

                    var assignmentDataRange = worksheet.Cells[10 + tasks.Count + i, 1, 10 + tasks.Count + i, 4];
                    assignmentDataRange.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                }

                // Tự động điều chỉnh cột
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // Thêm ngày xuất
                worksheet.Cells[2, 5, 2, 6].Merge = true;
                worksheet.Cells[2, 5].Value = $"Export Date: {DateTime.Now:dd/MM/yyyy HH:mm:ss}";
                worksheet.Cells[2, 5].Style.Font.Italic = true;
                worksheet.Cells[2, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                // Lưu file
                File.WriteAllBytes(filePath, package.GetAsByteArray());
            }
        }
    }
}
