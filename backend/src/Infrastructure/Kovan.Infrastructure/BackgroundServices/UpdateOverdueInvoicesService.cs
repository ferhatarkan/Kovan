using Kovan.Application.Common.Interfaces;
using Kovan.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Kovan.Infrastructure.BackgroundServices;

public class UpdateOverdueInvoicesService : BackgroundService
{
    private readonly ILogger<UpdateOverdueInvoicesService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public UpdateOverdueInvoicesService(ILogger<UpdateOverdueInvoicesService> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Vadesi Geçmiş Faturaları Güncelleme Servisi Başlatıldı.");

        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
                _logger.LogInformation("Vadesi geçmiş faturalar kontrol ediliyor...");

                var overdueInvoices = await dbContext.Invoices
                    .Where(i => i.Status != InvoiceStatus.Paid && i.DueDate < DateTime.UtcNow)
                    .ToListAsync(stoppingToken);

                foreach (var invoice in overdueInvoices)
                {
                    // Reflection yerine doğrudan domain metodunu çağırıyoruz.
                    invoice.UpdateStatusBasedOnPayment();
                }

                await dbContext.SaveChangesAsync(stoppingToken);
                _logger.LogInformation("{Count} adet fatura 'Vadesi Geçti' olarak güncellendi.", overdueInvoices.Count);
            }

            await Task.Delay(TimeSpan.FromHours(24), stoppingToken); // Servisi günde bir kez çalıştır.
        }
    }
}