using FluentValidation;

namespace Kovan.Application.Features.Invoices.Queries.GetInvoiceById;

public class GetInvoiceByIdQueryValidator : AbstractValidator<GetInvoiceByIdQuery>
{
    public GetInvoiceByIdQueryValidator()
    {
        RuleFor(v => v.Id).NotEmpty().WithMessage("Fatura ID'si boş olamaz.");
    }
}