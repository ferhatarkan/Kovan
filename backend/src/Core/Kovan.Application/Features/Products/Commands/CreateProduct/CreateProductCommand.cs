using MediatR;
using Kovan.Application.Common.Interfaces;
using System.Collections.Generic;

namespace Kovan.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommand : IRequest<Guid>, ITransactionalRequest
{
    public string Name { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public Guid CategoryId { get; set; } // Yeni eklenen CategoryId özelliği
    public string Brand { get; set; } = string.Empty; // Yeni eklenen Brand özelliği
    public Dictionary<string, string>? Properties { get; set; }
}