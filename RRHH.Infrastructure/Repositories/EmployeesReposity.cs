using Microsoft.EntityFrameworkCore;
using RRHH.Domain.Data;
using RRHH.Domain.Entities;
using RRHH.Domain.Interfaces;
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

        public async Task<T> GetEmployeeByIdAsync(int id)
        {
            return await _context.Set<T>()
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);

        }
    }
}
