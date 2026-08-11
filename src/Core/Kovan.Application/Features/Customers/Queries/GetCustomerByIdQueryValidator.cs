using FluentValidation;

namespace Kovan.Application.Features.Customers.Queries;

public class GetCustomerByIdQueryValidator : AbstractValidator<GetCustomerByIdQuery>
{
    public GetCustomerByIdQueryValidator()
    {
        RuleFor(v => v.Id).NotEmpty().WithMessage("Müşteri ID'si boş olamaz.");
    }
}