using Microsoft.AspNetCore.Identity;
using OnlineExam.Domain.Entities.Identity;
using OnlineExam.Infrastructure.Data;
using OnlineExam.Service.Abstracts;
using OnlineExam.Service.Enums;

namespace OnlineExam.Service.Services
{
    public class ApplicationUserServices : IApplicationUserServices
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<Role> roleManager;
        private readonly IEmailNotificationServices emailNotificationService;
        private readonly AppDbContext context;

        public ApplicationUserServices(UserManager<ApplicationUser> userManager,
                                       RoleManager<Role> roleManager,
                                       IEmailNotificationServices emailNotificationService,
                                       AppDbContext context)
        {
            this.userManager              = userManager;
            this.roleManager              = roleManager;
            this.emailNotificationService = emailNotificationService;
            this.context                  = context;
        }

        public async Task<(ApplicationUser?, ApplicationUserResultEnum?)> Register(ApplicationUser user, string Password)
        {
            try
            {
                var ExistUser = await userManager.FindByEmailAsync(user.Email!);
                if (ExistUser != null)
                    return (null, ApplicationUserResultEnum.EXIST_USER);

                using var Trans = await context.Database.BeginTransactionAsync();
                try
                {
                    var RegisterResult = await userManager.CreateAsync(user, Password);
                    if (!RegisterResult.Succeeded)
                    {
                        LogErrors(RegisterResult.Errors); // Log errors here
                        await Trans.RollbackAsync();
                        return (null, ApplicationUserResultEnum.FAILED);
                    }

                    var isRoleExist = await roleManager.RoleExistsAsync(user.RoleName);
                    if (!isRoleExist)
                    {
                        await Trans.RollbackAsync();
                        return (null, ApplicationUserResultEnum.INVALID_ROLE);
                    }

                    var RoleResult = await userManager.AddToRoleAsync(user, user.RoleName);
                    if (!RoleResult.Succeeded)
                    {
                        LogErrors(RoleResult.Errors); // Log errors here
                        await Trans.RollbackAsync();
                        return (null, ApplicationUserResultEnum.FAILED);
                    }


                    var EmailResult = await emailNotificationService.SendEmailConfirmation(user);
                    if (EmailResult != EmailServiceResult.SUCCESS)
                    {
                        await Trans.RollbackAsync();
                        return (null, ApplicationUserResultEnum.FAILED);
                    }

                    await Trans.CommitAsync();
                    return (user, ApplicationUserResultEnum.CREATED);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.InnerException!.Message.ToString());
                    await Trans.RollbackAsync();
                    return (null, ApplicationUserResultEnum.FAILED);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                return (null, ApplicationUserResultEnum.FAILED);
            }
        }

        public async Task<ApplicationUserResultEnum> ConfirmEmail(string? userId, string? code)
        {
            if (userId == null || code == null)
                return ApplicationUserResultEnum.FAILED;

            var user = await userManager.FindByIdAsync(userId);

            var confirm = await userManager.ConfirmEmailAsync(user, code);

            if (!confirm.Succeeded)
            {
                Console.WriteLine(string.Join("; ", confirm.Errors.Select(e => e.Description)));
                return ApplicationUserResultEnum.FAILED;
            }


            return ApplicationUserResultEnum.CONFIRMED_SUCCESS;
        }


        public async Task<ApplicationUserResultEnum> DeleteUser(string UserId)
        {
            var ExistUser = await userManager.FindByIdAsync(UserId);
            if (ExistUser == null)
                return ApplicationUserResultEnum.NOTFOUND_USER;

            using var Trans = await context.Database.BeginTransactionAsync();
            try
            {
                var result = await userManager.DeleteAsync(ExistUser);
                if (!result.Succeeded)
                {
                    await Trans.RollbackAsync();
                    return ApplicationUserResultEnum.FAILED;
                }

                await Trans.CommitAsync();
                return ApplicationUserResultEnum.DELETED;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errors: {ex.InnerException!.Message}");
                await Trans.RollbackAsync();
                return ApplicationUserResultEnum.FAILED;
            }
        }

        public async Task<ApplicationUserResultEnum> ChangePassword(string UserId, string OldPassword, string NewPassword)
        {
            var ExistingUser = await userManager.FindByIdAsync(UserId);
            if (ExistingUser == null)
                return ApplicationUserResultEnum.NOTFOUND_USER;


            try
            {
                var changePassword = await userManager.ChangePasswordAsync(ExistingUser, OldPassword, NewPassword);
                if (!changePassword.Succeeded)
                {
                    return ApplicationUserResultEnum.WRONG_PASSWORD;
                }

                return ApplicationUserResultEnum.UPDATED_PASSWORD;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errors: {ex.InnerException!.Message}");
                return ApplicationUserResultEnum.FAILED;
            }
        }

        private void LogErrors(IEnumerable<IdentityError> errors)
        {
            foreach (var error in errors)
            {
                Console.WriteLine($"Code: {error.Code}, Description: {error.Description}");
            }
        }

    }
}
