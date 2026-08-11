using MediatR;

namespace Kovan.Application.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommand : IRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public decimal Price { get; set; }
}