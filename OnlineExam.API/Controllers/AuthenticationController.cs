using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineExam.API.Base;
using OnlineExam.Core.Features.ApplicationUser.Commands.Models;
using OnlineExam.Core.Features.Authentication.Commands.Models;
using System.Security.Claims;

namespace OnlineExam.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : AppController
    {

        [HttpPost("login")]
        public async Task<IActionResult> RegisterStudent([FromBody] LoginCommand command)
        {
            var response = await Mediator.Send(command);
            return FinalResponse(response);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenComamnd command)
        {
            var response = await Mediator.Send(command);
            return FinalResponse(response);
        }

        [HttpPost("request-otp(reset password)")]
        public async Task<IActionResult> RequestResetPasswordOTP([FromQuery] RequestResetPasswordOTPCommand command)
        {
            var response = await Mediator.Send(command);
            return FinalResponse(response);
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOTP([FromQuery] VerifyOTPCommand command)
        {
            var response = await Mediator.Send(command);
            return FinalResponse(response);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> RefreshToken([FromQuery] ResetPasswordCommand command)
        {
            var response = await Mediator.Send(command);
            return FinalResponse(response);
        }


        [HttpPost("sign-out")]
        [Authorize]
        public async Task<IActionResult> RefreshToken([FromQuery] string Id)
        {
            var UserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            var command = new SignoutCommand
            {
                UserId = UserId.Value ?? ""
            }; 

            var response = await Mediator.Send(command);
            return FinalResponse(response);
        }
    }
}
