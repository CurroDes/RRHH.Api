using RRHH.Api;
using RRHH.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRHH.Infrastructure.Repositories
{
    public class EmployeesReposity<T> : IEmployeesRepository<T> where T : Employee
    {
        private readonly ApprrhhApiContext _context;
        public EmployeesReposity(ApprrhhApiContext context)
        {
            _context = context;
        }

        public async Task AddEmployeesAsync(T Employees)
        {
            await _context.AddAsync(Employees);
        }
    }
}
