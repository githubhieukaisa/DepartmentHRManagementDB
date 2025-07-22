using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace assignment.Models;

public partial class Task
{
    public int TaskId { get; set; }

    public int? ProjectId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public int? StatusId { get; set; }

    public int? ReporterId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Project? Project { get; set; }

    public virtual Employee? Reporter { get; set; }

    public virtual TaskStatus? Status { get; set; }

    public virtual ICollection<TaskAssignment> TaskAssignments { get; set; } = new List<TaskAssignment>();
}
