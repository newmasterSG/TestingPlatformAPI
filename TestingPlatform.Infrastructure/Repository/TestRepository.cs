using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TestingPlatform.Domain.Entities;
using TestingPlatform.Infrastructure.Interfaces.Repositories;

namespace TestingPlatform.Infrastructure.Repository
{
    internal class TestRepository :
        ITestRepository
    {
        private readonly DbContext _dbContext;

        public TestRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(Test entity)
        {
            await _dbContext.AddAsync(entity);
        }

        public void Delete(Test entity)
        {
            _dbContext.Remove(entity);
        }

        public async Task<ICollection<Test>> GetAllAsync()
        {
            return await _dbContext.Set<Test>()
                .Include(t => t.Questions)
                .ThenInclude(q => q.Answers)
                .ToListAsync();
        }

        public async Task<Test> GetEntityAsync(int id)
        {
            return await _dbContext.Set<Test>().
               Where(item => item.Id.Equals(id))
               .Include(t => t.Questions)
               .ThenInclude(q => q.Answers)
               .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Test>> TakeAsync(int skipElements, int takeElements, (Expression<Func<Test, object>> expression, bool ascending) sortOrder)
        {
            var query = _dbContext.Set<Test>()
                .AsNoTracking()
                .Include(t => t.Questions)
                .ThenInclude(q => q.Answers)
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

        public void Update(Test entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }
        
        public int Count()
        {
            return _dbContext.Set<Test>().Count();
        }
    }
}
