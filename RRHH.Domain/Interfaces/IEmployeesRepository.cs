using RRHH.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRHH.Domain.Interfaces
{
    public interface IEmployeesRepository<T> where T : Employee
    {
        Task AddEmployeesAsync(T Employees);
    }
}
