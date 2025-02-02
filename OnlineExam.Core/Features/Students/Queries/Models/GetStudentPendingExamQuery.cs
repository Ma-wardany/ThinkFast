using MediatR;
using OnlineExam.Core.Bases;
using OnlineExam.Core.Features.Students.Results;

namespace OnlineExam.Core.Features.Students.Queries.Models
{
    public class GetStudentPendingExamQuery : IRequest<Response<List<PendingAbsentExamResultModel>>>
    {
        public string StudentId { get; set; }
    }
}