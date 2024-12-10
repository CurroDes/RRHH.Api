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

        public async Task<List<T>> GetAllEmployeesAsync()
        {
            return await _context.Set<T>()
                .Include(e => e.Department)
                .Include(e => e.Leaves)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<T> GetEmployeesEmailAsync(string email)
        {
            return await _context.Set<T>()
                .SingleOrDefaultAsync(e => e.Email == email);
        }

        public async Task ModifyEmployees(T employees)
        {
            _context.Entry(employees).State = EntityState.Modified;
        }
    }
}
