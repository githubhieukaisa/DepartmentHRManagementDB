using System;
using System.Collections.Generic;

namespace assignment.Models;

public partial class TaskStatus
{
    public int StatusId { get; set; }

    public string? StatusName { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
