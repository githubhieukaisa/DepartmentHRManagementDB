using System;
using System.Collections.Generic;

namespace assignment.Models;

public partial class ProjectProgress
{
    public int ProgressId { get; set; }

    public int? EmployeeId { get; set; }

    public int? ProjectId { get; set; }

    public string? ProgressNote { get; set; }

    public DateOnly? ProgressDate { get; set; }

    public int? PercentageComplete { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual Project? Project { get; set; }
}
