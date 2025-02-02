
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using OnlineExam.Domain.Results;
using OnlineExam.Service.Enums;
using OnlineExam.Service.Services;

namespace OnlineExam.Service.Abstracts
{
    public interface IAuthenticationsServices
    {
        public Task<(JWTAuthResult?, AuthenticationResultEnum?)> Login(string Email, string Password);
        public Task<(JWTAuthResult?, AuthenticationResultEnum?)> RefreshToken(string AccessToken, string RefreshToken);
        public Task<AuthenticationResultEnum> RequestRestPasswordOPT(string Email);
        public Task<(string?, AuthenticationResultEnum)> VerifyOTP(string Email, string Otp);
        public Task<AuthenticationResultEnum> ResetPassword(string Email, string Token, string NewPassword);
    }
}
