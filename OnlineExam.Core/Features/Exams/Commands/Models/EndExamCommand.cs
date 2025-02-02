using MediatR;
using OnlineExam.Core.Bases;

namespace OnlineExam.Core.Features.Exams.Commands.Models
{
    public class EndExamCommand : IRequest<Response<string>>
    {
        public int ExamId { get; set; }
    }
}
