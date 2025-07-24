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
    public class AllProjectsExcelExporter : IExcelExporter
    {
        public void ExportToExcel(Dictionary<string, object> data, string filePath)
        {
            if (data == null || !data.Any())
            {
                throw new ArgumentException("Data cannot be null or empty", nameof(data));
            }

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var projects = data["Projects"] as List<Project>;
            var allTasks = data["Tasks"] as List<Models.Task>;
            var allTaskAssignments = data["TaskAssignments"] as List<TaskAssignment>;

            if (projects == null || allTasks == null || allTaskAssignments == null)
            {
                throw new ArgumentException("Invalid data format: 'Projects', 'Tasks', or 'TaskAssignments' key is missing or invalid.", nameof(data));
            }

            using (var package = new ExcelPackage())
            {
                // Sheet 1: Project Overview
                var projectSheet = package.Workbook.Worksheets.Add("Project Overview");
                projectSheet.Cells[1, 1].Value = "Project Overview";
                projectSheet.Cells[1, 1, 1, 6].Merge = true;
                projectSheet.Cells[1, 1].Style.Font.Bold = true;
                projectSheet.Cells[1, 1].Style.Font.Size = 16;
                projectSheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                var projectHeaders = new string[] { "ID", "Project Name", "Team", "Start Date", "End Date", "Status" };
                for (int i = 0; i < projectHeaders.Length; i++)
                {
                    projectSheet.Cells[2, i + 1].Value = projectHeaders[i];
                }
                var projectHeaderRange = projectSheet.Cells[2, 1, 2, projectHeaders.Length];
                projectHeaderRange.Style.Font.Bold = true;
                projectHeaderRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                projectHeaderRange.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                projectHeaderRange.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                for (int i = 0; i < projects.Count; i++)
                {
                    var project = projects[i];
                    projectSheet.Cells[i + 3, 1].Value = project.ProjectId;
                    projectSheet.Cells[i + 3, 2].Value = project.ProjectName ?? "N/A";
                    projectSheet.Cells[i + 3, 3].Value = project.Team?.TeamName ?? "N/A";
                    projectSheet.Cells[i + 3, 4].Value = project.StartDate?.ToString("yyyy-MM-dd") ?? "N/A";
                    projectSheet.Cells[i + 3, 5].Value = project.EndDate?.ToString("yyyy-MM-dd") ?? "N/A";
                    projectSheet.Cells[i + 3, 6].Value = project.Team?.Status ?? "N/A";

                    var projectDataRange = projectSheet.Cells[i + 3, 1, i + 3, 6];
                    projectDataRange.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                }

                // Sheet 2: Tasks
                var taskSheet = package.Workbook.Worksheets.Add("Tasks");
                taskSheet.Cells[1, 1].Value = "Tasks";
                taskSheet.Cells[1, 1, 1, 6].Merge = true;
                taskSheet.Cells[1, 1].Style.Font.Bold = true;
                taskSheet.Cells[1, 1].Style.Font.Size = 16;
                taskSheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                var taskHeaders = new string[] { "Project Name", "Task ID", "Title", "Description", "Status", "Reporter", "Created At" };
                for (int i = 0; i < taskHeaders.Length; i++)
                {
                    taskSheet.Cells[2, i + 1].Value = taskHeaders[i];
                }
                var taskHeaderRange = taskSheet.Cells[2, 1, 2, taskHeaders.Length];
                taskHeaderRange.Style.Font.Bold = true;
                taskHeaderRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                taskHeaderRange.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                taskHeaderRange.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                var projectDict = projects.ToDictionary(p => p.ProjectId, p => p.ProjectName ?? "N/A");
                for (int i = 0; i < allTasks.Count; i++)
                {
                    var task = allTasks[i];
                    taskSheet.Cells[i + 3, 1].Value = projectDict[task.ProjectId.Value];
                    taskSheet.Cells[i + 3, 2].Value = task.TaskId;
                    taskSheet.Cells[i + 3, 3].Value = task.Title ?? "N/A";
                    taskSheet.Cells[i + 3, 4].Value = task.Description ?? "N/A";
                    taskSheet.Cells[i + 3, 5].Value = task.Status?.StatusName ?? "N/A";
                    taskSheet.Cells[i + 3, 6].Value = task.Reporter?.FullName ?? "N/A";
                    taskSheet.Cells[i + 3, 7].Value = task.CreatedAt?.ToString("yyyy-MM-dd HH:mm:ss") ?? "N/A";

                    var taskDataRange = taskSheet.Cells[i + 3, 1, i + 3, 7];
                    taskDataRange.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                }

                // Sheet 3: Task Assignments
                var assignmentSheet = package.Workbook.Worksheets.Add("Task Assignments");
                assignmentSheet.Cells[1, 1].Value = "Task Assignments";
                assignmentSheet.Cells[1, 1, 1, 5].Merge = true;
                assignmentSheet.Cells[1, 1].Style.Font.Bold = true;
                assignmentSheet.Cells[1, 1].Style.Font.Size = 16;
                assignmentSheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                var assignmentHeaders = new string[] { "Task ID", "Assignee", "Role", "Assigned At", "Project Name" };
                for (int i = 0; i < assignmentHeaders.Length; i++)
                {
                    assignmentSheet.Cells[2, i + 1].Value = assignmentHeaders[i];
                }
                var assignmentHeaderRange = assignmentSheet.Cells[2, 1, 2, assignmentHeaders.Length];
                assignmentHeaderRange.Style.Font.Bold = true;
                assignmentHeaderRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                assignmentHeaderRange.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                assignmentHeaderRange.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                for (int i = 0; i < allTaskAssignments.Count; i++)
                {
                    var assignment = allTaskAssignments[i];
                    var task = allTasks.FirstOrDefault(t => t.TaskId == assignment.TaskId);
                    assignmentSheet.Cells[i + 3, 1].Value = assignment.TaskId;
                    assignmentSheet.Cells[i + 3, 2].Value = assignment.Employee?.FullName ?? "N/A";
                    assignmentSheet.Cells[i + 3, 3].Value = assignment.Role?.RoleName ?? "N/A";
                    assignmentSheet.Cells[i + 3, 4].Value = assignment.AssignedAt?.ToString("yyyy-MM-dd HH:mm:ss") ?? "N/A";
                    assignmentSheet.Cells[i + 3, 5].Value = projectDict[task?.ProjectId ?? 0] ?? "N/A";

                    var assignmentDataRange = assignmentSheet.Cells[i + 3, 1, i + 3, 5];
                    assignmentDataRange.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                }

                // Tự động điều chỉnh cột cho tất cả sheet
                foreach (var worksheet in package.Workbook.Worksheets)
                {
                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                }

                // Thêm ngày xuất vào sheet đầu tiên
                projectSheet.Cells[3, 5, 3, 6].Merge = true;
                projectSheet.Cells[3, 5].Value = $"Export Date: {DateTime.Now:dd/MM/yyyy HH:mm:ss}"; // 01:24 PM +07, 24/07/2025
                projectSheet.Cells[3, 5].Style.Font.Italic = true;
                projectSheet.Cells[3, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                // Lưu file
                File.WriteAllBytes(filePath, package.GetAsByteArray());
            }
        }
    }
}
