using FluentValidation;

namespace Kovan.Application.Features.Products.Queries.GetAllProducts;

public class GetAllProductsQueryValidator : AbstractValidator<GetAllProductsQuery>
{
    public GetAllProductsQueryValidator()
    {
        // Şu anda GetAllProductsQuery'de doğrulanacak bir parametre bulunmuyor.
        // Gelecekte filtreleme gibi parametreler eklendiğinde kurallar buraya yazılabilir.
    }
}