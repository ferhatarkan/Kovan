using Kovan.Application.Features.Auth.Commands.Login;
using MediatR;

namespace Kovan.Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenCommand : IRequest<LoginResponseDto>
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
}