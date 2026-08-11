namespace Kovan.Application.Features.Users.Queries;

public class GetTwoFactorSetupDto
{
    public string SharedKey { get; set; } = string.Empty;
    public string AuthenticatorUri { get; set; } = string.Empty;
}