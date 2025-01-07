using Microsoft.Extensions.Logging;
using RRHH.Application.DTOs;
using RRHH.Application.Interfaces;
using RRHH.Domain.Entities;
using RRHH.Domain.Interfaces;
using RRHH.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRHH.Application.Services;

public class MassMessagingService
{

    private readonly IEmployeesRepository<Employee> _employeesRepository;
    private readonly ILogger<MassMessagingService> _logger;
    private readonly IEmailService _emailService;
    public MassMessagingService(IEmployeesRepository<Employee> employeesRepository, ILogger<MassMessagingService> logger, IEmailService emailService)
    {
        _employeesRepository = employeesRepository;
        _logger = logger;
        _emailService = emailService;
    }

    public async Task<Result> AllMassMessagingService(TextDTO t)
    {
        Result result = new Result();

        try
        {

            //TODO: Traernos todos los emails de todos los empleados (Recuerdo: De momento solo podemos enviar mensaje al mio personal).
            result.IsSuccess = true;
            var emails = await _employeesRepository.AllEmailEmployeesAsync();

            //TODO: Comprobar si ha sido posible mediante linq obtener todos los emails.
            if (emails == null)
            {
                result.IsSuccess &= false;
                result.Error = "Error al intentar mandar correos de forma masiva a todos los empleados";
                _logger.LogError(result.ToString());
                return result;
            }

            //TODO: Llamar al servicio de mensajería y mandar los o el correo correspondiente.

            result = await _emailService.SendEmailToMultipleAsync(emails, t);

            if (!result.IsSuccess)
            {
                result.IsSuccess &= false;
                result.Error = "Error al intentar mandar correos de forma masiva a todos los empleados";
                _logger.LogError(result.ToString());
                return result;
            }
        }
        catch (Exception ex)
        {
            result.IsSuccess &= false;
            result.Error = "Error al intentar mandar correos de forma masiva a todos los empleados";
            _logger.LogError(result.ToString(), ex.ToString());
            return result;

        }

        return result;
    }
}