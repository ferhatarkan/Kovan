using FluentValidation;
using Kovan.Domain.Enums;

namespace Kovan.Application.Features.Customers.Commands.UpdateCustomer;

public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerCommandValidator()
    {
        RuleFor(v => v.Id).NotEmpty();
        RuleFor(v => v.CustomerType).IsInEnum();

        RuleFor(v => v.FirstName).NotEmpty().When(v => v.CustomerType == CustomerType.Individual);
        RuleFor(v => v.LastName).NotEmpty().When(v => v.CustomerType == CustomerType.Individual);
        RuleFor(v => v.NationalIdentityNumber).NotEmpty().Length(11).When(v => v.CustomerType == CustomerType.Individual);

        RuleFor(v => v.Title).NotEmpty().When(v => v.CustomerType == CustomerType.Corporate);
        RuleFor(v => v.TaxNumber).NotEmpty().When(v => v.CustomerType == CustomerType.Corporate);

        RuleFor(v => v.PhoneNumber).NotEmpty();
        RuleFor(v => v.Email).EmailAddress().When(v => !string.IsNullOrEmpty(v.Email));
    }
}