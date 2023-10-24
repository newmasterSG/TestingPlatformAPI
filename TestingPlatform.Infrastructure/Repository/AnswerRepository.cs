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
    public class AnswerRepository:
        IAnswerRepository
    {
        private readonly DbContext _dbContext;

        public AnswerRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(Answer entity)
        {
            await _dbContext.Set<Answer>().AddAsync(entity);
        }

        public void Delete(Answer entity)
        {
            _dbContext.Set<Answer>().Remove(entity);
        }

        public async Task<Answer> GetEntityAsync(int id)
        {
            return await _dbContext.Set<Answer>().FirstOrDefaultAsync(a => a.Id == id);
        }

        public void Update(Answer entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public async Task<ICollection<Answer>> GetAllAsync()
        {
            return await _dbContext.Set<Answer>().ToListAsync();
        }

        public async Task<IEnumerable<Answer>> TakeAsync(int skipElements, int takeElements, (Expression<Func<Answer, object>> expression, bool ascending) sortOrder)
        {
            var query = _dbContext.Set<Answer>()
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
