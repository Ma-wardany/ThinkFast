using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineExam.API.Base;
using OnlineExam.API.Dtos.QuestionsCQDtos;
using OnlineExam.Core.Features.Questions.Commands.Models;
using OnlineExam.Core.Features.Questions.Queries.Models;
using System.Security.Claims;

namespace OnlineExam.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : AppController
    {

        [HttpPost("add-question")]
        [Authorize(Roles = "instructor")]
        public async Task<IActionResult> AddQuestionToExam([FromBody] AddQuestionCommandDto commandDto)
        {
            var UserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            var command = new AddQuestionCommand
            {
                InstructorId = UserId != null ? UserId.Value : "",
                ExamId  = commandDto.ExamId,
                Content = commandDto.Content,
                OptionA = commandDto.OptionA,
                OptionB = commandDto.OptionB,
                OptionC = commandDto.OptionC,
                OptionD = commandDto.OptionD,
                CorrectAnswer = commandDto.CorrectAnswer
            };

            var response = await Mediator.Send(command);
            return FinalResponse(response);
        }


        [HttpPut("update-question")]
        [Authorize(Roles = "instructor")]
        public async Task<IActionResult> UpdateQuestion([FromBody] UpdateQuestionCommandDto commandDto)
        {
            var UserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            var command = new UpdateQuestionCommand
            {
                InstructorId  = UserId != null ? UserId.Value : "",
                ExamId        = commandDto.ExamId,
                QuestionId    = commandDto.QuestionId,
                Content       = commandDto.Content,
                OptionA       = commandDto.OptionA,
                OptionB       = commandDto.OptionB,
                OptionC       = commandDto.OptionC,
                OptionD       = commandDto.OptionD,
                CorrectAnswer = commandDto.CorrectAnswer
            };
            var response = await Mediator.Send(command);
            return FinalResponse(response);
        }


        [HttpPost("add-questions-range")]
        [Authorize(Roles = "instructor")]
        public async Task<IActionResult> AddQuestionsRangeToExam([FromBody] AddQuestionsRangeCommandDto commandDto)
        {
            var UserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            var command = new AddQuestionsRangeCommand
            {
                InstructorId = UserId != null ? UserId.Value : "",
                ExamId = commandDto.ExamId,
                QuestionList = commandDto.QuestionList
            };

            var response = await Mediator.Send(command);
            return FinalResponse(response);
        }


        [HttpPut("update-questions-range")]
        [Authorize(Roles = "instructor")]
        public async Task<IActionResult> UpdateQuestionsRange([FromBody] UpdateQuestionRangeCommandDto commandDto)
        {
            var UserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            var command = new UpdateQuestionRangeCommand
            {
                InstructorId = UserId != null ? UserId.Value : "",
                ExamId       = commandDto.ExamId,
                QuestionList = commandDto.QuestionList
            };
            var response = await Mediator.Send(command);
            return FinalResponse(response);
        }

        [HttpDelete("delete-question")]
        [Authorize(Roles = "instructor")]
        public async Task<IActionResult> DeleteQuestion([FromQuery] DeleteQuestionCommandDto commandDto)
        {
            var UserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            var command = new DeleteQuestionCommand
            {
                InstructorId = UserId != null ? UserId.Value : "",
                ExamId       = commandDto.ExamId,
                QuestionId  = commandDto.QuestionId
            };

            var response = await Mediator.Send(command);
            return FinalResponse(response);
        }


        [HttpDelete("delete-questions-range")]
        [Authorize(Roles = "instructor")]
        public async Task<IActionResult> DeleteQuestionsRange([FromQuery] DeleteQuestionsRangeDto commandDto)
        {
            var UserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            var command = new DeleteQuestionsRangeCommand
            {
                InstructorId = UserId != null ? UserId.Value : "",
                QuestionIDs  = commandDto.QuestionIDs
            };

            var response = await Mediator.Send(command);
            return FinalResponse(response);
        }


        [HttpGet("questions-by-exam-id")]
        [Authorize(Roles = "instructor")]
        public async Task<IActionResult> GetQuestionsByExamId([FromQuery] int ExamId)
        {
            var UserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            var query = new GetQuestionsByExamIdQuery
            {
                InstructorId = UserId != null ? UserId.Value : "",
                ExamId       = ExamId
            };

            var response = await Mediator.Send(query);
            return FinalResponse(response);
        }


        [HttpGet("take-exam")]
        [Authorize(Roles = "student")]
        public async Task<IActionResult> GetQuestionsForStudents([FromQuery] int ExamId)
        {
            var UserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            var query = new GetQuestionsForStudentsQuery
            {
                StudentId = UserId != null ? UserId.Value : "",
                ExamId    = ExamId
            };

            var response = await Mediator.Send(query);
            return FinalResponse(response);
        }
    }
}
