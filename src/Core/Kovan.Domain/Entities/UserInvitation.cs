using Kovan.Domain.Common;

namespace Kovan.Domain.Entities;

public class UserInvitation : BaseEntity
{
    public string Email { get; private set; } = string.Empty;
    public string InvitationToken { get; private set; } = string.Empty;
    public DateTime ExpiresAt { get; private set; }
    public bool IsAccepted { get; private set; }
    public string InvitedByUserId { get; private set; } = string.Empty;

    private UserInvitation() { }

    public static UserInvitation Create(Guid tenantId, string email, string token, TimeSpan validity, string invitedByUserId)
    {
        return new UserInvitation
        {
            TenantId = tenantId,
            Email = email,
            InvitationToken = token,
            ExpiresAt = DateTime.UtcNow.Add(validity),
            IsAccepted = false,
            InvitedByUserId = invitedByUserId
        };
    }

    public void Accept()
    {
        if (IsAccepted) throw new InvalidOperationException("Bu davet zaten kabul edilmiş.");
        if (ExpiresAt < DateTime.UtcNow) throw new InvalidOperationException("Bu davetin süresi dolmuş.");
        IsAccepted = true;
    }
}