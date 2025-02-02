using MediatR;
using OnlineExam.Core.Bases;

namespace OnlineExam.Core.Features.ApplicationUser.Commands.Models
{
    public class DeleteUserCommand : IRequest<Response<string>>
    {
        public string UserId { get; set; }
    }
}
