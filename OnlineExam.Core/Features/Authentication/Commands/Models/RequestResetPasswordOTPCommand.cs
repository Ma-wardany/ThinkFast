
using MediatR;
using OnlineExam.Core.Bases;

namespace OnlineExam.Core.Features.Authentication.Commands.Models
{
    public class RequestResetPasswordOTPCommand : IRequest<Response<string>>
    {
        public string Email { get; set; }
    }
}
