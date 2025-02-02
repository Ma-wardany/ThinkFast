
using MediatR;
using OnlineExam.Core.Bases;

namespace OnlineExam.Core.Features.Instructors.Commands.Models
{
    public class DeleteInstructorAcountCommand : IRequest<Response<string>>
    {
        public string InstructorId { get; set; }
        public string Password { get; set; }
    }
}
