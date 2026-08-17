using MediatR;
using Kovan.Application.Common.Interfaces;
using System;

namespace Kovan.Application.Features.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommand : IRequest, ITransactionalRequest
{
    public Guid Id { get; set; }
}