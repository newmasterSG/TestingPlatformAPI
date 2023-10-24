using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TestingPlatform.Domain.Entities;
using TestingPlatform.Domain.Interfaces;

namespace TestingPlatform.Infrastructure.Interfaces.Repositories
{
    public interface IQuestionRepository
        :IRepository<Question>
    {
        Task<List<Question>> Where(Expression<Func<Question, bool>> expression);

        int Count();
    }
}
