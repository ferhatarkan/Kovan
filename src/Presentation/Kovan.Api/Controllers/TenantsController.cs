using Kovan.Application.Features.Tenants.Commands.UpdateTenantSettings;
using Kovan.Application.Features.Tenants.Commands.InviteUser;
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

    [HttpPut("settings")]
    public async Task<IActionResult> UpdateSettings(UpdateTenantSettingsCommand command)
    {
        await _sender.Send(command);
        return NoContent();
    }

    [HttpPost("invitations")]
    public async Task<IActionResult> InviteUser(InviteUserCommand command)
    {
        await _sender.Send(command);
        return Ok(new { Message = "Davet başarıyla gönderildi." });
    }
}