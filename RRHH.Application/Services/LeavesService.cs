using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using RRHH.Application.DTOs;
using RRHH.Application.Interfaces;
using RRHH.Application.Mapper;
using RRHH.Domain.Data;
using RRHH.Domain.Entities;
using RRHH.Domain.Interfaces;
using RRHH.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRHH.Application.Services;

public class LeavesService : ILeaveService
{
    private readonly ApprrhhApiContext _context;
    private readonly LeaveMapper _leaveMapper;
    private readonly ILeaveRepository<Leaf> _leaveRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<LeavesService> _logger;
    private readonly IEmployeesRepository<Employee> _employeesRepository;
    private readonly IMessageService _messageService;
    private readonly IEmailService _emailService;
    public LeavesService(ApprrhhApiContext context, LeaveMapper leaveMapper, ILeaveRepository<Leaf> leaveRepository, IUnitOfWork unitOfWork, ILogger<LeavesService> logger,
        IEmployeesRepository<Employee> employeesRepository, IMessageService messageService, IEmailService emailService)
    {
        _context = context;
        _leaveMapper = leaveMapper;
        _leaveRepository = leaveRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _employeesRepository = employeesRepository;
        _messageService = messageService;
        _emailService = emailService;
    }

    public async Task<Result> GetLeavePending()
    {
        Result result = new Result();

        try
        {
            result.IsSuccess = true;
            var leave = await _leaveRepository.PendingLeave();

            if (leave == null)
            {
                result.IsSuccess = false;
                result.Error = "Error al recibir lista de peteciones pendientes por aprobar";
                _logger.LogError(result.Error.ToString());

                return result;
            }

            result.GenericObject = leave;
            result.Text = "Se ha obtenido la lista de solicitudes pendiente con éxito";
            _logger.LogInformation(result.Text.ToString());
        }
        catch (Exception ex)
        {
            result.IsSuccess = false;
            result.Error = "Error al recibir lista de peteciones pendientes por aprobar";
            _logger.LogError(result.Error.ToString());
        }

        return result;
    }

    public async Task<Result> ValidateLeaveOverlap(LeaveDTO l)
    {
        Result result = new Result();

        try
        {
            var reasonLeave = _leaveMapper.MapToLeave(l);

            if (reasonLeave == null)
            {
                result.IsSuccess = false;
                return result;
            }

            await _leaveRepository.AddLeaveAsync(reasonLeave);
            await _unitOfWork.SaveChangesAsync();

            var employee = await _employeesRepository.GetEmployeeByIdAsync(l.EmployeeId);

            if(employee == null)
            {
                result.IsSuccess = false;
                result.Error = "No se encontró el empleado asociado a esta solicitud.";
                _logger.LogError(result.Error.ToString());
                return result;
            }

            await _emailService.SendEmailRequest(employee);

            await _unitOfWork.CommitAsync();

            result.IsSuccess = true;
            result.Text = "Se ha registrado el mensaje de leave correctamente";
            _logger.LogInformation(result.Text.ToString());
        }
        catch (Exception ex)
        {
            result.IsSuccess = false;
            result.Error = "Error al intentar guardar el leave";

            await _unitOfWork.RollbackAsync();
        }

        return result;
    }

    public async Task<Result> ValidateLeaveApproved(int id, LeaveDTO l)
    {
        Result result = new Result();

        try
        {
            result.IsSuccess = true;

            var leaveId = await _leaveRepository.LeaveId(id);

            if (leaveId == null)
            {
                result.IsSuccess = false;
                result.Error = "Error al intentar aceptar la solicitud, no es el mismo trabajador. Por favor, revise la petición";
                _logger.LogError(result.Error.ToString());
                return result;
            }

            await _unitOfWork.SaveChangesAsync();

            //Vamos a preparar el email de confirmación para el empleado:

            var employee = await _employeesRepository.GetEmployeeByIdAsync(l.EmployeeId);

            if (employee == null)
            {
                result.IsSuccess = false;
                result.Error = "No se encontró el empleado asociado a esta solicitud.";
                _logger.LogError(result.Error.ToString());
                return result;
            }

            await _emailService.SendEmailAsyncApproved(employee);

            leaveId = _leaveMapper.MapToModifyApproved(l, leaveId);

            await _leaveRepository.ModifyLeave(leaveId);

            await _unitOfWork.CommitAsync();

            result.Text = $"Se ha modificado con éxito la solicitud del empelado con id: {l.EmployeeId}";
            _logger.LogInformation(result.Text.ToString());

        }
        catch (Exception ex)
        {
            result.IsSuccess = false;
            result.Error = "Error al intentar modificar el estado de la solicitud";
            _logger.LogError(result.Error.ToString());

            await _unitOfWork.RollbackAsync();
        }

        return result;
    }

    public async Task<Result> PutLeaveCancel(int id, LeaveDTO l)
    {
        Result result = new Result();

        try
        {
            result.IsSuccess = true;

            var leaveCancel = await _leaveRepository.LeaveId(id);

            if (leaveCancel == null)
            {
                result.IsSuccess = false;
                result.Error = "Ha habido un error al intentar cancelar la solicitud del empleado, por favor, revise la petición";
                _logger.LogError(result.Error.ToString());

                return result;
            }

            //mapeamos el nuevo status
            leaveCancel = _leaveMapper.MapToModifyCancel(l, leaveCancel);
            await _leaveRepository.ModifyLeave(leaveCancel);
            await _unitOfWork.SaveChangesAsync();

            //Mapeamos el mensaje que vamos a enviar a la API.MESSAGE:
            var employee = await _employeesRepository.GetEmployeeByIdAsync(l.EmployeeId);


            if (employee == null)
            {
                result.IsSuccess = false;
                result.Error = "No se encontró el empleado asociado a esta solicitud.";
                _logger.LogError(result.Error.ToString());
                return result;
            }

            result = await _emailService.SendEmailAsyncCancel(employee);

            if (!result.IsSuccess)
            {
                result.IsSuccess = false;
                result.Error = $"No se puedo enviar el mensaje al empleado {employee.FirstName} {employee.LastName} con la solicitud cancelada.";
                _logger.LogError(result.Error.ToString());
                return result;
            }

            await _unitOfWork.CommitAsync();
            result.Text = $"Se ha modificado la solicitud a cancelada correctamente para el empleado con id: {l.EmployeeId}";
            _logger.LogInformation(result.Text.ToString());
        }
        catch (Exception ex)
        {
            result.IsSuccess = false;
            result.Error = "Ha habido un error al intentar cancelar la solicitud del empleado, por favor, revise la petición";
            _logger.LogError(result.Error.ToString());

            return result;
        }

        return result;
    }
}
