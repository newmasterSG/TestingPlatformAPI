using System.Security.Claims;
using TestingPlatform.Domain.Entities;

namespace TestingPlatform.Application.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
    ClaimsPrincipal GetPrincipalFromToken(string token);
}