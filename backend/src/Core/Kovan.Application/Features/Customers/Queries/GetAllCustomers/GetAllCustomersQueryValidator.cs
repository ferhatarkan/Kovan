using FluentValidation;

namespace Kovan.Application.Features.Customers.Queries.GetAllCustomers;

public class GetAllCustomersQueryValidator : AbstractValidator<GetAllCustomersQuery>
{
    public GetAllCustomersQueryValidator()
    {
        // Şu anda GetAllCustomersQuery'de doğrulanacak bir parametre bulunmuyor.
        // Gelecekte sayfalama veya filtreleme gibi parametreler eklendiğinde
        // doğrulama kuralları buraya eklenebilir.
    }
}