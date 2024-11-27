
using RRHH.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRHH.Domain.Interfaces
{
    public interface IDepartmentRepository<T> where T : Department
    {
        Task AddAsync(T Department);
        Task<List<Employee>> DepartmentId(int id);
    }
}
