using RRHH.Application.DTOs;
using RRHH.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRHH.Application.Interfaces
{
    public interface ILeaveService
    {
        Task<Result> ValidateLeaveOverlap(LeaveDTO l);
        Task<Result> GetLeavePending();
        Task<Result> ValidateLeaveApproved(int id, LeaveDTO l);
        Task<Result> PutLeaveCancel(int id, LeaveDTO l);
    }
}
