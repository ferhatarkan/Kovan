using Kovan.Domain.Common;
using Kovan.Domain.Enums;

namespace Kovan.Domain.Entities;

public class Customer : BaseEntity
{
    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }
    public CustomerType CustomerType { get; private set; }

    // Corporate fields
    public string? Title { get; private set; }
    public string TaxOffice { get; private set; } = string.Empty;
    public string TaxNumber { get; private set; } = string.Empty;

    // Individual fields
    public string NationalIdentityNumber { get; private set; } = string.Empty;

    // Common fields
    public string Address { get; private set; } = string.Empty;
    public string PhoneNumber { get; private set; } = string.Empty;
    public string? Email { get; private set; }

    private Customer() { } // EF Core için gerekli

    public static Customer CreateCorporate(string title, string taxNumber, string taxOffice, string address, string phoneNumber, string? email = null)
    {
        ValidateCorporateInfo(title, taxNumber, taxOffice);

        return new Customer
        {
            Id = Guid.NewGuid(),
            CustomerType = CustomerType.Corporate,
            Title = title,
            TaxNumber = taxNumber,
            TaxOffice = taxOffice,
            Address = address,
            PhoneNumber = phoneNumber,
            Email = email
        };
    }

    public static Customer CreateIndividual(string firstName, string lastName, string nationalIdentityNumber, string address, string phoneNumber, string? email = null)
    {
        ValidateIndividualInfo(firstName, lastName, nationalIdentityNumber);
        return new Customer
        {
            Id = Guid.NewGuid(),
            CustomerType = CustomerType.Individual,
            FirstName = firstName,
            LastName = lastName,
            TaxNumber = nationalIdentityNumber, // T.C. Kimlik Numarasını Vergi Numarası olarak da ata
            NationalIdentityNumber = nationalIdentityNumber,
            Address = address,
            PhoneNumber = phoneNumber,
            Email = email
        };
    }

    public void UpdateCorporate(string title, string taxNumber, string taxOffice, string address, string phoneNumber, string? email)
    {
        if (CustomerType != CustomerType.Corporate)
            throw new InvalidOperationException("Şahıs müşterisi kurumsal müşteri olarak güncellenemez.");
        ValidateCorporateInfo(title, taxNumber, taxOffice);

        Title = title;
        TaxNumber = taxNumber;
        TaxOffice = taxOffice;
        Address = address;
        PhoneNumber = phoneNumber;
        Email = email;
    }

    public void UpdateIndividual(string firstName, string lastName, string nationalIdentityNumber, string address, string phoneNumber, string? email)
    {
        if (CustomerType != CustomerType.Individual)
            throw new InvalidOperationException("Kurumsal müşteri şahıs müşterisi olarak güncellenemez.");
        ValidateIndividualInfo(firstName, lastName, nationalIdentityNumber);

        FirstName = firstName;
        LastName = lastName;
        TaxNumber = nationalIdentityNumber; // T.C. Kimlik Numarasını Vergi Numarası olarak da ata
        NationalIdentityNumber = nationalIdentityNumber;
        Address = address;
        PhoneNumber = phoneNumber;
        Email = email;
    }

    private static void ValidateIndividualInfo(string? firstName, string? lastName, string nationalIdentityNumber)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("Müşteri adı boş olamaz.");
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Müşteri soyadı boş olamaz.");
        if (string.IsNullOrWhiteSpace(nationalIdentityNumber))
            throw new ArgumentException("T.C. Kimlik numarası boş olamaz.");
    }

    private static void ValidateCorporateInfo(string? title, string taxNumber, string taxOffice)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Müşteri ünvanı boş olamaz.");
        if (string.IsNullOrWhiteSpace(taxNumber))
            throw new ArgumentException("Vergi numarası boş olamaz.");
        if (string.IsNullOrWhiteSpace(taxOffice))
            throw new ArgumentException("Vergi dairesi boş olamaz.");
    }

    public void Delete()
    {
        IsDeleted = true;
    }
}
