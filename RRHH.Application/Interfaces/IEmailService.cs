using RRHH.Application.DTOs;
using RRHH.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRHH.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsyncApproved(Employee e);
        Task<Result> SendEmailAsyncCancel(Employee e);
        Task SendEmailRequest(Employee e);
        Task<Result> SendEmailToMultipleAsync(List<string> emails, TextDTO t);
        Task SendEmailReviewsApproved(Employee e, PerformanceReviewsDTO p);
    }
}
