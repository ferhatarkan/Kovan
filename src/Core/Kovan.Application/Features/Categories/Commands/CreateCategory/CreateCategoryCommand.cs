using MediatR;
using Kovan.Application.Common.Interfaces;
using System;

namespace Kovan.Application.Features.Categories.Commands.CreateCategory;

public class CreateCategoryCommand : IRequest<Guid>, ITransactionalRequest
{
    public string Name { get; set; } = string.Empty;
    public Guid? ParentCategoryId { get; set; }
}