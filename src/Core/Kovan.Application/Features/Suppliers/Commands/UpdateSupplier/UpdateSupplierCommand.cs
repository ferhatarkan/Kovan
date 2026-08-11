using MediatR;

namespace Kovan.Application.Features.Suppliers.Commands.UpdateSupplier;

public class UpdateSupplierCommand : IRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? ContactPerson { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
}