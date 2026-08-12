using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Kovan.Application.Common.Interfaces;

namespace Kovan.Application.Common.Behaviours;

public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ITransactionalRequest // Sadece ITransactionalRequest uygulayan isteklere uygulanır
{
    private readonly IApplicationDbContext _dbContext;
    private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;

    public TransactionBehavior(IApplicationDbContext dbContext, ILogger<TransactionBehavior<TRequest, TResponse>> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        _logger.LogInformation("Kovan TransactionBehavior: Handling transaction for {RequestName}", requestName);

        // Geçici hatalarda yeniden deneme için EF Core'un execution strategy'sini kullanın
        var strategy = _dbContext.Database.CreateExecutionStrategy();

        return await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await _dbContext.BeginTransactionAsync(cancellationToken);
            _logger.LogInformation("Kovan TransactionBehavior: Transaction started for {RequestName}", requestName);

            try
            {
                var response = await next();
                await _dbContext.CommitTransactionAsync(transaction, cancellationToken);
                _logger.LogInformation("Kovan TransactionBehavior: Transaction committed for {RequestName}", requestName);
                return response;
            }
            catch (Exception ex)
            {
                await _dbContext.RollbackTransactionAsync(transaction, cancellationToken);
                _logger.LogError(ex, "Kovan TransactionBehavior: Transaction rolled back for {RequestName} due to error: {ErrorMessage}",
                    requestName, ex.Message);
                throw; // Hatayı yeniden fırlat
            }
        });
    }
}