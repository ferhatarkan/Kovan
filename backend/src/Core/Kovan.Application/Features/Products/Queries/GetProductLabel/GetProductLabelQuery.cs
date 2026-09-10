using MediatR;

namespace Kovan.Application.Features.Products.Queries.GetProductLabel;

public class GetProductLabelQuery : IRequest<GetProductLabelResult>
{
    public Guid ProductId { get; set; }
}