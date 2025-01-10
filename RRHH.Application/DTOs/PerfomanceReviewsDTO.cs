using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRHH.Application.DTOs
{
    public class PerformanceReviewsDTO
    {
        public int Id { get; set; }

        public int? EmployeeId { get; set; }

        public DateTime? ReviewDate { get; set; }

        public int? Score { get; set; }

        public string? Comments { get; set; }
    }
}
