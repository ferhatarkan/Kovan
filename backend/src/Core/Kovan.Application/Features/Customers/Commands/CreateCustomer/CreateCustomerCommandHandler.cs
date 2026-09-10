using Kovan.Application.Common.Interfaces;
using Kovan.Domain.Entities;
using Kovan.Domain.Enums;
using MediatR;

namespace Kovan.Application.Features.Customers.Commands.CreateCustomer;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateCustomerCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        Customer customer;

        if (request.CustomerType == CustomerType.Individual)
        {
            customer = Customer.CreateIndividual(request.FirstName!, request.LastName!, request.NationalIdentityNumber!, request.Address, request.PhoneNumber, request.Email);
        }
        else if (request.CustomerType == CustomerType.Corporate)
        {
            customer = Customer.CreateCorporate(request.Title!, request.TaxNumber!, request.TaxOffice!, request.Address, request.PhoneNumber, request.Email);
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(request.CustomerType), "Geçersiz müşteri tipi belirtildi.");
        }

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync(cancellationToken);

        return customer.Id;
    }
}