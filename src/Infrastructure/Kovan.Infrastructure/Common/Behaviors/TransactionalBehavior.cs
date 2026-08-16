using Kovan.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Kovan.Infrastructure.Common.Behaviors;

public class TransactionalBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, ITransactionalRequest
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<TransactionalBehavior<TRequest, TResponse>> _logger;

    public TransactionalBehavior(IApplicationDbContext context, ILogger<TransactionalBehavior<TRequest, TResponse>> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // Zaten var olan bir transaction varsa, ona dahil ol.
        if (_context.Database.CurrentTransaction != null)
        {
            _logger.LogDebug("----- Existing transaction found for {RequestName}. Joining it.", typeof(TRequest).Name);
            return await next();
        }

        var strategy = _context.Database.CreateExecutionStrategy();

        return await strategy.ExecuteAsync(next, async (context, state, cancellationToken) =>
        {
            // Use the DbContext from the execution strategy
            await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
            _logger.LogInformation("----- Transaction başladı {TransactionId} for {RequestName} ({@Request})", transaction.TransactionId, typeof(TRequest).Name, request);

            try
            {
                // Execute the next handler in the pipeline
                var response = await state();

                // EF Core's DbContext doesn't have CommitTransactionAsync. Use the transaction object directly.
                await transaction.CommitAsync(cancellationToken);
                _logger.LogInformation("----- Transaction commit edildi {TransactionId} for {RequestName}", transaction.TransactionId, typeof(TRequest).Name);

                return response;
            }
            catch (Exception ex)
            {
                // EF Core's DbContext doesn't have RollbackTransactionAsync. Use the transaction object directly.
                _logger.LogError(ex, "----- Transaction geri alınıyor {TransactionId} for {RequestName}", transaction.TransactionId, typeof(TRequest).Name);
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }, verifySucceeded: null, cancellationToken: cancellationToken);
    }
}