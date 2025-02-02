
using MediatR;
using OnlineExam.Core.Bases;
using OnlineExam.Core.Features.Students.Results;

namespace OnlineExam.Core.Features.Students.Queries.Models
{
    public class GetStudentByIdQuery : IRequest<Response<GetStudentResultModel>>
    {
        public string StudentId { get; set; }
    }
}
