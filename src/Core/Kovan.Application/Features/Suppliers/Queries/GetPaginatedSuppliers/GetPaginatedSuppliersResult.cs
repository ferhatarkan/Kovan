namespace Kovan.Application.Features.Suppliers.Queries.GetPaginatedSuppliers;

public class GetPaginatedSuppliersResult
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? ContactPerson { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
}