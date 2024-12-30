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


        [HttpGet]
        public async Task<IActionResult> GetLeaves()
        {
            Result result = new Result();

            //Tenemos que obtener una lista de peticiones/solicitudes pendientes por parte de los empleados (Seguir pinrcipios SOLID)
            try
            {
                result = await _leaveService.GetLeavePending();

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

        [HttpPost]
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

        [HttpPut("{id}/Approved")]
        public async Task<IActionResult> PutLeaveApproved(int id, LeaveDTO l)
        {
            Result result = new Result();

            try
            {
                result = await _leaveService.ValidateLeaveApproved(id, l);

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

        [HttpPut("{id}/Cancel")]
        public async Task<IActionResult> PutLeaveCancel(int id, LeaveDTO l)
        {
            Result result = new Result();

            try
            {
                result = await _leaveService.PutLeaveCancel(id, l);

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
