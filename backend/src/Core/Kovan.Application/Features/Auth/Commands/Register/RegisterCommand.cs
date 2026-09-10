using Kovan.Application.Features.Auth.Commands.Login;
using MediatR;
using Kovan.Application.Common.Interfaces;

namespace Kovan.Application.Features.Auth.Commands.Register;

public class RegisterCommand : IRequest<LoginResponseDto>, ITransactionalRequest
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
