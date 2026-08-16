using FluentValidation;

namespace Kovan.Application.Features.Suppliers.Queries.GetSupplierById;

public class GetSupplierByIdQueryValidator : AbstractValidator<GetSupplierByIdQuery>
{
    public GetSupplierByIdQueryValidator()
    {
        RuleFor(v => v.Id).NotEmpty().WithMessage("Tedarikçi ID'si boş olamaz.");
    }
}