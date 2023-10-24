using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingPlatform.Application.DTO
{
    public class QuestionDTO
    {
        public string Text { get; set; }
        public List<AnswerDTO> Answers { get; set; }
    }
}
