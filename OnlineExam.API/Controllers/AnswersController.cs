using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineExam.API.Base;
using OnlineExam.API.Dtos;
using OnlineExam.API.Dtos.AnswersCommandDto;
using OnlineExam.Core.Features.Answers.Commands.Models;
using System.Security.Claims;

namespace OnlineExam.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswersController : AppController
    {

        [HttpPost("submit-answers")]
        [Authorize(Roles ="student")]
        public async Task<IActionResult> AddExam([FromBody] SubmitAnswerDto commandDto)
        {
            var UserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            var command = new SubmitAnswersCommand
            {
                StudentId = UserId != null ? UserId.Value : "",
                Answers   = commandDto.Answers,
                ExamId    = commandDto.ExamId,
            };

            var response = await Mediator.Send(command);
            return FinalResponse(response);
        }
    }
}
