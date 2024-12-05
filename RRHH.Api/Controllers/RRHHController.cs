using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RRHH.Application.DTOs;
using RRHH.Application.Interfaces;
using RRHH.Domain.Entities;

namespace RRHH.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RRHHController : ControllerBase
{
    private readonly IRrhhService _rrhhService;
    public RRHHController(IRrhhService rrhhService)
    {
        _rrhhService = rrhhService;
    }

    [HttpPost]
    public async Task<IActionResult> PostRrhh(RrhhDTO r)
    {
        Result result = new Result();

        try
        {
            result = await _rrhhService.PostRrhhService(r);

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
