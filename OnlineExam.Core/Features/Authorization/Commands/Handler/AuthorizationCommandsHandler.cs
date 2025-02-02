using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using OnlineExam.Core.Bases;
using OnlineExam.Core.Features.Authorization.Commands.Models;
using OnlineExam.Domain.Entities.Identity;
using OnlineExam.Service.Abstracts;
using OnlineExam.Service.Enums;

namespace OnlineExam.Core.Features.Authorization.Commands.Handler
{
    public class AuthorizationCommandsHandler : ResponseHandler,
                                                IRequestHandler<AddRoleCommand, Response<Role>>,
                                                IRequestHandler<DeleteRoleCommand, Response<string>>,
                                                IRequestHandler<UpdateRoleCommand, Response<Role>>
    {
        private readonly IAuthorizationServices authorizationServices;

        public AuthorizationCommandsHandler(IAuthorizationServices authorizationServices)
        {
            this.authorizationServices = authorizationServices;
        }

        public async Task<Response<Role>> Handle(AddRoleCommand request, CancellationToken cancellationToken)
        {
            var result = await authorizationServices.AddRole(request.RoleName);
            var status = result.Item2;
            var role   = result.Item1;

            return status switch
            {
                AuthorizationResultEnum.CREATED     => Created(role, message: $"{request.RoleName} role has been created"),
                AuthorizationResultEnum.EXIST_ROLE  => BadRequest<Role>($"{ request.RoleName } is already exist!"),
                _ or AuthorizationResultEnum.FAILED => BadRequest<Role>("something went wrong"),
            };
        }

        public async Task<Response<string>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            var result = await authorizationServices.DeleteRole(request.RoleId);

            return result switch
            {
                AuthorizationResultEnum.DELETED       => Success<string>(null, message: $"{request.RoleId} has been deleted"),
                AuthorizationResultEnum.NOTFOUND_ROLE => NotFound<string>("not found role"),
                AuthorizationResultEnum.USED_ROLE     => NotFound<string>("role is used, you can not delete!"),
                _ or AuthorizationResultEnum.FAILED   => BadRequest<string>("something went wrong"),
            };
        }

        public async Task<Response<Role>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            var result = await authorizationServices.UpdateRole(request.RoleId, request.RoleName);
            var status = result.Item2;
            var role   = result.Item1;

            return status switch
            {
                AuthorizationResultEnum.UPDATED       => Success(role, message: "role has been updated"),
                AuthorizationResultEnum.NOTFOUND_ROLE => BadRequest<Role>($"{request.RoleName} role is not found!"),
                _ or AuthorizationResultEnum.FAILED   => BadRequest<Role>("something went wrong"),
            };
        }
    }
}
