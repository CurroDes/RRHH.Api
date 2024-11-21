using System;
using System.Collections.Generic;

namespace RRHH.Api;

public partial class PerformanceReview
{
    public int Id { get; set; }

    public int? EmployeeId { get; set; }

    public DateTime? ReviewDate { get; set; }

    public int? Score { get; set; }

    public string? Comments { get; set; }

    public virtual Employee? Employee { get; set; }
}
