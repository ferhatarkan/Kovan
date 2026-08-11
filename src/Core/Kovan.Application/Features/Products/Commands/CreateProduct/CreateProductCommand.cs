using MediatR;

namespace Kovan.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommand : IRequest<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public Dictionary<string, string>? Properties { get; set; }
}