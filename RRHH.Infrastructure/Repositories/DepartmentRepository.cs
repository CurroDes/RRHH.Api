using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using RRHH.Api;
using RRHH.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace RRHH.Infrastructure.Repositories
{
    public class DepartmentRepository<T> : IDepartmentRepository<T> where T : Department
    {
        private readonly ApprrhhApiContext _context;
        public DepartmentRepository(ApprrhhApiContext context)
        {
            _context = context;
        }

        public async Task AddAsync(T Department)
        {
            await _context.AddAsync(Department);
        }

        public async Task<List<Employee>> DepartmentId(int id)
        {
            return await _context.Employees
                .Where(e => e.DepartmentId == id)
                .ToListAsync();
        }
    }
}
