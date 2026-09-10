namespace Kovan.Application.Features.Auth.Commands.Login;

public class LoginResponseDto
{
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
    public bool Is2faRequired { get; set; }
    public string? Message { get; set; }
}