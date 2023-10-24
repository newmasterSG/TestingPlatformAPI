using TestingPlatform.Application.DTO;
using TestingPlatform.Application.Interfaces.MainInterface;
using TestingPlatform.Domain.Entities;

namespace TestingPlatform.Application.Interfaces
{
    public interface ITestService : 
        IService<TestDTO>
    {
        Task<List<QuestionReturnDTO>> GetQuestionsForTest(int testId);

        Task<bool> SubmitAnswers(int testId, string userEmail, List<AnswerSubmitDTO> answersDTO);

    }
}