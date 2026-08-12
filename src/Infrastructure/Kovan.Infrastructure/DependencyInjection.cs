using Kovan.Application.Common.Interfaces;
using Kovan.Application.Common.Models;
using Kovan.Infrastructure.BackgroundServices;
using Kovan.Infrastructure.Persistence;
using Kovan.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kovan.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Ayarları yapılandır
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        services.Configure<PdfSettings>(configuration.GetSection("PdfSettings"));

        // Veritabanı bağlantısı
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        // Servisler
        services.AddTransient<IDateTime, DateTimeService>();
        services.AddTransient<ITokenService, TokenService>();
        services.AddTransient<IEmailService, EmailService>();
        services.AddTransient<IFileStorageService, FileStorageService>();
        services.AddScoped<IPdfGenerator, PdfGenerator>();

        // Arka plan servisleri
        services.AddHostedService<UpdateOverdueInvoicesService>();

        return services;
    }
}