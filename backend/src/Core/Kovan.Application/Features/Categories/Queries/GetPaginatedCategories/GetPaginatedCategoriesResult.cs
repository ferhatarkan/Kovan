namespace Kovan.Application.Features.Categories.Queries.GetPaginatedCategories;

public class GetPaginatedCategoriesResult
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid? ParentCategoryId { get; set; }
    public string? ParentCategoryName { get; set; }
}