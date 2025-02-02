
using MediatR;
using OnlineExam.Core.Bases;
using OnlineExam.Domain.Entities.Identity;

namespace OnlineExam.Core.Features.Authorization.Queries.Models
{
    public class GetAllRolesQuery : IRequest<Response<List<Role>>>
    {
    }
}
