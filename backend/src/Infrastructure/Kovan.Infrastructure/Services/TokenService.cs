using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Kovan.Application.Common.Interfaces;
using Kovan.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Kovan.Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly RoleManager<IdentityRole> _roleManager;

    public TokenService(IConfiguration configuration, RoleManager<IdentityRole> roleManager)
    {
        _configuration = configuration;
        _roleManager = roleManager;
    }

    public async Task<string> GenerateJwtTokenAsync(ApplicationUser user, IEnumerable<string> roles)
    {
        // 1. Claim'leri (Token içinde taşınacak bilgiler) oluşturma
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id), // Kullanıcının benzersiz ID'si
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Token için benzersiz bir kimlik
            new("tenant_id", user.TenantId.ToString()), // Kiracı ID'si (Multi-tenancy için kritik)
            new(ClaimTypes.Name, $"{user.FirstName} {user.LastName}") // Örnek: Kullanıcının tam adını ekleme
        };

        // Kullanıcının rollerini claim'lere ekle
        foreach (var roleName in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, roleName));
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role != null)
            {
                var roleClaims = await _roleManager.GetClaimsAsync(role);
                // Rol'e ait "permission" tipindeki claim'leri token'a ekle
                claims.AddRange(roleClaims.Where(c => c.Type == "permission"));
            }
        }

        // 2. Güvenlik anahtarını (Secret Key) appsettings'den alma
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // 3. Token'ın geçerlilik süresini belirleme
        var expires = DateTime.UtcNow.AddDays(Convert.ToDouble(_configuration["Jwt:ExpireDays"] ?? "7"));

        // 4. Token'ı oluşturma
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        // 5. Token'ı string formatına dönüştürme
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
    {
        if (token == null) return null;

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true, // Token'ın kimin için olduğunu doğrula
            ValidateIssuer = true,   // Token'ı kimin yayınladığını doğrula
            ValidateIssuerSigningKey = true, // İmzalama anahtarını doğrula
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)),
            ValidateLifetime = false // Burası önemli: Süresi dolmuş token'ı doğrulamak için false yapıyoruz.
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            // Token'ı doğrula ve ClaimsPrincipal'ı al
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            // Token'ın algoritmasının HMAC SHA256 olduğundan emin ol
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Geçersiz token algoritması.");
            }

            return principal;
        }
        catch
        {
            return null; // Token geçersizse veya başka bir hata olursa null döndür
        }
    }
}