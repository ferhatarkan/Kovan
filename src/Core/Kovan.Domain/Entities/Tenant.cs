using Kovan.Domain.Common;

namespace Kovan.Domain.Entities;

public class Tenant : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string? Subdomain { get; private set; } // Örn: "sirket-a.kovan.com" için "sirket-a"
    public string? LogoPath { get; private set; } // Kiracıya özel logo dosya yolu

    // Kiracıya özel diğer ayarlar buraya eklenebilir.
    // Örneğin, para birimi, dil, özel logo yolu vb.

    private Tenant() { }

    public static Tenant Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Kiracı adı boş olamaz.");
        }
        return new Tenant { Name = name };
    }

    public void SetLogoPath(string? path)
    {
        LogoPath = path;
    }
}