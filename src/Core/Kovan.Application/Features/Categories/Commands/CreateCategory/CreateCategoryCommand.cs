using MediatR;
using System;

namespace Kovan.Application.Features.Categories.Commands.CreateCategory;

public class CreateCategoryCommand : IRequest<Guid>
{
    public string Name { get; set; } = string.Empty;
    public Guid? ParentCategoryId { get; set; }
}