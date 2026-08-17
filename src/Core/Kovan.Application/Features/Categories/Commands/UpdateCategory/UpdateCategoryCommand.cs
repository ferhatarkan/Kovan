using MediatR;
using Kovan.Application.Common.Interfaces;
using System;

namespace Kovan.Application.Features.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommand : IRequest, ITransactionalRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid? ParentCategoryId { get; set; }
}