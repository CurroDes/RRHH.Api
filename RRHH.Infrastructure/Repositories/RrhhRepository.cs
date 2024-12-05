using Microsoft.EntityFrameworkCore;
using RRHH.Domain.Data;
using RRHH.Domain.Entities;
using RRHH.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRHH.Infrastructure.Repositories
{
    public class RrhhRepository<T> : IRrhhRepository<T> where T : Rrhh
    {

        private readonly ApprrhhApiContext _context;

        public RrhhRepository(ApprrhhApiContext context)
        {
            _context = context;
        }

        public async Task<T> GetIdAsync(int id)
        {
            return await _context.Set<T>()
                .Where(r => r.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task AddRrhhAsync(T rrhh)
        {
            await _context.AddAsync(rrhh);
        }
    }
}
