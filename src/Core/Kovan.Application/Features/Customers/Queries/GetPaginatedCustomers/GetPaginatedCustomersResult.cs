using Kovan.Domain.Enums;

namespace Kovan.Application.Features.Customers.Queries.GetPaginatedCustomers;

public class GetPaginatedCustomersResult
{
    public Guid Id { get; set; }
    public string DisplayName { get; set; } = string.Empty; // Title veya Ad Soyad
    public CustomerType CustomerType { get; set; }
    public string? Email { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
}