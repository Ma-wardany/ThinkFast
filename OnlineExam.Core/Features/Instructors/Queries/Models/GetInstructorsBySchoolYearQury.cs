using MediatR;
using OnlineExam.Core.Bases;
using OnlineExam.Core.Features.Instructors.Results;

namespace OnlineExam.Core.Features.Instructors.Queries.Models
{
    public class GetInstructorsBySchoolYearQury : IRequest<Response<List<GetInstructorResultModel>>>
    {
        public int SchoolYear { get; set; }
    }
}
