﻿using Microsoft.AspNetCore.Http.HttpResults;
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

        public async Task<Result> GetLeavePending()
        {
            Result result = new Result();

            try
            {
                result.IsSuccess = true;
                //TODO: Crear un repository en el cual obtengamos una lista de solicitudes que estén pendiente
                var leave = await _leaveRepository.PendingLeave();

                if (leave == null || leave.Any())
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
