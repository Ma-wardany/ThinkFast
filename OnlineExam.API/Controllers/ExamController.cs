using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineExam.API.Base;
using OnlineExam.Core.Features.Exams.Commands.Models;
using OnlineExam.Core.Features.Exams.Queries.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace OnlineExam.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin,instructor")]
    public class ExamController : AppController
    {

        [HttpPost("add-exam")]
        public async Task<IActionResult> AddExam([FromBody] AddExamCommand command)
        {
            var response = await Mediator.Send(command);
            return FinalResponse(response);
        }

        [HttpPut("update-exam")]
        public async Task<IActionResult> UpdateExam([FromBody] UpdateExamCommand command)
        {
            var response = await Mediator.Send(command);
            return FinalResponse(response);
        }

        [HttpDelete("delete-exam")]
        public async Task<IActionResult> DeleteExam([FromQuery] DeleteExamCommand command)
        {
            var response = await Mediator.Send(command);
            return FinalResponse(response);
        }

        [HttpGet("exams-by-instructor-id")]
        public async Task<IActionResult> GetExamsByInstructorId([FromQuery] GetExamsByInstructorIdQuery query)
        {
            var response = await Mediator.Send(query);
            return FinalResponse(response);
        }

        [HttpPut("end-exam")]
        public async Task<IActionResult> EndExam([FromQuery] EndExamCommand command)
        {
            var response = await Mediator.Send(command);
            return FinalResponse(response);
        }
    }
}
