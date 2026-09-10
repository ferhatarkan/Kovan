using Kovan.Application.Common.Exceptions;
using Kovan.Application.Common.Interfaces;
using Kovan.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Web;

namespace Kovan.Application.Features.Auth.Commands.ForgotPassword;

public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, string>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailService _emailService;
    private readonly ILogger<ForgotPasswordCommandHandler> _logger;

    public ForgotPasswordCommandHandler(UserManager<ApplicationUser> userManager, IEmailService emailService, ILogger<ForgotPasswordCommandHandler> logger)
    {
        _userManager = userManager;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<string> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var responseMessage = "Şifre sıfırlama bağlantısı e-posta adresinize gönderildi (eğer kayıtlıysa).";
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            // Güvenlik nedeniyle, kullanıcının var olup olmadığını ifşa etmiyoruz.
            _logger.LogWarning("Şifre sıfırlama isteği, bulunamayan e-posta adresi için alındı: {Email}", request.Email);
            return responseMessage;
        }

        if (user.Email is null)
        {
            _logger.LogError("Kullanıcı ID {UserId} için e-posta adresi bulunamadı.", user.Id);
            return responseMessage;
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var encodedToken = HttpUtility.UrlEncode(token);
        var resetLink = $"https://your-frontend-app.com/reset-password?email={user.Email}&token={encodedToken}"; // Frontend URL'nizi buraya yazın

        await _emailService.SendEmailAsync(user.Email, "Şifre Sıfırlama", $"Şifrenizi sıfırlamak için lütfen şu bağlantıya tıklayın: <a href='{resetLink}'>Sıfırla</a>");

        return responseMessage;
    }
}
