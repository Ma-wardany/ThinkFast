using MediatR;
using OnlineExam.Core.Bases;
using OnlineExam.Core.Features.Students.Results;

namespace OnlineExam.Core.Features.Students.Queries.Models
{
    public class GetStudentTakenExamsQuery : IRequest<Response<List<StudentTakenExamsResultModel>>>
    {
        public string StudentId { get; set; }
    }
}
