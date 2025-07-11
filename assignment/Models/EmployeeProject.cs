﻿using System;
using System.Collections.Generic;

namespace assignment.Models;

public partial class EmployeeProject
{
    public int EmployeeProjectId { get; set; }

    public int? EmployeeId { get; set; }

    public int? ProjectId { get; set; }

    public DateTime? AssignedAt { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual Project? Project { get; set; }
}
