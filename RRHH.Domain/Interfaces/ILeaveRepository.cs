using RRHH.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRHH.Domain.Interfaces
{
    public interface ILeaveRepository<T> where T : Leaf
    {
        Task AddLeaveAsync(T leave);
        Task<List<Leaf>> PendingLeave();
        Task<T> LeaveId(int id);
        Task ModifyLeave(T leave);
    }
}
