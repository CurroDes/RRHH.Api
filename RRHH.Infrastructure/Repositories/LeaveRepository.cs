﻿using RRHH.Api;
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
    }
}
