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
        Task SendEmailAsyncApprovedd(EmployeesDTO e);
        Task SendEmailAsyncCancel(EmployeesDTO e);
    }
}
