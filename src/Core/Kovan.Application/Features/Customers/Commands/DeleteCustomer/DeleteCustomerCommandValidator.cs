using FluentValidation;

namespace Kovan.Application.Features.Customers.Commands.DeleteCustomer;

public class DeleteCustomerCommandValidator : AbstractValidator<DeleteCustomerCommand>
{
    public DeleteCustomerCommandValidator()
    {
        RuleFor(v => v.Id).NotEmpty().WithMessage("Müşteri ID'si boş olamaz.");
    }
}