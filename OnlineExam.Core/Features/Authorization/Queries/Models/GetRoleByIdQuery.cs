using MediatR;
using OnlineExam.Core.Bases;
using OnlineExam.Domain.Entities.Identity;

namespace OnlineExam.Core.Features.Authorization.Queries.Models
{
    public class GetRoleByIdQuery : IRequest<Response<Role>>
    {
        public string RoleId { get; set; }
    }
}
