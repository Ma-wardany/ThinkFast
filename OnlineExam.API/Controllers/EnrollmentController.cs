using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineExam.API.Base;
using OnlineExam.API.Dtos.EnrollmentCommandsDtos;
using OnlineExam.Core.Features.Enrollment.Commands.Models;
using System.Security.Claims;

namespace OnlineExam.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController : AppController
    {

        [HttpPost("enroll")]
        [Authorize(Roles = "student")]
        public async Task<IActionResult> AddEnrollment([FromBody] EnrollCommandDto commandDto)
        {
            var UserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            var command = new EnrollmentCommand
            {
                StudentId   = UserId != null ? UserId.Value : "",
                SubjectId   = commandDto.SubjectId,
                SubjectCode = commandDto.SubjectCode,
                SchoolYear  = commandDto.SchoolYear
            };
            var response = await Mediator.Send(command);
            return FinalResponse(response);
        }
    }
}
