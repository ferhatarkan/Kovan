using MediatR;

namespace Kovan.Application.Features.Dashboard.Queries;

public class GetDashboardStatsQuery : IRequest<DashboardDto>
{
}