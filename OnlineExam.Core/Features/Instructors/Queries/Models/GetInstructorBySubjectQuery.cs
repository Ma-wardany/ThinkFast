using MediatR;
using OnlineExam.Core.Bases;
using OnlineExam.Core.Features.Instructors.Results;

namespace OnlineExam.Core.Features.Instructors.Queries.Models
{
    public class GetInstructorBySubjectQuery : IRequest<Response<GetInstructorBySubjectResultModel>>
    {
        public int SubjectId { get; set; }
    }
}
