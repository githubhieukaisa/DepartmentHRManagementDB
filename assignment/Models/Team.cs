using System;
using System.Collections.Generic;

namespace assignment.Models;

public partial class Team
{
    public int TeamId { get; set; }

    public string? TeamName { get; set; }

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? Status { get; set; }

    public DateTime? DoneAt { get; set; }

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

    public virtual ICollection<TeamMember> TeamMembers { get; set; } = new List<TeamMember>();
}
