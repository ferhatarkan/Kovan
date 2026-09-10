using FluentValidation;

namespace Kovan.Application.Features.Invoices.Queries.GetInvoicePdf;

public class GetInvoicePdfQueryValidator : AbstractValidator<GetInvoicePdfQuery>
{
    public GetInvoicePdfQueryValidator()
    {
        RuleFor(x => x.InvoiceId).NotEmpty().WithMessage("Fatura ID'si boş olamaz.");
    }
}