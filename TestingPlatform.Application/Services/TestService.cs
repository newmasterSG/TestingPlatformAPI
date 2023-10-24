using AutoMapper;
using System.Linq.Expressions;
using TestingPlatform.Application.DTO;
using TestingPlatform.Application.Interfaces;
using TestingPlatform.Domain.Entities;
using TestingPlatform.Domain.Interfaces;
using TestingPlatform.Infrastructure.Interfaces.Repositories;

namespace TestingPlatform.Application.Services
{
    public class TestService : ITestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TestService(IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> AddAsync(TestDTO dTO)
        {
            if (dTO == null)
            {
                return false;
            }

            Test test = _mapper.Map<Test>(dTO);

            var repository = _unitOfWork.GetRepository<Test>() as ITestRepository;

            await repository.AddAsync(test);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<List<TestDTO>> GetAllAsync(int pageNumber, int pageSize, string attribute = "", string order = "asc")
        {
            int skipElements = (pageNumber - 1) * pageSize;

            Expression<Func<Test, object>> orderByExpression = null;

            switch (attribute.ToLower())
            {
                case "name":
                    orderByExpression = test => test.Name;
                    break;
                default:
                    orderByExpression = test => test.Id;
                    break;
            }

            bool ordering = order == "asc" ? true : false;

            var testDb = await _unitOfWork.GetRepository<Test>()
                .TakeAsync(skipElements, pageSize, (orderByExpression, ordering));

            var testDTOs = _mapper.Map<List<TestDTO>>(testDb);

            return testDTOs;
        }

        public async Task<TestDTO> GetAsync(int id)
        {
            var test = _mapper.Map<TestDTO>(await _unitOfWork.GetRepository<Test>().GetEntityAsync(id));

            if (test is null)
            {
                return default;
            }
            
            return test;
        }

        public async Task UpdateAsync(int id, TestDTO dto)
        {
            var repository = _unitOfWork.GetRepository<Test>();

            var testExist = await repository.GetEntityAsync(id);

            if (testExist == null)
            {
                await Task.CompletedTask;
            }
            
            var testDTO = _mapper.Map<Test>(dto);
            testExist.Name = testDTO.Name;
            testExist.Questions = testDTO.Questions;
            
            repository.Update(testExist);
            
            await _unitOfWork.SaveChangesAsync();
        }
        
        public async Task<bool> DeleteAsync(int id)
        {
            var rep = _unitOfWork.GetRepository<Test>();
            var test = await rep.GetEntityAsync(id);

            if(test == null)
            {
                return false;
            }
            
            rep.Delete(test);

            await _unitOfWork.SaveChangesAsync();

            test = await _unitOfWork.GetRepository<Test>().GetEntityAsync(id);

            if(test == null)
            {
                return true;
            }

            return false;
        }

        public async Task<List<QuestionReturnDTO>> GetQuestionsForTest(int testId)
        {
            var rep = _unitOfWork.GetRepository<Question>() as IQuestionRepository;
            return _mapper.Map<List<QuestionReturnDTO>>(await rep.Where(q => q.TestId == testId));
        }

        public async Task<bool> SubmitAnswers(int testId, string userEmail, List<AnswerSubmitDTO> answersDTO)
        {
            if (answersDTO == null && !answersDTO.Any())
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(userEmail))
            {
                return false;
            }

            var userRepo = _unitOfWork.GetRepository<User>() as IUserRepository;

            var user = await userRepo.GetByEmailAsync(userEmail);
            
            var test = await _unitOfWork.GetRepository<Test>().GetEntityAsync(testId);
            
            var answersDtoDictionary = answersDTO.ToDictionary(dto => dto.Id);
            
            int tempId = 0;
            
            foreach (var answer in test.Questions.SelectMany(q => q.Answers).ToList())
            {
                if (answersDtoDictionary.TryGetValue(answer.Id, out var dto))
                {
                    var question = test.Questions.FirstOrDefault(q =>
                        q.Answers.Any(a => a.Id == dto.Id));

                    tempId = question.Answers.FirstOrDefault(item =>
                        item.Id == dto.Id).Id;

                    var userAnswer = new UserAnswer
                    {
                        TestId = testId,
                        QuestionId = question.Id,
                        UserId = user.Id,
                        AnswerId = tempId
                    };

                    await _unitOfWork.GetRepository<UserAnswer>().AddAsync(userAnswer);
                }
            }

            await _unitOfWork.SaveChangesAsync();
            return true;
        }
        
    }
}
