
using MediatR;
using OnlineExam.Core.Bases;
using OnlineExam.Core.Features.Subjects.Results;

namespace OnlineExam.Core.Features.Subjects.Queries.Models
{
    public class GetAllSubjectsQuery : IRequest<Response<List<SubjectResultModel>>>
    {
    }
}
