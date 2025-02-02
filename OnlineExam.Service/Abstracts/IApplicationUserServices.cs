using OnlineExam.Domain.Entities.Identity;
using OnlineExam.Service.Enums;


namespace OnlineExam.Service.Abstracts
{
    public interface IApplicationUserServices
    {
        public Task<(ApplicationUser?, ApplicationUserResultEnum?)> Register(ApplicationUser user, string Password);
        public Task<ApplicationUserResultEnum> ConfirmEmail(string? userId, string? code);
        public Task<ApplicationUserResultEnum> DeleteUser(string UserId);
        public Task<ApplicationUserResultEnum> ChangePassword(string UserId, string OldPassword, string NewPassword);
    }
}