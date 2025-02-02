
using MediatR;
using OnlineExam.Core.Bases;
using OnlineExam.Core.Features.Enrollment.Results;

namespace OnlineExam.Core.Features.Enrollment.Commands.Models
{
    public class EnrollmentCommand : IRequest<Response<EnrollmentResultModel>>
    {
        public string StudentId { get; set; }
        public int SubjectId { get; set; }
        public string SubjectCode { get; set; }
        public int SchoolYear { get; set; }
    }
}
