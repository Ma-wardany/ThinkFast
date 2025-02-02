using OnlineExam.Core.Features.Exams.Commands.Models;
using OnlineExam.Core.Features.Questions.Commands.Models;
using OnlineExam.Core.Features.Questions.Results;
using OnlineExam.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExam.Core.Mappings.QuestionMapping
{
    public partial class QuestionProfile
    {
        public void AddQuestionsRangeCommandProfile()
        {
            CreateMap<AddQuestionsRangeCommand, List<Question>>()
                        .ConvertUsing(command => command.QuestionList.Select(q => new Question
                        {
                            Content = q.Content,
                            OptionA = q.OptionA,
                            OptionB = q.OptionB,
                            OptionC = q.OptionC,
                            OptionD = q.OptionD,
                            CorrectAnswer = q.CorrectAnswer,
                            ExamId = command.ExamId,
                        }).ToList());
        }
    }
}
