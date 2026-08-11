using Kovan.Application.Features.Products.Commands.CreateProduct;
using Kovan.Application.Features.Products.Queries.GetProductLabel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kovan.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly ISender _sender;

    public ProductsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductCommand command)
    {
        var productId = await _sender.Send(command);
        // Oluşturulan kaynağın konumunu ve ID'sini döndürmek iyi bir pratiktir.
        return CreatedAtAction(nameof(Create), new { id = productId }, new { ProductId = productId });
    }

    [HttpGet("{id:guid}/label")]
    public async Task<IActionResult> GetProductLabel(Guid id)
    {
        var query = new GetProductLabelQuery { ProductId = id };
        var result = await _sender.Send(query);
        return File(result.FileContents, result.ContentType, result.FileName);
    }
}