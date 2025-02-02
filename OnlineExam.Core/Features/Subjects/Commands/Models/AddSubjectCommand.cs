
using MediatR;
using OnlineExam.Core.Bases;
using OnlineExam.Core.Features.Subjects.Results;

namespace OnlineExam.Core.Features.Subjects.Commands.Models
{
    public class AddSubjectCommand : IRequest<Response<SubjectResultModel>>
    {
        public string Name          { get; set; }
        public string Code          { get; set; }
        public string? InstructorId { get; set; }
        public int SchoolYear       { get; set; }
    }
}
