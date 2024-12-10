using System;
using System.Collections.Generic;

namespace RRHH.Domain.Entities;

public partial class Token
{
    public int Id { get; set; }

    public string Token1 { get; set; } = null!;

    public int EmployeesId { get; set; }

    public DateTime CreatedAtToken { get; set; }

    public DateTime ExpirationDate { get; set; }

    public virtual Employee Employees { get; set; } = null!;
}
