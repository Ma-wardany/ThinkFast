
using AutoMapper;
using OnlineExam.Core.Features.Answers.Commands.Models;
using OnlineExam.Domain.Entities;

namespace OnlineExam.Core.Mappings.AnswersMapping
{
    public partial class AnswersProfile
    {
        public void SubmitAnswersCommandProfile()
        {
            CreateMap<SubmitAnswersCommand, List<Answer>>()
                .ConvertUsing((command, answers, context) =>
                {
                    return command.Answers.Select(a => new Answer
                    {
                        ExamId         = command.ExamId,
                        StudentId      = command.StudentId,
                        QuestionId     = a.QuestionId,
                        SelectedAnswer = a.Selected
                    }).ToList();
                });
        }
    }
}
