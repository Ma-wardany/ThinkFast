using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using OnlineExam.Domain.Entities.Identity;
using OnlineExam.Service.Abstracts;
using OnlineExam.Service.Enums;


namespace OnlineExam.Service.Services
{
    public class EmailNotificationServices : IEmailNotificationServices
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IEmailServices emailService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IUrlHelper urlHelper;

        public EmailNotificationServices(
            UserManager<ApplicationUser> userManager,
            IEmailServices emailService,
            IHttpContextAccessor httpContextAccessor,
            IUrlHelper urlHelper)
        {
            this.userManager         = userManager;
            this.emailService        = emailService;
            this.httpContextAccessor = httpContextAccessor;
            this.urlHelper           = urlHelper;
        }
        public async Task<EmailServiceResult> SendEmailConfirmation(ApplicationUser user)
        {
            var code    = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var request = httpContextAccessor.HttpContext!.Request;
            var url     = request.Scheme + "://" + request.Host + urlHelper.Action("ConfirmEmail", "ApplicationUser", new { userId = user.Id, code = code });
            var message = $"Confirm your email: <a href='{url}'> Confirm Email</a>";

            var result = await emailService.SendEmailAsync(user.Email!, message);

            return result;
        }

        public async Task<EmailServiceResult> SendOTP(string Email, string otp)
        {
            string Message = $"OTP: {otp}";

            var result = await emailService.SendEmailAsync(Email, Message);

            return result;
        }
    }
}