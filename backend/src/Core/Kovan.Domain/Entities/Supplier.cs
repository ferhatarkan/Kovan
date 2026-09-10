using Kovan.Domain.Common;

namespace Kovan.Domain.Entities;

public class Supplier : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string? ContactPerson { get; private set; }
    public string? Email { get; private set; }
    public string? PhoneNumber { get; private set; }
    public string? Address { get; private set; }

    private Supplier() { }

    public static Supplier Create(string name, string? contactPerson, string? email, string? phoneNumber, string? address)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Tedarikçi adı boş olamaz.");

        return new Supplier { Name = name, ContactPerson = contactPerson, Email = email, PhoneNumber = phoneNumber, Address = address };
    }

    public void Update(string name, string? contactPerson, string? email, string? phoneNumber, string? address)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Tedarikçi adı boş olamaz.");

        Name = name;
        ContactPerson = contactPerson;
        Email = email;
        PhoneNumber = phoneNumber;
        Address = address;
    }

    public void Delete() => IsDeleted = true;
}