using Kovan.Application.Features.Auth.Commands.Login;
using Kovan.Application.Features.Auth.Commands.RefreshToken;
using Kovan.Application.Features.Auth.Commands.Register;
using Kovan.Application.Features.Auth.Commands.Logout;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kovan.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ISender _sender;

    public AuthController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterCommand command)
    {
        return Ok(await _sender.Send(command));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginCommand command)
    {
        return Ok(await _sender.Send(command));
    }

    [HttpPost("refresh-token")]
    [AllowAnonymous] // Bu endpoint'e süresi dolmuş token ile gelineceği için yetki kontrolü olmamalıdır.
    public async Task<IActionResult> RefreshToken(RefreshTokenCommand command)
    {
        return Ok(await _sender.Send(command));
    }

    [HttpPost("logout")]
    [Authorize] // Sadece kimliği doğrulanmış kullanıcılar çıkış yapabilir.
    public async Task<IActionResult> Logout()
    {
        await _sender.Send(new LogoutCommand());
        return Ok(new { Message = "Başarıyla çıkış yapıldı." });
    }
}