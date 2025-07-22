using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace assignment.Models;

public partial class Project
{
    public int ProjectId { get; set; }

    public string? ProjectName { get; set; }

    public string? Description { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public int? TeamId { get; set; }
    [NotMapped]
    public bool? isSuccessful { get; set; }
    [NotMapped]
    public string? status 
    { 
        get
        {
            if (isSuccessful == null) return "Processing";
            if (isSuccessful == true) return "Success";
            return "Processing";
        }
    }
    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();

    public virtual Team? Team { get; set; }
}
