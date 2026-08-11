using Kovan.Application.Common.Interfaces;

namespace Kovan.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.UtcNow;
}