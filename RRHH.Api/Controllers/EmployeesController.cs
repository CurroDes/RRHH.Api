using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RRHH.Application.DTOs;
using RRHH.Application.Interfaces;
using RRHH.Domain.Entities;


namespace RRHH.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeesService _employeesService;

        public EmployeesController(IEmployeesService employeesService)
        {
            _employeesService = employeesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            Result result = new Result();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> PostEmployees(EmployeesDTO e)
        {
            Result result = new Result();

            try
            {
                result = await _employeesService.PostEmployeesService(e);

                if (result.Error != null)
                {
                    return StatusCode(500, result);
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
