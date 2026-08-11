using MediatR;

namespace Kovan.Application.Features.Products.Queries.GetProductLabel;

public class GetProductLabelQuery : IRequest<ProductLabelDto>
{
    public Guid ProductId { get; set; }
}