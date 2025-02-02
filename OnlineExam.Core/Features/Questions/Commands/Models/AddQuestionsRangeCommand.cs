using MediatR;
using OnlineExam.Core.Bases;
using OnlineExam.Core.Features.Questions.DTOs;
using OnlineExam.Core.Features.Questions.Results;

namespace OnlineExam.Core.Features.Questions.Commands.Models
{
    public class AddQuestionsRangeCommand : IRequest<Response<List<QuestionResultModel>>>
    {
        public string InstructorId { get; set; }
        public int ExamId { get; set; }
        public List<QuestionDto> QuestionList { get; set; } = new List<QuestionDto>();
    }
}
