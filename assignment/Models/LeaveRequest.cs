using System;
using System.Collections.Generic;

namespace assignment.Models;

public partial class LeaveRequest
{
    public int LeaveRequestId { get; set; }

    public int? EmployeeId { get; set; }

    public DateOnly? FromDate { get; set; }

    public DateOnly? ToDate { get; set; }

    public string? Reason { get; set; }

    public string? Status { get; set; }

    public int? ManagerId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual Employee? Manager { get; set; }
}
