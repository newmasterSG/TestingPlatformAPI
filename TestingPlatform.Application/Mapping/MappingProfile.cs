using AutoMapper;
using TestingPlatform.Application.DTO;
using TestingPlatform.Domain.Entities;

namespace TestingPlatform.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Answer, AnswerDTO>().ReverseMap();
            CreateMap<Question, QuestionDTO>().ReverseMap();
            CreateMap<Question, QuestionReturnDTO>().ReverseMap();
            CreateMap<Answer, AnswerReturnDTO>().ReverseMap();
            CreateMap<Answer, AnswerSubmitDTO>().ReverseMap();
            CreateMap<Test, TestDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
        }
    }
}
