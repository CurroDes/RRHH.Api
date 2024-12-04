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

        public Leaf MapToModifyApproved(LeaveDTO l, Leaf le)
        {
            le.EmployeeId = l.EmployeeId;
            le.StartDate = l.StartDate;
            le.EndDate = l.EndDate;
            le.LeaveType = l.LeaveType;
            le.Reason = l.Reason;
            le.Status = "Approved";
            le.Text = l.Text;

            return le;
        }

        public Leaf MapToModifyCancel(LeaveDTO l, Leaf le)
        {
            le.EmployeeId = l.EmployeeId;
            le.StartDate = l.StartDate;
            le.EndDate = l.EndDate;
            le.LeaveType = l.LeaveType;
            le.Reason = l.Reason;
            le.Status = "Cancel";
            le.Text = l.Text;

            return le;
        }

        public MessageDTO MapToMessageApi(Leaf l, MessageDTO m, Employee e)
        {
            m.FirstName = e.FirstName;
            m.LastName = e.LastName;
            m.Email = e.Email;
            m.LeaveType = l.LeaveType;
            m.Reason = l.Reason;
            m.Status = l.Status;

            return m;
        }
    }
}
