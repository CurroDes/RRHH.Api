using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RRHH.Application.DTOs;
using RRHH.Application.Interfaces;
using RRHH.Domain.Entities;

namespace RRHH.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveController : ControllerBase
    {
        private readonly ILeaveService _leaveService;
        public LeaveController(ILeaveService leaveService)
        {
            _leaveService = leaveService;
        }

        public async Task<IActionResult> PostLeave(LeaveDTO l)
        {
            Result result = new Result();

            try
            {
                result = await _leaveService.ValidateLeaveOverlap(l);

                if (!result.IsSuccess)
                {
                    return StatusCode(500, result.Error);
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
