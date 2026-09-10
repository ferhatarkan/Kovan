using Kovan.Application.Common.Exceptions;
using Kovan.Application.Common.Interfaces;
using Kovan.Domain.Entities;
using MediatR;

namespace Kovan.Application.Features.Suppliers.Commands.UpdateSupplier;

public class UpdateSupplierCommandHandler : IRequestHandler<UpdateSupplierCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateSupplierCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
    {
        var supplier = await _context.Suppliers.FindAsync(new object[] { request.Id }, cancellationToken)
                       ?? throw new NotFoundException(nameof(Supplier), request.Id);

        supplier.Update(request.Name, request.ContactPerson, request.Email, request.PhoneNumber, request.Address);

        await _context.SaveChangesAsync(cancellationToken);
    }
}