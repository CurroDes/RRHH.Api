using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRHH.Application.DTOs
{
    public class LeaveDTO
    {
        public int EmployeeId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string LeaveType { get; set; }

        public string Reason { get; set; }

        public string Status { get; set; }

        public int? ApproverId { get; set; }
        public string Text { get; set; }

    }
}
