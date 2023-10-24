using System.Security.Claims;
using TestingPlatform.Application.DTO;
using TestingPlatform.Application.Interfaces.MainInterface;
using TestingPlatform.Domain.Entities;

namespace TestingPlatform.Application.Interfaces
{
    public interface IUserService : 
        IService<UserDTO>
    {
        string GenerateToken(User user);

        ClaimsPrincipal GetClaims(string token);
    }
}