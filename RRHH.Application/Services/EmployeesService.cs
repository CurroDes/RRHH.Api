using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RRHH.Application.DTOs;
using RRHH.Application.Interfaces;
using RRHH.Application.Mapper;
using RRHH.Domain.Data;
using RRHH.Domain.Entities;
using RRHH.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRHH.Application.Services
{
    public class EmployeesService : IEmployeesService
    {

        private readonly ApprrhhApiContext _context;
        private readonly EmployeesMapper _employeesMapper;
        private readonly IEmployeesRepository<Employee> _employeesRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<EmployeesService> _logger;
        private readonly AuthenticationService _authenticationService;
        private readonly GenerateTokenService _generateToken;
        private readonly TokensMapper _tokensMapper;
        public EmployeesService(ApprrhhApiContext context, EmployeesMapper employeesMapper, IEmployeesRepository<Employee> employeesRepository, IUnitOfWork unitOfWork, ILogger<EmployeesService> logger,
            AuthenticationService authenticationService, GenerateTokenService generateToken, TokensMapper tokensMapper)
        {
            _context = context;
            _employeesMapper = employeesMapper;
            _employeesRepository = employeesRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _authenticationService = authenticationService;
            _generateToken = generateToken;
            _tokensMapper = tokensMapper;
        }

        public async Task<Result> GetEmployees()
        {
            Result result = new Result();

            try
            {
                result.IsSuccess = true;

                var getEmployees = await _employeesRepository.GetAllEmployeesAsync();

                if (getEmployees == null)
                {
                    result.IsSuccess = false;
                    result.Error = "Error al intentar obtener la lista de empleados";
                    _logger.LogError(result.Error.ToString());
                    return result;
                }

                result.GenericObject = getEmployees;

                result.Text = "Éxito al obtener la lista de empleados";
                _logger.LogInformation(result.Text.ToString());

            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Error = "Error al intentar obtener la lista de empleados";
                _logger.LogError(result.Error.ToString(), ex.ToString());
                return result;
            }

            return result;
        }

        public async Task<Result> PostEmployeesService(EmployeesDTO e)
        {
            Result result = new Result();

            try
            {
                result.IsSuccess = true;
                var employees = _employeesMapper.MapToEmployees(e);

                if (employees == null)
                {
                    result.IsSuccess = false;
                    result.Error = "Error al intentar añadir un nuevo empleado";
                    return result;
                }

                await _employeesRepository.AddEmployeesAsync(employees);
                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitAsync();

                result.Text = "Se ha añadido correctamente el nuevo empleado";
                _logger.LogInformation(result.Text.ToString());
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Error = "Error al intentar añadir un nuevo empleado";
                await _unitOfWork.RollbackAsync();
                return result;
            }

            return result;
        }

        //TODO: Pendiente logear el personal de RRHH y que obtenga el correspondiente JWT de permiso.
        public async Task<Result> LoginEmployees(int id, AuthApiViewModelDTO a)
        {
            Result result = new Result();
            Token token = new Token();

            try
            {
                result.IsSuccess = true;
                //Comprobamos si el empleado está en nuestra bbdd y es el que queremos realizar el login
                var employees = await _employeesRepository.GetEmployeeByIdAsync(id);

                if (employees == null)
                {
                    result.IsSuccess = false;
                    result.Error = $"Error al realizar el login con el email: {a.email}";
                    _logger.LogError(result.Error.ToString());

                    return result;
                }

                employees = await _employeesRepository.GetEmployeesEmailAsync(a.email);

                if (employees == null)
                {
                    result.IsSuccess = false;
                    result.Error = $"Error al realizar el login con el email: {a.email}";
                    _logger.LogError(result.Error.ToString());

                    return result;
                }

                //Comprobamos que el email y password son los idóneos y llevamos a cabo la authentication.
                employees = await _authenticationService.AuthenticationAsync(a.email, a.password);

                if (employees == null)
                {
                    result.IsSuccess = false;
                    result.Error = $"Error al intentar logear el email: {a.email}";
                    _logger.LogError(result.Error.ToString());
                    return result;
                }
                //Generamos el token
                token = _tokensMapper.MapToTokens(token, a, employees);

                await _context.Tokens.AddAsync(token);
                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Error = $"Error al intentar logear el email: {a.email}";
                _logger.LogError(result.Error.ToString(), ex.ToString());

                await _unitOfWork.RollbackAsync();

                return result;
            }

            return result;
        }

        public async Task<Result> PutEmployeesService(int id, EmployeesDTO e)
        {
            Result result = new Result();

            result.IsSuccess = true;
            try
            {
                //Condición para verificar que vamos a modificar el empleado deseado:
                var employees = await _employeesRepository.GetEmployeeByIdAsync(id);

                if (employees == null)
                {
                    result.IsSuccess = false;
                    result.Error = $"Error al intentar modificar al empleado {e.LastName}";
                    _logger.LogError(result.Error.ToString());
                    return result;
                }

                employees = _employeesMapper.MapToPutEmployees(e, employees);

                await _employeesRepository.ModifyEmployees(employees);
                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitAsync();

                result.Text = $"Éxito al modificar al cliente {e.FirstName}{e.LastName}";
                _logger.LogInformation(result.Text.ToString());
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Error = $"Error al intentar modificar al empleado {e.LastName}";
                _logger.LogError(result.Error.ToString(), ex.ToString());

                await _unitOfWork.RollbackAsync();

                return result;
            }

            return result;
        }
    }
}
