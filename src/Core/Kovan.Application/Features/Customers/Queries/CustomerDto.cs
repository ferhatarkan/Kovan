using Kovan.Domain.Enums;

namespace Kovan.Application.Features.Customers.Queries;

public class CustomerDto
{
    public Guid Id { get; set; }
    public CustomerType CustomerType { get; set; }

    // Ortak Alanlar
    public string Name { get; set; } = string.Empty; // Bireysel için Ad+Soyad, Kurumsal için Ünvan olacak
    public string Address { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? Email { get; set; }

    // Tipe Özel Alanlar
    public string? TaxOffice { get; set; }
    public string? TaxNumber { get; set; }
    public string? NationalIdentityNumber { get; set; }
}