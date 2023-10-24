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
    public class QuestionRepository : 
        IQuestionRepository
    {
        private readonly DbContext _dbContext;

        public QuestionRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task AddAsync(Question entity)
        {
            await _dbContext.Set<Question>().AddAsync(entity);
        }

        public void Delete(Question entity)
        {
            _dbContext.Set<Question>().Remove(entity);
        }

        public async Task<Question> GetEntityAsync(int id)
        {
            return await _dbContext.Set<Question>()
                .Include(a => a.Answers)
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public void Update(Question entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public async Task<ICollection<Question>> GetAllAsync()
        {
            return await _dbContext.Set<Question>()
                .Include(q => q.Answers)
                .ToListAsync();
        }

        public async Task<IEnumerable<Question>> TakeAsync(int skipElements, int takeElements, (Expression<Func<Question, object>> expression, bool ascending) sortOrder)
        {
            var query = _dbContext.Set<Question>()
                .AsNoTracking()
                .Include(q => q.Answers)
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

        public async Task<List<Question>> Where(Expression<Func<Question, bool>> expression)
        {
            return await _dbContext.Set<Question>()
                .Include(a => a.Answers)
                .Where(expression)
                .ToListAsync();
        }

        public int Count()
        {
            return _dbContext.Set<Question>().Count();
        }
    }
}
