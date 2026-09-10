using Kovan.Domain.Enums;
using MediatR;
using Kovan.Application.Common.Interfaces;

namespace Kovan.Application.Features.Customers.Commands.UpdateCustomer;

public class UpdateCustomerCommand : IRequest, ITransactionalRequest
{
    public Guid Id { get; set; }
    public CustomerType CustomerType { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? NationalIdentityNumber { get; set; }
    public string? Title { get; set; }
    public string? TaxOffice { get; set; }
    public string? TaxNumber { get; set; }
    public string Address { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? Email { get; set; }
}