using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineExam.Domain.Entities.Identity;
using OnlineExam.Service.Abstracts;
using OnlineExam.Service.Enums;

namespace OnlineExam.Service.Services
{
    public class AuthorizationServices : IAuthorizationServices
    {
        private readonly RoleManager<Role> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public AuthorizationServices(RoleManager<Role> roleManager, UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public async Task<(Role?, AuthorizationResultEnum?)> AddRole(string RoleName)
        {
            var IsRoleExist = await roleManager.RoleExistsAsync(RoleName);

            if (IsRoleExist) return (null, AuthorizationResultEnum.EXIST_ROLE);

            try
            {
                var result = await roleManager.CreateAsync(new Role { Name = RoleName });

                if (!result.Succeeded)
                    return (null, AuthorizationResultEnum.FAILED);

                var role = await roleManager.FindByNameAsync(RoleName);

                return (role, AuthorizationResultEnum.CREATED);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message + "+++++++++++++++++++++++++++++++++++++++");
                return (null, AuthorizationResultEnum.FAILED);
            }
        }

        public async Task<AuthorizationResultEnum> DeleteRole(string RoleId)
        {
            var role = await roleManager.FindByIdAsync(RoleId);
            if (role == null)
                return AuthorizationResultEnum.NOTFOUND_ROLE;

            var IsRoleUsed = userManager.Users.Any(u => u.RoleName == role.Name);
            if (IsRoleUsed)
                return AuthorizationResultEnum.USED_ROLE;


            var result = await roleManager.DeleteAsync(role);
            if (!result.Succeeded)
                return AuthorizationResultEnum.FAILED;

            return AuthorizationResultEnum.DELETED;
        }

        public async Task<(List<Role>?, AuthorizationResultEnum?)> GetAllRoles()
        {
            try
            {
                var roles = await roleManager.Roles.ToListAsync();
                if (!roles.Any())
                    return (null, AuthorizationResultEnum.EMPTY);

                return (roles, AuthorizationResultEnum.SUCCESS);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errors: {ex.InnerException!.Message}");
                return (null, AuthorizationResultEnum.FAILED);
            }
        }

        public async Task<(Role?, AuthorizationResultEnum?)> GetRoleById(string RoleId)
        {
            try
            {
                var role = await roleManager.FindByIdAsync(RoleId);

                if (role == null)
                    return (null, AuthorizationResultEnum.NOTFOUND_ROLE);

                return (role, AuthorizationResultEnum.SUCCESS);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errors: {ex.InnerException!.Message}");
                return (null, AuthorizationResultEnum.FAILED);
            }
            
        }

        public async Task<bool> IsRoleExist(string RoleName)
        {
            return await roleManager.RoleExistsAsync(RoleName);
        }

        public async Task<(Role?, AuthorizationResultEnum?)> UpdateRole(string RoleId, string NewRoleName)
        {
            var role = await roleManager.FindByIdAsync(RoleId);
            if (role == null)
                return (null, AuthorizationResultEnum.NOTFOUND_ROLE);

            role.Name = NewRoleName;
            var result = await roleManager.UpdateAsync(role);

            if (!result.Succeeded)
                return (null, AuthorizationResultEnum.FAILED);

            return (role, AuthorizationResultEnum.UPDATED);
        }
    }
}
