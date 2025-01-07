using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RRHH.Application.DTOs;
using RRHH.Application.Services;
using RRHH.Domain.Entities;

namespace RRHH.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MassMessagingController : ControllerBase
{
    private readonly MassMessagingService _massMessagingService;
    public MassMessagingController(MassMessagingService massMessagingService)
    {
        _massMessagingService = massMessagingService;
    }

    [HttpPost]
    public async Task<IActionResult> AllMassMessaging(TextDTO t)
    {
        Result result = new Result();

        try
        {
            result = await _massMessagingService.AllMassMessagingService(t);

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
