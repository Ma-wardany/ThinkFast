using OnlineExam.Core.Features.Questions.DTOs;

namespace OnlineExam.API.Dtos.QuestionsCQDtos
{
    public class DeleteQuestionsRangeDto
    {
        public int ExamId { get; set; }
        public List<int> QuestionIDs { get; set; }
    }
}
