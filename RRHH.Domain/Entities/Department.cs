﻿using System;
using System.Collections.Generic;

namespace RRHH.Api;

public partial class Department
{
    public int Id { get; set; }

    public string? DepartmentName { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
