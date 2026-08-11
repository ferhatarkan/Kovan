using Kovan.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kovan.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Customer> Customers { get; }
    DbSet<Invoice> Invoices { get; }
    DbSet<InvoiceLine> InvoiceLines { get; }
    DbSet<Payment> Payments { get; }
    DbSet<InventoryTransaction> InventoryTransactions { get; }
    DbSet<Product> Products { get; }
    DbSet<Tenant> Tenants { get; }
    DbSet<UserInvitation> UserInvitations { get; }
    DbSet<Supplier> Suppliers { get; }
    DbSet<PurchaseOrder> PurchaseOrders { get; }
    DbSet<PurchaseOrderLine> PurchaseOrderLines { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}