using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingPlatform.Domain.Entities;
using TestingPlatform.Domain.Interfaces;

namespace TestingPlatform.Infrastructure.Interfaces.Repositories
{
    public interface IUserAnswerRepository
        :IRepository<UserAnswer>
    {
    }
}
