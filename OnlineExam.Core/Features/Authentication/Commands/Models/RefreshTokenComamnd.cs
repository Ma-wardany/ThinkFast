using MediatR;
using OnlineExam.Core.Bases;
using OnlineExam.Domain.Results;

namespace OnlineExam.Core.Features.Authentication.Commands.Models
{
    public class RefreshTokenComamnd : IRequest<Response<JWTAuthResult>>
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
