using System;
using System.Collections.Generic;

namespace RRHH.Domain.Entities;

public partial class Rrhh
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public byte[]? Password { get; set; }

    public string? Email { get; set; }

    public int DepartmentId { get; set; }

    public virtual Employee? Department { get; set; }
}
