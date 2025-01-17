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

public class PerformanceReviewsService : IPerformanceReviewsService
{
    private readonly IEmployeesRepository<Employee> _employeesRepository;
    private readonly ILogger<PerformanceReviewsService> _logger;
    private readonly ReviewsMapper _reviewsMapper;
    private readonly IPerformanceReviewsRepository<PerformanceReview> _performanceReviewRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;

    public PerformanceReviewsService(IEmployeesRepository<Employee> employeesRepository, ILogger<PerformanceReviewsService> logger,
        ReviewsMapper reviewsMapper, IPerformanceReviewsRepository<PerformanceReview> performanceReviewsRepository, IUnitOfWork unitOfWork, IEmailService emailService)
    {
        _employeesRepository = employeesRepository;
        _logger = logger;
        _reviewsMapper = reviewsMapper;
        _performanceReviewRepository = performanceReviewsRepository;
        _unitOfWork = unitOfWork;
        _emailService = emailService;
    }


    //Vamos a recibir una petición de revisión sobre un empleado en concreto.
    public async Task<Result> PostReviewsService(int id, PerformanceReviewsDTO p)
    {
        Result result = new Result();

        try
        {
            result.IsSuccess = true;


            ////Comprobamos que es el mismo empleado al que se le está haciendo la revisión.
            var employees = await _employeesRepository.GetEmployeeByIdAsync(id);

            if (employees.Id != p.EmployeeId)
            {
                result.IsSuccess = false;
                result.Error = "Error al localizar al empleado para su correspondiente review";
                _logger.LogError(result.Error.ToString());

                return result;
            }

            //Mappeamos la review

            var reviewEmployee = _reviewsMapper.MapToReview(p, new PerformanceReview());


            //TODO:Llámamos al servicio de mensajería con smtp para notificar al empleado que tiene disponible la revisión en la app de recursos humanos.
            var message = _emailService.SendEmailReviewsApproved(employees, p);

            //Guardamos cambios en bbdd.
            await _performanceReviewRepository.AddReviewAsync(reviewEmployee);

            await _unitOfWork.SaveChangesAsync();

            await _unitOfWork.CommitAsync();

        }
        catch (Exception ex)
        {
            result.IsSuccess = false;
            result.Error = "Error al localizar al empleado para su correspondiente review";
            _logger.LogError(result.Error.ToString(), ex.ToString());
            await _unitOfWork.RollbackAsync();
            return result;

        }

        return result;


    }
}