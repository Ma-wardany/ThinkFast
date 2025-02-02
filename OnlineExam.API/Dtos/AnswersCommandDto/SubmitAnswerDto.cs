using OnlineExam.Core.Features.Answers.Commands.Models;

namespace OnlineExam.API.Dtos.AnswersCommandDto
{
    public class SubmitAnswerDto
    {
        public int ExamId { get; set; }
        public List<AnswerItem> Answers { get; set; }
    }
}
