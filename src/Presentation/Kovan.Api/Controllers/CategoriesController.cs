using Kovan.Application.Features.Categories.Queries.GetAllCategories;
using Kovan.Application.Features.Categories.Queries.GetCategoryById;
using Kovan.Application.Features.Categories.Commands.CreateCategory;
using Kovan.Application.Features.Categories.Queries.GetPaginatedCategories;
using Kovan.Application.Features.Categories.Commands.UpdateCategory;
using Kovan.Application.Features.Categories.Commands.DeleteCategory;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kovan.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CategoriesController : ControllerBase
{
    private readonly ISender _sender;

    public CategoriesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetPaginatedCategoriesQuery query)
    {
        var categories = await _sender.Send(query);
        return Ok(categories);
    }

    // TODO: GetCategoryById, CreateCategory, UpdateCategory, DeleteCategory endpoint'leri eklenecek.

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetCategoryByIdQuery { Id = id };
        var category = await _sender.Send(query);
        return Ok(category);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCategoryCommand command)
    {
        var categoryId = await _sender.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = categoryId }, new { CategoryId = categoryId });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateCategoryCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("Route ID must match command ID.");
        }
        await _sender.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = Kovan.Domain.Constants.Roles.Admin)] // Sadece Admin'ler silebilir
    public async Task<IActionResult> Delete(Guid id)
    {
        await _sender.Send(new DeleteCategoryCommand { Id = id });
        return NoContent();
    }
}