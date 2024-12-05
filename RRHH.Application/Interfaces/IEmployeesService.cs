using RRHH.Application.DTOs;
using RRHH.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRHH.Application.Interfaces
{
    public interface IEmployeesService
    {
        Task<Result> PostEmployeesService(EmployeesDTO e);
        Task<Result> GetEmployees();
    }
}
