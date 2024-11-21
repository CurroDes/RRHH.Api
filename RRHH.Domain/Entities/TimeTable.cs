using System;
using System.Collections.Generic;

namespace RRHH.Api;

public partial class TimeTable
{
    public int Id { get; set; }

    public string? ScheduleType { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
