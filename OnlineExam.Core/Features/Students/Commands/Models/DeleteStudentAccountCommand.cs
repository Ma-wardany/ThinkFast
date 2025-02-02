using MediatR;
using OnlineExam.Core.Bases;
using System;

namespace OnlineExam.Core.Features.Students.Commands.Models
{
    public class DeleteStudentAccountCommand : IRequest<Response<string>>
    {
        public string StudentId { get; set; }
        public string Password  { get; set; }
    }
}
