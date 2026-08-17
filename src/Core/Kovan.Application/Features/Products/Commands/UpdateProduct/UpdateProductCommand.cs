using MediatR;
using Kovan.Application.Common.Interfaces;
using System.Collections.Generic;

namespace Kovan.Application.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommand : IRequest, ITransactionalRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Brand { get; set; } = string.Empty;
    public Guid CategoryId { get; set; }
    public Dictionary<string, string> Properties { get; set; } = new();
}