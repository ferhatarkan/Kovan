using MediatR;
using Kovan.Application.Common.Interfaces;

namespace Kovan.Application.Features.Products.Commands.DeleteProduct;

public class DeleteProductCommand : IRequest, ITransactionalRequest
{
    public Guid Id { get; set; }
}
