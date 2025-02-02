
using MediatR;
using OnlineExam.Core.Bases;
using OnlineExam.Core.Features.Questions.Results;

namespace OnlineExam.Core.Features.Questions.Commands.Models
{
    public class UpdateQuestionCommand : IRequest<Response<QuestionResultModel>>
    {
        public string InstructorId { get; set; }
        public int ExamId { get; set; }
        public int QuestionId { get; set; }
        public string Content { get; set; }
        public string OptionA { get; set; }
        public string OptionB { get; set; }
        public string OptionC { get; set; }
        public string OptionD { get; set; }
        public char CorrectAnswer { get; set; }
    }
}
