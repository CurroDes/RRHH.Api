using Microsoft.Extensions.Logging;
using RRHH.Application.DTOs;
using RRHH.Application.Interfaces;
using RRHH.Application.Mapper;
using RRHH.Domain.Data;
using RRHH.Domain.Entities;
using RRHH.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRHH.Application.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly ApprrhhApiContext _context;
        private readonly DepartmentMapper _departmentMapper;
        private readonly ILogger<DepartmentService> _logger;
        private readonly IDepartmentRepository<Department> _departmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        public DepartmentService(ApprrhhApiContext context, DepartmentMapper departmentMapper, ILogger<DepartmentService> logger, IDepartmentRepository<Department> departmentRepository,
            IUnitOfWork unitOfWork)
        {
            _context = context;
            _departmentMapper = departmentMapper;
            _logger = logger;
            _departmentRepository = departmentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> PostDepartment(DepartmentDTO d)
        {
            Result result = new Result();
            try
            {
                var department = _departmentMapper.MapToDepartment(d);

                if (department == null)
                {
                    result.Error = "Ha habido un problema a la hora de crear un nuevo departamento";
                    _logger.LogError(result.ToString());
                }

                await _departmentRepository.AddAsync(department);
                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitAsync();

                result.Text = $"Se ha creado el nuevo departamento: {department.DepartmentName} con éxito";
                _logger.LogInformation(result.ToString());
            }
            catch (Exception ex)
            {
                result.Error = "Ha habido un problema a la hora de crear un nuevo departamento";
                _logger.LogError(result.ToString());
                await _unitOfWork.RollbackAsync();
            }

            return result;
        }

        public async Task<Result> GetDepartmentIdService(int id)
        {
            Result result = new Result();
            try
            {
                var departmentId = await _departmentRepository.DepartmentId(id);

                if (departmentId == null)
                {
                    result.Error = "Error al intentar obtener la información del departamento";
                    _logger.LogError(result.ToString());
                }

                result.GenericObject = departmentId;
                result.Text = "Se ha obtenido con éxito la información del departamento";
            }
            catch (Exception ex)
            {
                result.Error = "Error al intentar obtener la información del departamento";
                _logger.LogError(result.ToString());
            }

            return result;
        }
    }
}
