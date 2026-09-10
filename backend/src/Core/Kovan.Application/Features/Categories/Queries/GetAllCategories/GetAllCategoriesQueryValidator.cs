using FluentValidation;

namespace Kovan.Application.Features.Categories.Queries.GetAllCategories;

public class GetAllCategoriesQueryValidator : AbstractValidator<GetAllCategoriesQuery>
{
    public GetAllCategoriesQueryValidator()
    {
        // Şu anda doğrulanacak bir parametre bulunmuyor.
        // Gelecekte filtreleme gibi parametreler eklendiğinde kurallar buraya yazılabilir.
    }
}