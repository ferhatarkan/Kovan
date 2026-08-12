using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Kovan.Application.Common.Interfaces;

namespace Kovan.Application.Common.Behaviours;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
    private readonly ICurrentUserService _currentUserService;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger, ICurrentUserService currentUserService)
    {
        _logger = logger;
        _currentUserService = currentUserService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var userId = _currentUserService.UserId ?? "Anonymous";
        var tenantId = _currentUserService.TenantId ?? "N/A";

        _logger.LogInformation("Kovan Request: {RequestName} started by User: {UserId}, Tenant: {TenantId}, Request: {@Request}",
            requestName, userId, tenantId, request);

        var timer = Stopwatch.StartNew();
        try
        {
            var response = await next();
            timer.Stop();
            _logger.LogInformation("Kovan Request: {RequestName} completed in {ElapsedMilliseconds}ms by User: {UserId}, Tenant: {TenantId}, Response: {@Response}",
                requestName, timer.ElapsedMilliseconds, userId, tenantId, response);
            return response;
        }
        catch (Exception ex)
        {
            timer.Stop();
            _logger.LogError(ex, "Kovan Request: {RequestName} failed in {ElapsedMilliseconds}ms by User: {UserId}, Tenant: {TenantId}, Request: {@Request}",
                requestName, timer.ElapsedMilliseconds, userId, tenantId, request);
            throw;
        }
    }
}