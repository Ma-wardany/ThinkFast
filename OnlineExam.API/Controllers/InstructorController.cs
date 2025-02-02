using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineExam.API.Base;
using OnlineExam.API.Dtos.InstructorCommandsDtos;
using OnlineExam.Core.Features.ApplicationUser.Commands.Models;
using OnlineExam.Core.Features.Instructors.Commands.Models;
using OnlineExam.Core.Features.Instructors.Queries.Models;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace OnlineExam.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorController : AppController
    {

        [HttpPut("update-profile")]
        [Authorize(Roles = "instructor")]
        public async Task<IActionResult> UpdateInstructor([FromBody] UpdateInstructorProfileDto commandDto)
        {
            var UserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            var command = new UpdateInstructorProfileCommand
            {
                InstructorId = UserId != null ? UserId.Value : "",
                FirstName    = commandDto.FirstName,
                LastName     = commandDto.LastName,
                Email        = commandDto.Email,
                Password     = commandDto.Password
            };

            var response = await Mediator.Send(command);
            return FinalResponse(response);
        }

        [HttpDelete("delete-account")]
        [Authorize(Roles = "instructor")]
        public async Task<IActionResult> DeleteAccount([FromQuery] string Password)
        {
            var UserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            var command = new DeleteInstructorAcountCommand
            {
                InstructorId = UserId != null ? UserId.Value : "",
                Password     = Password
            };
            var response = await Mediator.Send(command);
            return FinalResponse(response);
        }


        [HttpGet("all-instructors")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllSubjects([FromQuery] GetAllInstructorQuery query)
        {
            var response = await Mediator.Send(query);
            return FinalResponse(response);
        }

        [HttpGet("instructor-by-subject")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetInstructorBySubject([FromQuery] GetInstructorBySubjectQuery query)
        {
            var response = await Mediator.Send(query);
            return FinalResponse(response);
        }

        [HttpGet("instructors-by-school-year")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetInstructorsBySchoolYear([FromQuery] GetInstructorsBySchoolYearQury query)
        {
            var response = await Mediator.Send(query);
            return FinalResponse(response);
        }

    }
}
