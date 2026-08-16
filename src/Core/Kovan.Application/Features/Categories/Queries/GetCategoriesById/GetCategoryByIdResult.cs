namespace Kovan.Application.Features.Categories.Queries.GetCategoryById;

public class GetCategoryByIdResult
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid? ParentCategoryId { get; set; }
    public string? ParentCategoryName { get; set; }
}