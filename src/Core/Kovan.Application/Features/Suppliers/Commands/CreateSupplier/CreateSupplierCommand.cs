using MediatR;
using Kovan.Application.Common.Interfaces;

namespace Kovan.Application.Features.Suppliers.Commands.CreateSupplier;

public class CreateSupplierCommand : IRequest<Guid>, ITransactionalRequest
{
    public string Name { get; set; } = string.Empty;
    public string? ContactPerson { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
}