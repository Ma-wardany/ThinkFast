using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OnlineExam.Core.Bases;
using OnlineExam.Core.Features.Answers.Results;
using System.Text.Json.Serialization;

namespace OnlineExam.Core.Features.Answers.Commands.Models
{
    public class SubmitAnswersCommand : IRequest<Response<AnswersResultModel>>
    {
        public string StudentId { get; set; }
        public int ExamId { get; set; }

        public List<AnswerItem> Answers { get; set; }
    }

    public class AnswerItem
    {
        public int QuestionId { get; set; }
        public char Selected {  get; set; }
    }
}
