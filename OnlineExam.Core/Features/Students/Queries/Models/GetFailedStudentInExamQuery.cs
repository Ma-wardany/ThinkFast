using MediatR;
using OnlineExam.Core.Bases;
using OnlineExam.Core.Features.Students.Results;
using OnlineExam.Core.Wrapper;

namespace OnlineExam.Core.Features.Students.Queries.Models
{
    public class GetFailedStudentInExamQuery : IRequest<Response<PaginatedResult<StudentExamsResultModel>>>
    {
        public int ExamId     { get; set; }
        public int PageNumber { get; set; }
    }
}