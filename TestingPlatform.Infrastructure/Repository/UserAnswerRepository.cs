using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestingPlatform.Domain.Entities;
using TestingPlatform.Infrastructure.Interfaces.Repositories;

namespace TestingPlatform.Infrastructure.Repository
{
    public class UserAnswerRepository :
        IUserAnswerRepository
    {
        private readonly DbContext _dbContext;

        public UserAnswerRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(UserAnswer entity)
        {
            await _dbContext.Set<UserAnswer>().AddAsync(entity);
        }

        public void Delete(UserAnswer entity)
        {
            _dbContext.Set<UserAnswer>().Remove(entity);
        }

        public async Task<UserAnswer> GetEntityAsync(int id)
        {
            return await _dbContext.Set<UserAnswer>().FirstOrDefaultAsync(u => u.Id == id);
        }

        public void Update(UserAnswer entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public async Task<ICollection<UserAnswer>> GetAllAsync()
        {
            return await _dbContext.Set<UserAnswer>().ToListAsync();
        }

        public async Task<IEnumerable<UserAnswer>> TakeAsync(int skipElements, int takeElements, (Expression<Func<UserAnswer, object>> expression, bool ascending) sortOrder)
        {
            var query = _dbContext.Set<UserAnswer>()
                .AsNoTracking()
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
    }
}
