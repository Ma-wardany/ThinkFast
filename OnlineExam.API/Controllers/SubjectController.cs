using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineExam.API.Base;
using OnlineExam.Core.Features.Subjects.Commands.Models;
using OnlineExam.Core.Features.Subjects.Queries.Models;

namespace OnlineExam.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class SubjectController : AppController
    {

        [HttpPost("add-subject")]
        public async Task<IActionResult> AddSubject([FromBody] AddSubjectCommand command)
        {
            var response = await Mediator.Send(command);
            return FinalResponse(response);
        }


        [HttpPut("update-subject")]
        public async Task<IActionResult> UpdateSubject([FromBody] UpdateSubjectCommand command)
        {
            var response = await Mediator.Send(command);
            return FinalResponse(response);
        }


        [HttpDelete("delete-subject")]
        public async Task<IActionResult> DeleteSubject([FromQuery] DeleteSubjectCommand command)
        {
            var response = await Mediator.Send(command);
            return FinalResponse(response);
        }

        [HttpGet("all-subjects")]
        public async Task<IActionResult> GetAllSubjects()
        {
            var response = await Mediator.Send(new GetAllSubjectsQuery());
            return FinalResponse(response);
        }

        [HttpGet("subjects-without-instructor")]
        public async Task<IActionResult> GetSubjectsWithotInstructor()
        {
            var response = await Mediator.Send(new GetSubjectsWithoutInstructorsQuery());
            return FinalResponse(response);
        }
    }
}
