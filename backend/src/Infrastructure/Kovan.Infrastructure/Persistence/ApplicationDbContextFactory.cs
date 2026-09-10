using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using Kovan.Application.Common.Interfaces; // ICurrentUserService ve IDateTime için

namespace Kovan.Infrastructure.Persistence;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        // Bu, design-time araçlarının appsettings.json dosyasını bulmasını sağlar.
        // Proje yapınıza göre yolu ayarlayın.
        var basePath = Directory.GetCurrentDirectory();
        var configurationPath = Path.GetFullPath(Path.Combine(basePath, "../../Presentation/Kovan.Api/appsettings.json"));

        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile(configurationPath, optional: false, reloadOnChange: true)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException($"'DefaultConnection' adında bir bağlantı dizesi bulunamadı veya appsettings.json dosyasında boş. Yol: {configurationPath}");
        }

        optionsBuilder.UseNpgsql(connectionString, b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));

        // Design-time araçları için dummy servisler sağlıyoruz.
        // Bu servisler, gerçek bir HTTP isteği bağlamı olmadığında null referans hatasını önler.
        var dummyCurrentUserService = new DesignTimeCurrentUserService();
        var dummyDateTime = new DesignTimeDateTime();

        return new ApplicationDbContext(optionsBuilder.Options, dummyCurrentUserService, dummyDateTime);
    }

    // ICurrentUserService için tasarım zamanı implementasyonu
    private class DesignTimeCurrentUserService : ICurrentUserService
    {
        public string? UserId => null;
        public string? TenantId => null; // Tasarım zamanında TenantId'ye ihtiyaç duyulmaz veya null olabilir.
        public bool IsAuthenticated => false;
        public bool IsInRole(string role) => false;
    }

    // IDateTime için tasarım zamanı implementasyonu
    private class DesignTimeDateTime : IDateTime
    {
        public DateTime Now => DateTime.UtcNow;
    }
}