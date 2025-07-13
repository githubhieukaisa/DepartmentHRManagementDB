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

    public int? TeamId { get; set; }

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();

    public virtual Team? Team { get; set; }
}
