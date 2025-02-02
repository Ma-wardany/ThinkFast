
using MediatR;
using OnlineExam.Core.Bases;
using OnlineExam.Core.Features.Instructors.Results;
using OnlineExam.Core.Wrapper;

namespace OnlineExam.Core.Features.Instructors.Queries.Models
{
    public class GetAllInstructorQuery : IRequest<Response<PaginatedResult<GetInstructorResultModel>>>
    {
        public int PageNumber { get; set; }
    }
}
