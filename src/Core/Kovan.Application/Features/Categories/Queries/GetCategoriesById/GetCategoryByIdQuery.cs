using MediatR;

namespace Kovan.Application.Features.Categories.Queries.GetCategoryById;

public class GetCategoryByIdQuery : IRequest<GetCategoryByIdResult>
{
    public Guid Id { get; set; }
}