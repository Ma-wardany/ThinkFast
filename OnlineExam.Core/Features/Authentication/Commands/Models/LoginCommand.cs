using MediatR;
using OnlineExam.Core.Bases;
using OnlineExam.Domain.Results;

namespace OnlineExam.Core.Features.Authentication.Commands.Models
{
    public class LoginCommand : IRequest<Response<JWTAuthResult>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
