
using MediatR;
using OnlineExam.Core.Bases;
using OnlineExam.Core.Features.Authorization.Queries.Models;
using OnlineExam.Domain.Entities.Identity;
using OnlineExam.Service.Abstracts;
using OnlineExam.Service.Enums;

namespace OnlineExam.Core.Features.Authorization.Queries.Handler
{
    public class RoleQueriesHandler : ResponseHandler, 
                                      IRequestHandler<GetRoleByIdQuery, Response<Role>>,
                                      IRequestHandler<GetAllRolesQuery, Response<List<Role>>>
    {
        private readonly IAuthorizationServices authorizationServices;

        public RoleQueriesHandler(IAuthorizationServices authorizationServices)
        {
            this.authorizationServices = authorizationServices;
        }


        public async Task<Response<Role>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await authorizationServices.GetRoleById(request.RoleId);
            var status = result.Item2;
            var role   = result.Item1;

            return status switch
            {
                AuthorizationResultEnum.SUCCESS       => Success(role),
                AuthorizationResultEnum.NOTFOUND_ROLE => NotFound<Role>("not found role!"),
                _ or AuthorizationResultEnum.FAILED   => BadRequest<Role>("something went wrong"),
            };
        }


        public async Task<Response<List<Role>>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            var result = await authorizationServices.GetAllRoles();
            var status = result.Item2;
            var roles  = result.Item1;

            return status switch
            {
                AuthorizationResultEnum.SUCCESS     => Success(roles),
                AuthorizationResultEnum.EMPTY       => NotFound<List<Role>>("not found role!"),
                _ or AuthorizationResultEnum.FAILED => BadRequest<List<Role>>("something went wrong"),
            };
        }
    }
}
