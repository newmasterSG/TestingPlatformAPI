namespace TestingPlatform.Application.DTO;

public class QuestionReturnDTO
{
    public int Id { get; set; }
    public string Text { get; set; }
    public List<AnswerReturnDTO> Answers { get; set; }
}