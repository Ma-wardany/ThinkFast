using MediatR;
using OnlineExam.Core.Bases;

namespace OnlineExam.Core.Features.Questions.Commands.Models
{
    public class DeleteQuestionsRangeCommand : IRequest<Response<string>>
    {
        public string InstructorId   { get; set; }
        public int ExamId            { get; set; }
        public List<int> QuestionIDs { get; set; }
    }
}
