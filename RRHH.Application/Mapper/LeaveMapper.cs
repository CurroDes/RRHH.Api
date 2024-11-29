using RRHH.Application.DTOs;
using RRHH.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRHH.Application.Mapper
{
    public class LeaveMapper
    {
        public LeaveMapper()
        {
        }

        public Leaf MapToLeave(LeaveDTO l)
        {
            return new Leaf
            {
                EmployeeId = l.EmployeeId,
                StartDate = l.StartDate,
                EndDate = l.EndDate,
                LeaveType = l.LeaveType,
                Reason = l.Reason,
                Status = l.Status,
                Text = l.Text
            };
        }
    }
}
