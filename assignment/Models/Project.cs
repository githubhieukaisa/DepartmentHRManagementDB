using System;
using System.Collections.Generic;

namespace assignment.Models;

public partial class Project
{
    public int ProjectId { get; set; }

    public string? ProjectName { get; set; }

    public string? Description { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public virtual ICollection<EmployeeProject> EmployeeProjects { get; set; } = new List<EmployeeProject>();

    public virtual ICollection<ProjectProgress> ProjectProgresses { get; set; } = new List<ProjectProgress>();
}
