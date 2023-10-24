using TestingPlatform.Domain.Entities;
using TestingPlatform.Domain.Interfaces;

namespace TestingPlatform.Infrastructure.Interfaces.Repositories
{
    public interface IUserRepository
        : IRepository<User>
    {
        Task<User> GetByEmailAsync(string userEmail);
    }
}
