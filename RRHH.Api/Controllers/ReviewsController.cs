using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RRHH.Application.DTOs;
using RRHH.Application.Interfaces;
using RRHH.Domain.Entities;

namespace RRHH.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IPerformanceReviewsService _performanceReviewsService;

        public ReviewsController(IPerformanceReviewsService performanceReviewsService)
        {
            _performanceReviewsService = performanceReviewsService;
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> PostReviewsEmployees(int id, PerformanceReviewsDTO p)
        {
            Result result = new Result();

            try
            {
                result = await _performanceReviewsService.PostReviewsService(id, p);

                if (!result.IsSuccess)
                {
                    return StatusCode(500, result.Error.ToString());
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }

            return Ok(result);
        }
    }
}
