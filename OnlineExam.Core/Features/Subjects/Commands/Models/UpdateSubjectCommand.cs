
using MediatR;
using OnlineExam.Core.Bases;
using OnlineExam.Core.Features.Subjects.Results;

namespace OnlineExam.Core.Features.Subjects.Commands.Models
{
    public class UpdateSubjectCommand : IRequest<Response<SubjectResultModel>>
    {
        public int SubjectId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string? InstructorId { get; set; }
        public int SchoolYear { get; set; }
    }
}
