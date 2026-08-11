using Kovan.Application.Features.Auth.Commands.Login;
using MediatR;

namespace Kovan.Application.Features.Auth.Commands.AcceptInvitation;

public class AcceptInvitationCommand : IRequest<LoginResponseDto>
{
    public string Token { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
}