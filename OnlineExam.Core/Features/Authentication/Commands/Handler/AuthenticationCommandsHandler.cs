using MediatR;
using OnlineExam.Core.Bases;
using OnlineExam.Core.Features.Authentication.Commands.Models;
using OnlineExam.Domain.Results;
using OnlineExam.Service.Abstracts;
using OnlineExam.Service.Enums;

namespace OnlineExam.Core.Features.Authentication.Commands.Handler
{
    public class AuthenticationCommandsHandler : ResponseHandler,
                                                 IRequestHandler<LoginCommand, Response<JWTAuthResult>>,
                                                 IRequestHandler<RefreshTokenComamnd, Response<JWTAuthResult>>,
                                                 IRequestHandler<RequestResetPasswordOTPCommand, Response<string>>,
                                                 IRequestHandler<VerifyOTPCommand, Response<string>>,
                                                 IRequestHandler<ResetPasswordCommand, Response<string>>
    {
        private readonly IAuthenticationsServices authenticationService;

        public AuthenticationCommandsHandler(IAuthenticationsServices authenticationService)
        {
            this.authenticationService = authenticationService;
        }


        public async Task<Response<JWTAuthResult>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var result = await authenticationService.Login(request.Email, request.Password);
            var status      = result.Item2;
            var loginResult = result.Item1;

            if(status == AuthenticationResultEnum.SUCCESS && loginResult != null)
                return Success(loginResult);

            return status switch
            {
                AuthenticationResultEnum.INCORRECT_EMAIL_PASSWORD => BadRequest<JWTAuthResult>("incorrect email or password!"),
                AuthenticationResultEnum.NOT_CONFIRMED            => BadRequest<JWTAuthResult>("this email is not confirmed!"),
                AuthenticationResultEnum.TRY_LATER                => BadRequest<JWTAuthResult>("Too many failed login attempts. Please try again in 5 minutes."),
                _ or AuthenticationResultEnum.FAILED              => BadRequest<JWTAuthResult>("something went wrong")
            };
        }

        public async Task<Response<JWTAuthResult>> Handle(RefreshTokenComamnd request, CancellationToken cancellationToken)
        {
            var result = await authenticationService.RefreshToken(request.AccessToken, request.RefreshToken);
            var status      = result.Item2;
            var TokenResult = result.Item1;

            if (status == AuthenticationResultEnum.SUCCESS && TokenResult != null)
                return Success(TokenResult);

            return status switch
            {
                AuthenticationResultEnum.INVALID_ACCESS_TOKEN => BadRequest<JWTAuthResult>("invalid access or refresh token"),
                AuthenticationResultEnum.INVALID_REFRESH_TOKEN => BadRequest<JWTAuthResult>("this email is not confirmed!"),
                _ or AuthenticationResultEnum.FAILED => BadRequest<JWTAuthResult>("something went wrong")
            };
        }

        public async Task<Response<string>> Handle(RequestResetPasswordOTPCommand request, CancellationToken cancellationToken)
        {
            var result = await authenticationService.RequestRestPasswordOPT(request.Email);

            return result switch
            {
                AuthenticationResultEnum.SUCCESS       => Success<string>(null, "otp sent (check your inbox)"),
                AuthenticationResultEnum.NOTFOUND_USER => NotFound<string>("not found user!"),
                AuthenticationResultEnum.SENDING_ERROR => BadRequest<string>("error during send otp"),
                _ or AuthenticationResultEnum.FAILED   => BadRequest<string>("something went wrong!"),
            };
        }

        public async Task<Response<string>> Handle(VerifyOTPCommand request, CancellationToken cancellationToken)
        {
            var result = await authenticationService.VerifyOTP(request.Email, request.Otp);
            var status      = result.Item2;
            var token  = result.Item1;

            if(status == AuthenticationResultEnum.VERIFIED_OTP && token != null)
            {
                var response = $"Reset Token => {{{token}}}";

                return Success<string>(null, response);
            }

            return status switch
            {
                AuthenticationResultEnum.INVALID_OTP => BadRequest<string>("invaild otp!"),
                _ or AuthenticationResultEnum.FAILED => BadRequest<string>("something went wrong!"),
            };
        }

        public async Task<Response<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var result = await authenticationService.ResetPassword(request.Email, request.ResetToken, request.NewPassword);

            return result switch
            {
                AuthenticationResultEnum.PASSWORD_RESET      => Success<string>("password has been reset"),
                AuthenticationResultEnum.NOTFOUND_USER       => NotFound<string>("something went wrong!"),
                AuthenticationResultEnum.INVALID_RESET_TOKEN => BadRequest<string>("invalid reset token"),
                _ or AuthenticationResultEnum.FAILED         => BadRequest<string>("not found user!"),
            };
        }
    }
}
