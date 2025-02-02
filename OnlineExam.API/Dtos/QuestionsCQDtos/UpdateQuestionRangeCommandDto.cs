using OnlineExam.Core.Features.Questions.Commands.Models;

namespace OnlineExam.API.Dtos.QuestionsCQDtos
{
    public class UpdateQuestionRangeCommandDto
    {
        public int ExamId { get; set; }
        public List<QuestionsUpdateDto> QuestionList { get; set; } = new List<QuestionsUpdateDto>();

    }
}
