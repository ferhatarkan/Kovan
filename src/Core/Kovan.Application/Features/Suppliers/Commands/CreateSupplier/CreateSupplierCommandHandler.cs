using Kovan.Application.Common.Interfaces;
using Kovan.Domain.Entities;
using MediatR;

namespace Kovan.Application.Features.Suppliers.Commands.CreateSupplier;

public class CreateSupplierCommandHandler : IRequestHandler<CreateSupplierCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateSupplierCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
    {
        var supplier = Supplier.Create(request.Name, request.ContactPerson, request.Email, request.PhoneNumber, request.Address);

        _context.Suppliers.Add(supplier);
        await _context.SaveChangesAsync(cancellationToken);

        return supplier.Id;
    }
}