using RRHH.Api;
using RRHH.Application.DTOs;
using RRHH.Application.Interfaces;
using RRHH.Application.Mapper;
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
        public EmployeesService(ApprrhhApiContext context, EmployeesMapper employeesMapper, IEmployeesRepository<Employee> employeesRepository, IUnitOfWork unitOfWork)
        {
            _context = context;
            _employeesMapper = employeesMapper;
            _employeesRepository = employeesRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> PostEmployeesService(EmployeesDTO e)
        {
            Result result = new Result();

            try
            {
                var employees = _employeesMapper.MapToEmployees(e);

                if (employees == null)
                {
                    result.Error = "Error al intentar añadir un nuevo empleado";
                    return result;
                }

                await _employeesRepository.AddEmployeesAsync(employees);
                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitAsync();

                result.Text = "Se ha añadido correctamente el nuevo empleado";
                //TODO:Añadir _logger Serilog
            }
            catch (Exception ex)
            {
                result.Error = "Error al intentar añadir un nuevo empleado";
                await _unitOfWork.RollbackAsync();
                return result;
            }

            return result;
        }
    }
}
