using MediatR;

namespace Kovan.Application.Features.Users.Commands.UpdateMyProfile;

public class UpdateMyProfileCommand : IRequest
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}