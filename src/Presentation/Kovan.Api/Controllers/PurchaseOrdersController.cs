using Kovan.Application.Features.PurchaseOrders.Commands.CreatePurchaseOrder;
using Kovan.Application.Features.PurchaseOrders.Commands.UpdatePurchaseOrder;
using Kovan.Application.Features.PurchaseOrders.Commands.DeletePurchaseOrder;
using Kovan.Application.Features.PurchaseOrders.Queries.GetPurchaseOrderById;
using Kovan.Application.Features.PurchaseOrders.Queries.GetPaginatedPurchaseOrders; // Bu yeni sorgu eklenmeli
using Kovan.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kovan.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PurchaseOrdersController : ControllerBase
{
    private readonly ISender _sender;

    public PurchaseOrdersController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePurchaseOrderCommand command)
    {
        var purchaseOrderId = await _sender.Send(command);
        // RESTful API'ler için en iyi pratik: Oluşturulan kaynağın konumunu ve ID'sini döndür.
        return CreatedAtAction(nameof(GetById), new { id = purchaseOrderId }, new { PurchaseOrderId = purchaseOrderId });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var purchaseOrder = await _sender.Send(new GetPurchaseOrderByIdQuery { Id = id });
        return Ok(purchaseOrder);
    }

    [HttpGet]
    // Sayfalama desteği için [FromQuery] ile query nesnesi al.
    public async Task<IActionResult> GetAll([FromQuery] GetPaginatedPurchaseOrdersQuery query)
    {
        var purchaseOrders = await _sender.Send(query);
        return Ok(purchaseOrders);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePurchaseOrderCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("Route ID must match command ID.");
        }

        await _sender.Send(command);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _sender.Send(new DeletePurchaseOrderCommand { Id = id });

        return NoContent();
    }
}
