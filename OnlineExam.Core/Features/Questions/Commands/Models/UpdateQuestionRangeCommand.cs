using MediatR;
using OnlineExam.Core.Bases;
using OnlineExam.Core.Features.Questions.DTOs;
using OnlineExam.Core.Features.Questions.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExam.Core.Features.Questions.Commands.Models
{
    public class UpdateQuestionRangeCommand : IRequest<Response<List<QuestionResultModel>>>
    {
        public string InstructorId { get; set; }
        public int ExamId { get; set; }
        public List<QuestionsUpdateDto> QuestionList { get; set; } = new List<QuestionsUpdateDto>();
    }

    public class QuestionsUpdateDto
    {
        public int QuestionId { get; set; }
        public QuestionDto? Question { get; set; }
    }
}
