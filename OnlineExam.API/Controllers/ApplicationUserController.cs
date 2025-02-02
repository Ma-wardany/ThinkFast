using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineExam.API.Base;
using OnlineExam.API.Dtos.ApplicationUserCommandsDto;
using OnlineExam.Core.Features.Answers.Commands.Models;
using OnlineExam.Core.Features.ApplicationUser.Commands.Models;
using System.Security.Claims;

namespace OnlineExam.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController : AppController
    {

        [HttpPost("register-student")]
        public async Task<IActionResult> RegisterStudent([FromBody] RegisterStudentCommand command)
        {
            var response = await Mediator.Send(command);
            return FinalResponse(response);
        }

        [HttpPost("register-instructor")]
        public async Task<IActionResult> RegisterInstructor([FromBody] RegisterInstructorCommand command)
        {
            var response = await Mediator.Send(command);
            return FinalResponse(response);
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] EmailConfirmationCommand command)
        {
            var response = await Mediator.Send(command);
            return FinalResponse(response);
        }

        [HttpDelete("delete")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteUser([FromQuery] DeleteUserCommand command)
        {
            var response = await Mediator.Send(command);
            return FinalResponse(response);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommandDto commandDto)
        {
            var UserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            var command = new ChangePasswordCommand
            {
                UserId          = UserId != null ? UserId.Value : "",
                CurrentPassword = commandDto.CurrentPassword,
                NewPssword      = commandDto.NewPssword
            };
            var response = await Mediator.Send(command);
            return FinalResponse(response);
        }
    }
}
