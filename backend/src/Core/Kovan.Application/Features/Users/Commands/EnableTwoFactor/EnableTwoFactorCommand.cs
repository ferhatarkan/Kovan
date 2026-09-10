using MediatR;

namespace Kovan.Application.Features.Users.Commands.EnableTwoFactor;

public class EnableTwoFactorCommand : IRequest<IEnumerable<string>> // Kurtarma kodlarını döndürür
{
    public string VerificationCode { get; set; } = string.Empty;
}