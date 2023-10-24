using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Xml.Linq;
using TestingPlatform.Domain.Entities;
using TestingPlatform.Infrastructure.Interfaces.Repositories;

namespace TestingPlatform.Infrastructure.Repository
{
    public class UserRepository 
        : IUserRepository
    {
        private readonly DbContext _dbContext;

        public UserRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(User entity)
        {
            await _dbContext.AddAsync(entity);
        }

        public void Delete(User entity)
        {
            _dbContext.Remove(entity);
        }

        public async Task<ICollection<User>> GetAllAsync()
        {
            return await _dbContext.Set<User>().ToListAsync();
        }

        public async Task<User> GetEntityAsync(int id)
        {
            return await _dbContext.Set<User>().
               Where(item => item.Id.Equals(id.ToString()))
               .Include(u => u.UserAnswers)
               .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<User>> TakeAsync(int skipElements, int takeElements, (Expression<Func<User, object>> expression, bool ascending) sortOrder)
        {
            var query = _dbContext.Set<User>()
                .AsNoTracking()
                .Include(u => u.UserAnswers)
                .AsQueryable();

            if (sortOrder.ascending)
            {
                query = query.OrderBy(sortOrder.expression);
            }
            else
            {
                query = query.OrderByDescending(sortOrder.expression);
            }

            return query
                .Skip(skipElements)
                .Take(takeElements)
                .AsEnumerable();
        }

        public void Update(User entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public async Task<User> GetByEmailAsync(string userEmail)
        {
            var user = await _dbContext.Set<User>()
                .Where(u => u.Email == userEmail)
                .FirstOrDefaultAsync();

            return user;
        }
    }
}
