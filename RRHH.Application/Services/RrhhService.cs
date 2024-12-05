using Microsoft.Extensions.Logging;
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

namespace RRHH.Application.Services;

public class RrhhService : IRrhhService
{
    private readonly ILogger<RrhhService> _logger;
    private readonly IRrhhRepository<Rrhh> _rrhhRepository;
    private readonly RrhhMapper _rrhhMapper;
    private readonly IUnitOfWork _unitOfWork;
    public RrhhService(ILogger<RrhhService> logger, IRrhhRepository<Rrhh> rrhhRepository, RrhhMapper rrhhMapper, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _rrhhRepository = rrhhRepository;
        _rrhhMapper = rrhhMapper;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result> PostRrhhService(RrhhDTO r)
    {
        Result result = new Result();

        try
        {
            result.IsSuccess = true;

            var mapperHumans = _rrhhMapper.MaptoRrhh(r);

            if (mapperHumans == null)
            {
                result.IsSuccess = false;
                result.Error = "Error al intentar crear el nuevo empleado de recursos humanos.";
                _logger.LogError(result.Error.ToString());

                return result;
            }

            //Hay que solucionar la BBDD con la relación entre la tabla RRHH y Employees.

            await _rrhhRepository.AddRrhhAsync(mapperHumans);
            await _unitOfWork.SaveChangesAsync();

            await _unitOfWork.CommitAsync();

            result.Text = $"Se ha registrado el nuevo empleado de reursos humanos correctamente {r.Name} con email: {r.Email}";
            _logger.LogInformation(result.Text.ToString());

        }
        catch (Exception ex)
        {
            result.IsSuccess = false;
            result.Error = "Error al intentar crear el nuevo empleado de recursos humanos.";
            _logger.LogError(result.Error.ToString(), ex.ToString());
        }

        return result;
    }
}
