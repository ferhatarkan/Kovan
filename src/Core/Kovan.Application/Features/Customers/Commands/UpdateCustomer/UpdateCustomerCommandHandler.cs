using Kovan.Application.Common.Exceptions;
using Kovan.Application.Common.Interfaces;
using Kovan.Domain.Entities;
using Kovan.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kovan.Application.Features.Customers.Commands.UpdateCustomer;

public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateCustomerCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (customer == null)
        {
            throw new NotFoundException(nameof(Customer), request.Id);
        }

        if (request.CustomerType == CustomerType.Individual)
        {
            // Domain entity'sindeki metodu çağırarak güncelleme yapıyoruz.
            customer.UpdateIndividual(request.FirstName!, request.LastName!, request.NationalIdentityNumber!, request.Address, request.PhoneNumber, request.Email);
        }
        else if (request.CustomerType == CustomerType.Corporate)
        {
            customer.UpdateCorporate(request.Title!, request.TaxNumber!, request.TaxOffice!, request.Address, request.PhoneNumber, request.Email);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}