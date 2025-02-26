using OnlineExam.Domain.Results;
using OnlineExam.Service.Enums;

namespace OnlineExam.Service.Abstracts
{
    public interface IAuthenticationsServices
    {
        public Task<(JWTAuthResult?, AuthenticationResultEnum?)> Login(string Email, string Password);
        public Task<(JWTAuthResult?, AuthenticationResultEnum?)> RefreshToken(string AccessToken, string RefreshToken);
        public Task<AuthenticationResultEnum> RequestRestPasswordOPT(string Email);
        public Task<(string?, AuthenticationResultEnum)> VerifyOTP(string Email, string Otp);
        public Task<AuthenticationResultEnum> ResetPassword(string Email, string Token, string NewPassword);
        public Task<AuthenticationResultEnum> SignOut(string UserId);
    }
}
