using MediatR;
using OnlineExam.Core.Bases;
using OnlineExam.Core.Features.Subjects.Results;

namespace OnlineExam.Core.Features.Subjects.Queries.Models
{
    public class GetSubjectsWithoutInstructorsQuery : IRequest<Response<List<SubjectResultModel>>>
    {

    }
}
