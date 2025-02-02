using MediatR;
using OnlineExam.Core.Bases;
using OnlineExam.Core.Features.ApplicationUser.Commands.Result;

namespace OnlineExam.Core.Features.ApplicationUser.Commands.Models
{
    public class RegisterCommand 
    {
        public string RoleName { get; set; }
        public string Email    { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class RegisterStudentCommand : RegisterCommand, IRequest<Response<StudentRegisterResult>>
    {
        public string FullName { get; set; }
        public string? Gender { get; set; }
        public int SchoolYear { get; set; }
    }

    public class RegisterInstructorCommand : RegisterCommand, IRequest<Response<InstructorRegisterResult>>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
