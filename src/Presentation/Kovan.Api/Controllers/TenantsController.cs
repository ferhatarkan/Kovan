using Kovan.Application.Features.Tenants.Commands.UpdateTenantSettings;
using Kovan.Application.Features.Tenants.Commands.InviteUser;
using Kovan.Application.Features.Tenants.Queries.GetPaginatedTenantUsers;
using Kovan.Application.Features.Tenants.Queries.GetTenantSettings;
using Kovan.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kovan.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = Roles.Admin)] // Bu endpoint'e sadece Admin'ler erişebilir.
public class TenantsController : ControllerBase
{
    private readonly ISender _sender;

    public TenantsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("settings")]
    public async Task<IActionResult> GetSettings()
    {
        var settings = await _sender.Send(new GetTenantSettingsQuery());
        return Ok(settings);
    }

    [HttpPut("settings")]
    public async Task<IActionResult> UpdateSettings(UpdateTenantSettingsCommand command)
    {
        await _sender.Send(command);
        return NoContent();
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetUsers([FromQuery] GetPaginatedTenantUsersQuery query)
    {
        var users = await _sender.Send(query);
        return Ok(users);
    }

    [HttpPost("invitations")]
    public async Task<IActionResult> InviteUser(InviteUserCommand command)
    {
        await _sender.Send(command);
        return Ok(new { Message = "Davet başarıyla gönderildi." });
    }
}