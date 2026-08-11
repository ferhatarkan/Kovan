using MediatR;

namespace Kovan.Application.Features.Auth.Commands.ResetPassword;

public class ResetPasswordCommand : IRequest
{
    public string Email { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
}