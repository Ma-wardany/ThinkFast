using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineExam.API.Base;
using OnlineExam.API.Dtos.StudentCQDto;
using OnlineExam.Core.Features.Questions.Commands.Models;
using OnlineExam.Core.Features.Students.Commands.Models;
using OnlineExam.Core.Features.Students.Queries.Models;
using OnlineExam.Core.Features.Subjects.Queries.Models;
using System.Security.Claims;

namespace OnlineExam.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : AppController
    {

        [HttpPut("update-profile")]
        [Authorize(Roles = "student")]
        public async Task<IActionResult> UpdateStudent([FromBody] UpdateStudentProfileDto commandDto)
        {
            var UserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            var command = new UpdateStudentCommand
            {
                StudentId  = UserId != null ? UserId.Value : "",
                FullName   = commandDto.FullName,
                Email      = commandDto.Email,
                Gender     = commandDto.Gender,
                SchoolYear = commandDto.SchoolYear,
                Password   = commandDto.Password
            };

            var response = await Mediator.Send(command);
            return FinalResponse(response);
        }


        [HttpDelete("delete-account")]
        [Authorize(Roles = "student")]
        public async Task<IActionResult> DeleteAccount([FromQuery] string Password)
        {
            var UserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            var command = new DeleteStudentAccountCommand
            {
                StudentId = UserId != null ? UserId.Value : "",
                Password = Password
            };

            var response = await Mediator.Send(command);
            return FinalResponse(response);
        }


        [HttpGet("passed-student")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllSubjects([FromQuery] GetPassedStudentInExamsQuery query)
        {
            var response = await Mediator.Send(query);
            return FinalResponse(response);
        }


        [HttpGet("failed-students")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetFailedStudentsInExam([FromQuery] GetFailedStudentInExamQuery query)
        {
            var response = await Mediator.Send(query);
            return FinalResponse(response);
        }


        [HttpGet("student-by-id")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetStudentById([FromQuery] GetStudentByIdQuery query)
        {
            var response = await Mediator.Send(query);
            return FinalResponse(response);
        }


        [HttpGet("students-by-SchoolYear")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetStudentBySchoolYear([FromQuery] GetStudentsBySchoolYearQuery query)
        {
            var response = await Mediator.Send(query);
            return FinalResponse(response);
        }


        [HttpGet("student-taken-exams")]
        [Authorize(Roles = "student")]
        public async Task<IActionResult> GetStudentTakenExams()
        {
            var UserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            var response = await Mediator.Send(new GetStudentTakenExamsQuery { StudentId = UserId.Value});
            return FinalResponse(response);
        }


        [HttpGet("student-pending-exams")]
        [Authorize(Roles = "student")]
        public async Task<IActionResult> GetStudentPendingExams()
        {
            var UserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            var response = await Mediator.Send(new GetStudentPendingExamQuery { StudentId = UserId.Value});
            return FinalResponse(response);
        }


        [HttpGet("student-absent-exams")]
        [Authorize(Roles = "student")]
        public async Task<IActionResult> GetStudentAbsentExams()
        {
            var UserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            var response = await Mediator.Send(new GetStudentAbsentExamsQuery { StudentId = UserId.Value});
            return FinalResponse(response);
        }
    }
}
