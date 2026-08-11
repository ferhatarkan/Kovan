using System.Collections.Generic;
using System.Security.Claims;
using Kovan.Domain.Entities;

namespace Kovan.Application.Common.Interfaces;

public interface ITokenService
{
    Task<string> GenerateJwtTokenAsync(ApplicationUser user, IEnumerable<string> roles);
    string GenerateRefreshToken();
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token);
}