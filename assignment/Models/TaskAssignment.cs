using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace assignment.Models;

public partial class TaskAssignment
{
    public int TaskAssignmentId { get; set; }

    public int? TaskId { get; set; }

    public int? EmployeeId { get; set; }

    public int? RoleId { get; set; }

    public DateTime? AssignedAt { get; set; }

    [NotMapped]
    public List<TaskStatus> allStatusAvailable { get; set; }
    public virtual Employee? Employee { get; set; }

    public virtual Role? Role { get; set; }

    public virtual Task? Task { get; set; }
}
