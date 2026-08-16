using Kovan.Application.Features.Products.Commands.CreateProduct;
using Kovan.Application.Features.Products.Queries.GetProductLabel;
using Kovan.Application.Features.Products.Queries.GetAllProducts;
using Kovan.Application.Features.Products.Queries.GetProductById;
using Kovan.Application.Features.Products.Queries.GetPaginatedProducts;
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
        return CreatedAtAction(nameof(GetById), new { id = productId }, new { ProductId = productId }); // GetById'ye yönlendiriyoruz
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetPaginatedProductsQuery query)
    {
        var products = await _sender.Send(query);
        return Ok(products);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetProductByIdQuery { Id = id };
        var product = await _sender.Send(query);
        return Ok(product);
    }

    // TODO: UpdateProductCommand ve DeleteProductCommand için endpoint'ler eklenecek.
    // [HttpPut("{id:guid}")]
    // [HttpDelete("{id:guid}")]



    [HttpGet("{id:guid}/label")]
    public async Task<IActionResult> GetProductLabel(Guid id)
    {
        var query = new GetProductLabelQuery { ProductId = id };
        var result = await _sender.Send(query);
        return File(result.Content, result.ContentType, result.FileName);
    }
}