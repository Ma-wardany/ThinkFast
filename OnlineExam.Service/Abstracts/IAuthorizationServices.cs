using Microsoft.AspNetCore.Identity;
using OnlineExam.Domain.Entities.Identity;
using OnlineExam.Service.Enums;


namespace OnlineExam.Service.Abstracts
{
    public interface IAuthorizationServices
    {
        public Task<(Role?, AuthorizationResultEnum?)> AddRole(string RoleName);
        public Task<AuthorizationResultEnum> DeleteRole(string RoleId);
        public Task<(List<Role>?, AuthorizationResultEnum?)> GetAllRoles();
        public Task<(Role?, AuthorizationResultEnum?)> GetRoleById(string RoleId);
        public Task<bool> IsRoleExist(string RoleName);
        public Task<(Role?, AuthorizationResultEnum?)> UpdateRole(string RoleId, string NewRoleName);


    }
}
