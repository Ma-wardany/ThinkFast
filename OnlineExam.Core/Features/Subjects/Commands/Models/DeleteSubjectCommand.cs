
using MediatR;
using OnlineExam.Core.Bases;
using OnlineExam.Service.Enums;

namespace OnlineExam.Core.Features.Subjects.Commands.Models
{
    public class DeleteSubjectCommand : IRequest<Response<string>>
    {
        public int SubjectId { get; set; }
    }
}
