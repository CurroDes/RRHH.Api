using RRHH.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRHH.Domain.Interfaces
{
    public interface IRrhhRepository<T> where T : Rrhh
    {
        Task<T> GetIdAsync(int id);
        Task AddRrhhAsync(T rrhh);
    }
}
