
using MediatR;
using OnlineExam.Core.Bases;
using OnlineExam.Core.Features.Instructors.Results;

namespace OnlineExam.Core.Features.Instructors.Commands.Models
{
    public class UpdateInstructorProfileCommand : IRequest<Response<UpdateInstructorProfileResultModel>>
    {
        public string InstructorId { get; set; }
        public string FirstName    { get; set; }
        public string LastName     { get; set; }
        public string Email        { get; set; }
        public string Password     { get; set; }

    }
}
