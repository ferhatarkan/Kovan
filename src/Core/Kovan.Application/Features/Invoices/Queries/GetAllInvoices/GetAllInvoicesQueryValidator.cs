using FluentValidation;

namespace Kovan.Application.Features.Invoices.Queries.GetAllInvoices;

public class GetAllInvoicesQueryValidator : AbstractValidator<GetAllInvoicesQuery>
{
    public GetAllInvoicesQueryValidator()
    {
        // Şu anda GetAllInvoicesQuery'de doğrulanacak bir parametre bulunmuyor.
        // Gelecekte filtreleme gibi parametreler eklendiğinde kurallar buraya yazılabilir.
    }
}