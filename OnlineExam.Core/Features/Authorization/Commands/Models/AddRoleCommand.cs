using MediatR;
using OnlineExam.Core.Bases;
using OnlineExam.Domain.Entities.Identity;

namespace OnlineExam.Core.Features.Authorization.Commands.Models
{
    public class AddRoleCommand : IRequest<Response<Role>>
    {
        public string RoleName { get; set; }
    }
}
