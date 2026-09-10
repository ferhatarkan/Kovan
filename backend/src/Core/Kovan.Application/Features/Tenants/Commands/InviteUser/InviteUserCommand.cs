using MediatR;

namespace Kovan.Application.Features.Tenants.Commands.InviteUser;

public class InviteUserCommand : IRequest
{
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}