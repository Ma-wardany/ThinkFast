using MediatR;
using OnlineExam.Core.Bases;
using OnlineExam.Core.Features.Students.Results;
using OnlineExam.Core.Wrapper;


namespace OnlineExam.Core.Features.Students.Queries.Models
{
    public class GetStudentsBySchoolYearQuery : IRequest<Response<PaginatedResult<GetStudentResultModel>>>
    {
        public int SchoolYear { get; set; }
        public int PageNumber { get; set; }
    }
}
