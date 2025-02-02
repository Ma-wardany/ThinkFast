
using MediatR;
using OnlineExam.Core.Bases;
using OnlineExam.Core.Features.Students.Results;

namespace OnlineExam.Core.Features.Students.Commands.Models
{
    public class UpdateStudentCommand : IRequest<Response<UpdateStudentResultModel>>
    {
        public string? StudentId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public int SchoolYear { get; set; }
        public string? Gender { get; set; }
        public string? Password { get; set; }
    }
}
