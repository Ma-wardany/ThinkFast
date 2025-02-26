using MediatR;
using OnlineExam.Core.Bases;

namespace OnlineExam.Core.Features.Authentication.Commands.Models
{
    public class SignoutCommand : IRequest<Response<string>>
    {
        public string UserId { get; set; }
    }
}
