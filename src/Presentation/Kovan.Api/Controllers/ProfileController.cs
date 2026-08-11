using Kovan.Application.Features.Users.Commands.UpdateMyProfile;
using Kovan.Application.Features.Users.Commands.ChangeMyPassword;
using Kovan.Application.Features.Users.Queries;
using Kovan.Application.Features.Users.Commands.UpdateProfilePicture;
using Kovan.Application.Features.Users.Commands.EnableTwoFactor;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kovan.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Bu controller'daki tüm endpoint'ler kimlik doğrulaması gerektirir.
public class ProfileController : ControllerBase
{
    private readonly ISender _sender;

    public ProfileController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> GetProfile()
    {
        var profile = await _sender.Send(new GetMyProfileQuery());
        return Ok(profile);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProfile(UpdateMyProfileCommand command)
    {
        await _sender.Send(command);
        return NoContent();
    }

    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword(ChangeMyPasswordCommand command)
    {
        await _sender.Send(command);
        return NoContent();
    }

    [HttpPost("picture")]
    public async Task<IActionResult> UpdateProfilePicture(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("Lütfen bir dosya seçin.");
        }

        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);

        var command = new UpdateProfilePictureCommand { FileContent = memoryStream.ToArray(), FileName = file.FileName };
        var filePath = await _sender.Send(command);

        return Ok(new { FilePath = filePath });
    }

    [HttpGet("2fa-setup")]
    public async Task<IActionResult> GetTwoFactorSetup()
    {
        var setupInfo = await _sender.Send(new GetTwoFactorSetupQuery());
        // Not: Bu endpoint, QR kodunu bir resim olarak da döndürebilir.
        // Şimdilik URI'yi döndürmek, frontend'in QR kodunu oluşturması için yeterlidir.
        return Ok(setupInfo);
    }

    [HttpPost("enable-2fa")]
    public async Task<IActionResult> EnableTwoFactor(EnableTwoFactorCommand command)
    {
        var recoveryCodes = await _sender.Send(command);
        return Ok(new { Message = "İki faktörlü kimlik doğrulama başarıyla etkinleştirildi.", RecoveryCodes = recoveryCodes });
    }
}