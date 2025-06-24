using System;
using System.Collections.Generic;

namespace assignment.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string? PasswordHash { get; set; }

    public int? RoleId { get; set; }

    public int? DepartmentId { get; set; }

    public string? Status { get; set; }

    public virtual Department? Department { get; set; }

    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();

    public virtual ICollection<EmployeeProject> EmployeeProjects { get; set; } = new List<EmployeeProject>();

    public virtual ICollection<LeaveRequest> LeaveRequestEmployees { get; set; } = new List<LeaveRequest>();

    public virtual ICollection<LeaveRequest> LeaveRequestManagers { get; set; } = new List<LeaveRequest>();

    public virtual ICollection<ProjectProgress> ProjectProgresses { get; set; } = new List<ProjectProgress>();

    public virtual Role? Role { get; set; }
}
