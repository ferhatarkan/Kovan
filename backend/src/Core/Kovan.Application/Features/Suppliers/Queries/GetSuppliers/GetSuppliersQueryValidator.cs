using FluentValidation;

namespace Kovan.Application.Features.Suppliers.Queries.GetSuppliers;

public class GetSuppliersQueryValidator : AbstractValidator<GetSuppliersQuery>
{
    public GetSuppliersQueryValidator()
    {
        // Şu anda GetSuppliersQuery'de doğrulanacak bir parametre bulunmuyor.
        // Gelecekte filtreleme gibi parametreler eklendiğinde kurallar buraya yazılabilir.
    }
}