using Kovan.Application.Features.PurchaseOrders.Commands.CreatePurchaseOrder;
using Kovan.Application.Features.PurchaseOrders.Commands.DeletePurchaseOrder;
using Kovan.Application.Features.PurchaseOrders.Commands.UpdatePurchaseOrder;
using Kovan.Application.Features.PurchaseOrders.Queries.GetPaginatedPurchaseOrders;
using Kovan.Application.Features.PurchaseOrders.Queries.GetPurchaseOrderById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetPaginatedPurchaseOrdersQuery query)
    {
        var purchaseOrders = await _sender.Send(query);
        return Ok(purchaseOrders);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetPurchaseOrderByIdQuery { Id = id };
        var purchaseOrder = await _sender.Send(query);
        return Ok(purchaseOrder);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreatePurchaseOrderCommand command)
    {
        var purchaseOrderId = await _sender.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = purchaseOrderId }, new { PurchaseOrderId = purchaseOrderId });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdatePurchaseOrderCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("Route ID must match command ID.");
        }
        await _sender.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _sender.Send(new DeletePurchaseOrderCommand { Id = id });
        return NoContent();
    }
}