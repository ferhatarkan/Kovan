using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Kovan.Application.Common.Interfaces;

namespace Kovan.Application.Common.Behaviours;

public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly Stopwatch _timer;
    private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> _logger;
    private readonly ICurrentUserService _currentUserService;
    private const int _longRunningRequestThresholdMs = 500; // "Uzun süren" istekler için eşik değeri (milisaniye)

    public PerformanceBehavior(ILogger<PerformanceBehavior<TRequest, TResponse>> logger, ICurrentUserService currentUserService)
    {
        _timer = new Stopwatch();
        _logger = logger;
        _currentUserService = currentUserService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _timer.Start();
        var response = await next();
        _timer.Stop();

        var elapsedMilliseconds = _timer.ElapsedMilliseconds;
        if (elapsedMilliseconds > _longRunningRequestThresholdMs)
        {
            var requestName = typeof(TRequest).Name;
            var userId = _currentUserService.UserId ?? "Anonymous";
            var tenantId = _currentUserService.TenantId ?? "N/A";
            _logger.LogWarning("Kovan Long Running Request: {RequestName} ({ElapsedMilliseconds}ms) by User: {UserId}, Tenant: {TenantId}, Request: {@Request}",
                requestName, elapsedMilliseconds, userId, tenantId, request);
        }

        return response;
    }
}