
using MediatR;
using OnlineExam.Core.Bases;
using OnlineExam.Core.Features.Exams.Results;

namespace OnlineExam.Core.Features.Exams.Queries.Models
{
    public class GetExamsByInstructorIdQuery : IRequest<Response<List<ExamResultModel>>>
    {
        public string InstructoraId { get; set; }
    }
}
