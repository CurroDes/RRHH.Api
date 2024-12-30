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
    public class LeaveRepository<T> : ILeaveRepository<T> where T : Leaf
    {
        private readonly ApprrhhApiContext _context;
        public LeaveRepository(ApprrhhApiContext context)
        {
            _context = context;
        }

        public async Task AddLeaveAsync(T leave)
        {
            await _context.AddAsync(leave);
        }

        public async Task<List<Leaf>> PendingLeave()
        {
            return await _context.Leaves
                .Where(l => l.Status == "Pending")
                .ToListAsync();
        }


        public async Task<T> LeaveId(int id)
        {
            return await _context.Set<T>()
                .Where(l => l.Id == id)
                .FirstOrDefaultAsync();
        }

        //Añadimos dentro de leave la modificación:

        public async Task ModifyLeave(T leave)
        {
            _context.Entry(leave).State = EntityState.Modified;
        }
    }
}
