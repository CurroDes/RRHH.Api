using System;
using System.Collections.Generic;

namespace RRHH.Domain.Entities;

public partial class Employee
{
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public int? DepartmentId { get; set; }

    public string? Role { get; set; }

    public string? PhoneNumber { get; set; }

    public int? WorkingHoursId { get; set; }

    public string? Email { get; set; }

    public byte[]? Password { get; set; }

    public virtual Department? Department { get; set; }

    public virtual ICollection<Leaf> Leaves { get; set; } = new List<Leaf>();

    public virtual ICollection<PerformanceReview> PerformanceReviews { get; set; } = new List<PerformanceReview>();

    public virtual ICollection<Token> Tokens { get; set; } = new List<Token>();

    public virtual TimeTable? WorkingHours { get; set; }
}
