using System.Reflection;
using Kovan.Application.Common.Interfaces;
using Kovan.Domain.Common;
using Kovan.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;

namespace Kovan.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTime _dateTime;
    private Guid? CurrentTenantId => Guid.TryParse(_currentUserService.TenantId, out var tenantId) ? tenantId : null;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ICurrentUserService currentUserService, IDateTime dateTime) : base(options)
    {
        _currentUserService = currentUserService;
        _dateTime = dateTime;
    }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<InvoiceLine> InvoiceLines => Set<InvoiceLine>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<InventoryTransaction> InventoryTransactions => Set<InventoryTransaction>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<UserInvitation> UserInvitations => Set<UserInvitation>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();
    public DbSet<PurchaseOrderLine> PurchaseOrderLines => Set<PurchaseOrderLine>();
    public DbSet<Warehouse> Warehouses => Set<Warehouse>(); // Yeni
    public DbSet<ProductWarehouse> ProductWarehouses => Set<ProductWarehouse>(); // Yeni

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // Çoklu kiracı (multi-tenancy) için global query filter'ları ayarla
        // ICurrentUserService'ten TenantId'yi al. Arka plan servisleri gibi
        // bir istek bağlamı olmadığında null olabilir. Bu durumda filtre uygulanmaz.
        builder.Entity<Product>().HasQueryFilter(e => !e.IsDeleted && (CurrentTenantId == null || e.TenantId == CurrentTenantId));
        builder.Entity<Customer>().HasQueryFilter(e => !e.IsDeleted && (CurrentTenantId == null || e.TenantId == CurrentTenantId));
        builder.Entity<Invoice>().HasQueryFilter(e => !e.IsDeleted && (CurrentTenantId == null || e.TenantId == CurrentTenantId));
        builder.Entity<InvoiceLine>().HasQueryFilter(e => !e.IsDeleted && (CurrentTenantId == null || e.TenantId == CurrentTenantId));
        builder.Entity<Payment>().HasQueryFilter(p => !p.IsDeleted && (CurrentTenantId == null || p.TenantId == CurrentTenantId) && (p.Invoice == null || !p.Invoice.IsDeleted));
        builder.Entity<Supplier>().HasQueryFilter(e => !e.IsDeleted && (CurrentTenantId == null || e.TenantId == CurrentTenantId));
        builder.Entity<PurchaseOrder>().HasQueryFilter(e => !e.IsDeleted && (CurrentTenantId == null || e.TenantId == CurrentTenantId));
        builder.Entity<PurchaseOrderLine>().HasQueryFilter(e => !e.IsDeleted && (CurrentTenantId == null || e.TenantId == CurrentTenantId));
        builder.Entity<InventoryTransaction>().HasQueryFilter(e => !e.IsDeleted && (CurrentTenantId == null || e.TenantId == CurrentTenantId));
        builder.Entity<Warehouse>().HasQueryFilter(e => !e.IsDeleted && (CurrentTenantId == null || e.TenantId == CurrentTenantId));
        builder.Entity<ProductWarehouse>().HasQueryFilter(e => !e.IsDeleted && (CurrentTenantId == null || e.TenantId == CurrentTenantId));
        builder.Entity<UserInvitation>().HasQueryFilter(e => !e.IsDeleted && (CurrentTenantId == null || e.TenantId == CurrentTenantId));
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<IAuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = _currentUserService.UserId;
                    entry.Entity.CreatedDate = _dateTime.Now;

                    if (entry.Entity is BaseEntity baseEntity)
                    {
                        // Eğer TenantId zaten manuel olarak atanmamışsa (örn: arka plan servisleri)
                        // ve bir kullanıcı bağlamı varsa, TenantId'yi o anki kullanıcıdan al.
                        if (baseEntity.TenantId == Guid.Empty && Guid.TryParse(_currentUserService.TenantId, out var tenantId))
                        {
                            baseEntity.TenantId = tenantId;
                        }
                    }
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdatedBy = _currentUserService.UserId;
                    entry.Entity.UpdatedDate = _dateTime.Now;

                    // Eğer varlık silinmek üzere işaretlendiyse, silen kişiyi ve tarihi kaydet
                    if (entry.OriginalValues.GetValue<bool>(nameof(BaseEntity.IsDeleted)) == false &&
                        entry.CurrentValues.GetValue<bool>(nameof(BaseEntity.IsDeleted)) == true)
                    {
                        entry.Entity.DeletedBy = _currentUserService.UserId;
                        (entry.Entity as BaseEntity)!.DeletedDate = _dateTime.Now;
                    }
                    break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    // IApplicationDbContext'ten gelen transaction metotları
    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return await Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken = default)
    {
        await transaction.CommitAsync(cancellationToken);
    }

    public async Task RollbackTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken = default)
    {
        await transaction.RollbackAsync(cancellationToken);
    }
}
