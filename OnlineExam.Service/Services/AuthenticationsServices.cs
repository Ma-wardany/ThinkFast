using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OnlineExam.Domain.Entities.Identity;
using OnlineExam.Domain.Helpers;
using OnlineExam.Domain.Results;
using OnlineExam.Infrastructure.Repository.Abstracts;
using OnlineExam.Service.Abstracts;
using OnlineExam.Service.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace OnlineExam.Service.Services
{
    public class AuthenticationsServices : IAuthenticationsServices
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly JwtSettings jwtSettings;
        private readonly IRefreshTokenRepository refreshTokenRepository;
        private readonly IStudentRepository studentRepository;
        private readonly IInstructorRepository instructorRepository;
        private readonly IEmailNotificationServices emailNotificationServices;
        private readonly IDistributedCache cache;

        public AuthenticationsServices(
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,
            IOptions<JwtSettings> jwtSettings,
            IRefreshTokenRepository refreshTokenRepository,
            IStudentRepository studentRepository,
            IInstructorRepository instructorRepository,
            IEmailNotificationServices emailNotificationServices,
            IDistributedCache cache)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.jwtSettings = jwtSettings.Value;
            this.refreshTokenRepository = refreshTokenRepository;
            this.studentRepository = studentRepository;
            this.instructorRepository = instructorRepository;
            this.emailNotificationServices = emailNotificationServices;
            this.cache = cache;
        }

        public async Task<(JWTAuthResult?, AuthenticationResultEnum?)> Login(string Email, string Password)
        {
            var ExistingUser = await userManager.FindByEmailAsync(Email);
            if(ExistingUser == null)
                return (null, AuthenticationResultEnum.INCORRECT_EMAIL_PASSWORD);


            if (!ExistingUser.EmailConfirmed)
                return (null, AuthenticationResultEnum.NOT_CONFIRMED);


            var result = await signInManager.PasswordSignInAsync(ExistingUser, Password, isPersistent: true, lockoutOnFailure: true);
            if(!result.Succeeded)
            {
                if (result.IsLockedOut)
                    return (null, AuthenticationResultEnum.TRY_LATER);

                return (null, AuthenticationResultEnum.INCORRECT_EMAIL_PASSWORD);
            }


            using var Trans = await refreshTokenRepository.BeginTransactionAsync();
            try
            {
                var AccessToken  = await GetAccessToken(ExistingUser);
                var RefreshToken = GetRefreshToken();

                var RefreshTokenToDB = new RefreshToken
                {
                    UserId             = ExistingUser.Id,
                    AccessToken        = AccessToken,
                    RefreshTokenString = RefreshToken.RefreshTokenString,
                    ExpiresOn          = RefreshToken.ExpiresAt,
                    IsRevoked          = false,
                };

                await refreshTokenRepository.AddAsync(RefreshTokenToDB);
                await Trans.CommitAsync();

                var JwtResult = new JWTAuthResult
                {
                    UserId       = ExistingUser.Id,
                    UserName     = ExistingUser.UserName!,
                    Email        = ExistingUser.Email!,
                    Name         = await GetNameOfUser(ExistingUser),
                    Role         = ExistingUser.RoleName,
                    IsConfirmed  = ExistingUser.EmailConfirmed,
                    AccessToken  = AccessToken,
                    RefreshToken = RefreshToken,
                };

                return (JwtResult, AuthenticationResultEnum.SUCCESS);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errors: {ex.InnerException!.Message}");
                await Trans.RollbackAsync();
                return (null, AuthenticationResultEnum.FAILED);
            }
        }

        public async Task<(JWTAuthResult?, AuthenticationResultEnum?)> RefreshToken(string AccessToken, string RefreshToken)
        {
            try
            {
                var claimPrincipal = ReadAccessToken(AccessToken);

                var Username = claimPrincipal.FindFirst(ClaimTypes.Name)?.Value;

                var user = await userManager.FindByNameAsync(Username!);
                if (user == null)
                    return (null, AuthenticationResultEnum.INVALID_ACCESS_TOKEN);



                var StoredRefreshToken = await refreshTokenRepository.GetTableAsTracking()
                                               .FirstOrDefaultAsync(rt => rt.UserId == user.Id && !rt.IsRevoked);

                if (StoredRefreshToken == null || StoredRefreshToken.RefreshTokenString != RefreshToken)
                    return (null, AuthenticationResultEnum.INVALID_REFRESH_TOKEN);



                if (StoredRefreshToken.ExpiresOn <= DateTime.UtcNow)
                {
                    // Set the token as revoked when expired
                    StoredRefreshToken.IsRevoked = true;
                    await refreshTokenRepository.UpdateAsync(StoredRefreshToken);
                    return (null, AuthenticationResultEnum.INVALID_REFRESH_TOKEN);
                }



                // generate new access token
                var NewAccessToken = await GetAccessToken(user);
                StoredRefreshToken.AccessToken = NewAccessToken;

                if ((StoredRefreshToken.ExpiresOn - DateTime.UtcNow).TotalDays <= 1)
                {
                    StoredRefreshToken.RefreshTokenString = GetRefreshToken().RefreshTokenString;
                    StoredRefreshToken.ExpiresOn          = GetRefreshToken().ExpiresAt;
                    StoredRefreshToken.IsRevoked          = false;
                }


                await refreshTokenRepository.UpdateAsync(StoredRefreshToken);

                // Return the updated JWTAuthResult
                var result = new JWTAuthResult
                {
                    UserId       = user.Id,
                    Name         = await GetNameOfUser(user),
                    UserName     = user.UserName!,
                    Email        = user.Email!,
                    IsConfirmed  = user.EmailConfirmed,
                    AccessToken  = NewAccessToken,
                    Role         = user.RoleName,
                    RefreshToken = new RefreshTokenModel
                    {
                        RefreshTokenString = StoredRefreshToken.RefreshTokenString,
                        ExpiresAt = StoredRefreshToken.ExpiresOn
                    }
                };
                return (result, AuthenticationResultEnum.SUCCESS);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errors: {ex.InnerException!.Message}---------------------");
                return (null, AuthenticationResultEnum.FAILED);
            }
            
        }
        
        public async Task<AuthenticationResultEnum> RequestRestPasswordOPT(string Email)
        {
            var user = await userManager.FindByEmailAsync(Email);
            if (user == null)
                return AuthenticationResultEnum.NOTFOUND_USER;


            try
            {
                var otp = new Random().Next(100000, 999999).ToString();
                // caching OTP
                await cache.SetStringAsync($"{Email.ToLower()}_OTP", otp, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });


                var result = await emailNotificationServices.SendOTP(user.Email!, otp);

                if (result != EmailServiceResult.SUCCESS)
                    return AuthenticationResultEnum.SENDING_ERROR;

                return AuthenticationResultEnum.SUCCESS;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errors: {ex.InnerException!.Message}");
                return AuthenticationResultEnum.FAILED;
            }

        }

        public async Task<(string?, AuthenticationResultEnum)> VerifyOTP(string Email, string Otp)
        {
            try
            {
                var CahedOTP = await cache.GetStringAsync($"{Email.ToLower()}_OTP");

                if(CahedOTP == null || CahedOTP != Otp)
                    return (null, AuthenticationResultEnum.INVALID_OTP);

                // remove cached otp
                await cache.RemoveAsync($"{Email.ToLower()}_OTP");

                var user = await userManager.FindByEmailAsync(Email);
                var ResetToken = await userManager.GeneratePasswordResetTokenAsync(user!);

                await cache.SetStringAsync($"{Email.ToLower()}_Token", ResetToken);

                return (ResetToken, AuthenticationResultEnum.VERIFIED_OTP);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errors: {ex.InnerException!.Message}");
                return (null, AuthenticationResultEnum.FAILED);
            }
        }

        public async Task<AuthenticationResultEnum> ResetPassword(string Email, string Token, string NewPassword)
        {
            var user = await userManager.FindByEmailAsync(Email);
            if (user == null)
                return AuthenticationResultEnum.NOTFOUND_USER;

            try
            {
                var CachedToken = await cache.GetStringAsync($"{user.Email!.ToLower()}_Token");
                if (CachedToken == null || CachedToken != Token)
                    return AuthenticationResultEnum.INVALID_RESET_TOKEN;


                // remove cached token
                await cache.RemoveAsync($"{Email.ToLower()}_Token");


                // reset password
                var result = await userManager.ResetPasswordAsync(user, Token, NewPassword);
                if (!result.Succeeded)
                    return AuthenticationResultEnum.FAILED;


                return AuthenticationResultEnum.PASSWORD_RESET;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errors: {ex.InnerException!.Message}");
                return AuthenticationResultEnum.FAILED;
            }
        }

        public async Task<AuthenticationResultEnum> SignOut(string UserId)
        {
            var UserRefreshTokens = refreshTokenRepository.GetTableAsTracking()
                                         .Where(r => r.UserId == UserId);

            if(UserRefreshTokens.Any())
            {
                await signInManager.SignOutAsync();

                await UserRefreshTokens.ExecuteDeleteAsync();
                return AuthenticationResultEnum.SUCCESS;
            }

            return AuthenticationResultEnum.FAILED;
        }


        private ClaimsPrincipal ReadAccessToken(string AccessToken)
        {
            var tokenValidationParameter = new TokenValidationParameters
            {
                ValidateIssuer           = true,
                ValidIssuer              = jwtSettings.Issuer,
                ValidateAudience         = true,
                ValidAudience            = jwtSettings.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SignInKey)),
                ValidateLifetime         = false,
            };

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                // "handler" will use "token validation parameters" to validate token and extract claims
                var claimPrincipal = tokenHandler.ValidateToken(AccessToken, tokenValidationParameter, out SecurityToken securityToken);
                
                var JwtToken = securityToken as JwtSecurityToken;

                if (JwtToken == null || !JwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256))
                    throw new SecurityTokenException("Invalid token");

                return claimPrincipal;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errors : {ex.Message} -------------------");
                throw new UnauthorizedAccessException("Invalid access token", ex);
            }
        }

        private async Task<string> GetNameOfUser(ApplicationUser user)
        {
            if(user.RoleName.ToLower() == "instructor")
            {
                var instructor = await instructorRepository
                                       .GetTableNoTracking()
                                       .SingleOrDefaultAsync(i => i.Id == user.Id);

                return string.Concat(instructor!.FirstName, " ", instructor.LastName);
            }
            else if (user.RoleName.ToLower() == "student")
            {
                var student = await studentRepository
                                       .GetTableNoTracking()
                                       .SingleOrDefaultAsync(s => s.Id == user.Id);

                return string.Concat(student!.FullName);
            }

            return "";
        }

        private RefreshTokenModel GetRefreshToken()
        {
            return new RefreshTokenModel
            {
                RefreshTokenString = GenerateRefreshTokenString(),
                ExpiresAt          = DateTime.UtcNow.AddDays(jwtSettings.RefreshTokenExpireDate),
            };
        }

        private string GenerateRefreshTokenString()
        {
            var refreshToken = new byte[32];
            var generator = RandomNumberGenerator.Create();

            generator.GetBytes(refreshToken);
            return Convert.ToBase64String(refreshToken);
        }

        private async Task<string> GetAccessToken(ApplicationUser existingUser)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, existingUser.Id),
                new Claim(ClaimTypes.Name, existingUser.UserName!),
                new Claim(ClaimTypes.Email, existingUser.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var roles = await userManager.GetRolesAsync(existingUser);
            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var JwtToken = new JwtSecurityToken(
                issuer            : jwtSettings.Issuer,
                audience          : jwtSettings.Audience,
                claims            : claims,
                expires           : DateTime.UtcNow.AddDays(jwtSettings.TokenExpireDate),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SignInKey)),
                    SecurityAlgorithms.HmacSha256)
                );

            var Token = new JwtSecurityTokenHandler().WriteToken(JwtToken);

            return Token;
        }
    }
}
