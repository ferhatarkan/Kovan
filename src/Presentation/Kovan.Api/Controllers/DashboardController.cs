using Kovan.Application.Features.Dashboard.Queries;
using Kovan.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kovan.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = Roles.Admin)] // Bu endpoint'e sadece Admin'ler erişebilir.
public class DashboardController : ControllerBase
{
    private readonly ISender _sender;

    public DashboardController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("stats")]
    public async Task<IActionResult> GetStats()
    {
        var stats = await _sender.Send(new GetDashboardStatsQuery());
        return Ok(stats);
    }
}