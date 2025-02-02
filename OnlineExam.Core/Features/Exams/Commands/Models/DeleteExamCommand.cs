using MediatR;
using OnlineExam.Core.Bases;

namespace OnlineExam.Core.Features.Exams.Commands.Models
{
    public class DeleteExamCommand : IRequest<Response<string>>
    {
        public int ExamId          { get; set; }
        public string? InstructorId { get; set; }
    }
}
