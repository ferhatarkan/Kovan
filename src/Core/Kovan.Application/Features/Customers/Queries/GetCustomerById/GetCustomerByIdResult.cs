using Kovan.Domain.Enums;

namespace Kovan.Application.Features.Customers.Queries.GetCustomerById;

public class GetCustomerByIdResult
{
    public Guid Id { get; set; }
    public CustomerType CustomerType { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Title { get; set; }
    public string TaxOffice { get; set; } = string.Empty;
    public string TaxNumber { get; set; } = string.Empty;
    public string NationalIdentityNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? Email { get; set; }
}