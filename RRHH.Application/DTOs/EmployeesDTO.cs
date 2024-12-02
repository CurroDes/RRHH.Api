using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRHH.Application.DTOs
{
    public class EmployeesDTO
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public int? DepartmentId { get; set; }
        public string Email { get; set; }

        public string? Role { get; set; }

        public string? PhoneNumber { get; set; }

        public int? WorkingHoursId { get; set; }
    }
}
