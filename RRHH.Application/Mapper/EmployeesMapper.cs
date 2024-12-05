
using RRHH.Application.DTOs;
using RRHH.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRHH.Application.Mapper
{
    public class EmployeesMapper
    {
        public Employee MapToEmployees(EmployeesDTO e)
        {
            return new Employee
            {
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                DepartmentId = e.DepartmentId,
                Role = e.Role,
                PhoneNumber = e.PhoneNumber,
                WorkingHoursId = e.WorkingHoursId
            };
        }
    }
}
