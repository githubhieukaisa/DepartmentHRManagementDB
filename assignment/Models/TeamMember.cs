using System;
using System.Collections.Generic;

namespace assignment.Models;

public partial class TeamMember
{
    public int TeamMemberId { get; set; }

    public int? TeamId { get; set; }

    public int? EmployeeId { get; set; }

    public DateTime? JoinedAt { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual Team? Team { get; set; }
}
