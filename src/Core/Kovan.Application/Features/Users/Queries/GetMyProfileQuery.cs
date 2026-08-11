using MediatR;

namespace Kovan.Application.Features.Users.Queries;

public class GetMyProfileQuery : IRequest<MyProfileDto>
{
}