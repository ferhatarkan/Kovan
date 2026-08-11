using MediatR;
using Kovan.Application.Features.Auth.Commands.Login;

namespace Kovan.Application.Features.Auth.Commands.LoginWith2fa;

public class LoginWith2faCommand : IRequest<LoginResponseDto>
{
    public string Email { get; set; } = string.Empty;
    public string TwoFactorCode { get; set; } = string.Empty;
}