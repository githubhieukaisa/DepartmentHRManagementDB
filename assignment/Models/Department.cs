using System;
using System.Collections.Generic;

namespace assignment.Models;

public partial class Department
{
    public int DepartmentId { get; set; }

    public string? DepartmentName { get; set; }

    public string? Description { get; set; }

    public int? ManagerId { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual Employee? Manager { get; set; }
}
