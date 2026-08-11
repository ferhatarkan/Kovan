using MediatR;

namespace Kovan.Application.Features.Auth.Commands.ForgotPassword;

public class ForgotPasswordCommand : IRequest<string> // Token'ı test için döndürelim
{
    public string Email { get; set; } = string.Empty;
}
