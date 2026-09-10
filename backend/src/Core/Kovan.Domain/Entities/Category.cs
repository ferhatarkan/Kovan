using Kovan.Domain.Common;

namespace Kovan.Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public Guid? ParentCategoryId { get; private set; } // Hiyerarşik kategoriler için
    public Category? ParentCategory { get; private set; }

    private Category() { }

    public static Category Create(string name, Guid? parentCategoryId = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Kategori adı boş olamaz.");

        return new Category
        {
            Name = name,
            ParentCategoryId = parentCategoryId
        };
    }

    public void UpdateDetails(string name, Guid? parentCategoryId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Kategori adı boş olamaz.");

        Name = name;
        ParentCategoryId = parentCategoryId;
    }

    // Not: Kategori silme işlemi BaseEntity'deki IsDeleted bayrağı ile yönetilir.
}