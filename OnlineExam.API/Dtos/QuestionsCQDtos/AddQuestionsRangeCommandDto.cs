using OnlineExam.Core.Features.Questions.DTOs;

namespace OnlineExam.API.Dtos.QuestionsCQDtos
{
    public class AddQuestionsRangeCommandDto
    {
        public int ExamId { get; set; }
        public List<QuestionDto> QuestionList { get; set; } = new List<QuestionDto>();
    }
}
