using Microsoft.Extensions.Logging;
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
    public class LeavesService : ILeaveService
    {
        private readonly ApprrhhApiContext _context;
        private readonly LeaveMapper _leaveMapper;
        private readonly ILeaveRepository<Leaf> _leaveRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<LeavesService> _logger;
        public LeavesService(ApprrhhApiContext context, LeaveMapper leaveMapper, ILeaveRepository<Leaf> leaveRepository, IUnitOfWork unitOfWork, ILogger<LeavesService> logger)
        {
            _context = context;
            _leaveMapper = leaveMapper;
            _leaveRepository = leaveRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
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
    }
}
