using OnlineExam.Domain.Entities.Identity;
using OnlineExam.Service.Enums;

namespace OnlineExam.Service.Abstracts
{
    public interface IEmailNotificationServices
    {
        public Task<EmailServiceResult> SendEmailConfirmation(ApplicationUser user);
        public Task<EmailServiceResult> SendOTP(string Email, string otp);
    }
}
