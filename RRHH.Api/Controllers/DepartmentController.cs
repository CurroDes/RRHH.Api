using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RRHH.Application.DTOs;
using RRHH.Application.Interfaces;
using RRHH.Domain.Entities;
using RRHH.Domain.Interfaces;

namespace RRHH.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartmentId(int id)
        {
            Result result = new Result();
            try
            {
                result = await _departmentService.GetDepartmentIdService(id);

                if(result.Error != null)
                {
                    return StatusCode(500, result.ToString());
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> PostDepartment(DepartmentDTO d)
        {
            Result result = new Result();

            try
            {
                result = await _departmentService.PostDepartment(d);

                if (result.Error != null)
                {
                    return StatusCode(500, result.Error);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, result.Error);
            }

            return Ok(result);
        }
    }
}
