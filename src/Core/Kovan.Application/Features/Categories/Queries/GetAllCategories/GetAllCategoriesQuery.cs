using MediatR;
using System.Collections.Generic;

namespace Kovan.Application.Features.Categories.Queries.GetAllCategories;

public class GetAllCategoriesQuery : IRequest<List<GetAllCategoriesResult>>
{
}