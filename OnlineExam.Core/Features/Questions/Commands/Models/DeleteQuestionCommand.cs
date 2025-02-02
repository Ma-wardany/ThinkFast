using MediatR;
using OnlineExam.Core.Bases;

namespace OnlineExam.Core.Features.Questions.Commands.Models
{
    public class DeleteQuestionCommand : IRequest<Response<string>>
    {
        public string InstructorId { get; set; }
        public int ExamId          { get; set; }
        public int QuestionId      { get; set; }
    }
}
