using FluentValidation;
using OnlineExam.Core.Features.Questions.Commands.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExam.Core.Features.Questions.Commands.Validators
{
    public class UpdateQuestionsRangeCommandValidator : AbstractValidator<UpdateQuestionRangeCommand>
    {
        public UpdateQuestionsRangeCommandValidator()
        {
            RuleFor(x => x.InstructorId)
                .NotEmpty().WithMessage("instructor id must not be empty!")
                .NotNull().WithMessage("instructor id is required!");


            RuleFor(x => x.ExamId)
                .GreaterThan(0).WithMessage("invalid exam id (non zero)");


            RuleFor(x => x.QuestionList)
                .NotEmpty().WithMessage("no questions (empty!!)")

                .Must(questionList => questionList.All(q => q.QuestionId > 0))
                .WithMessage("invalid question id (non zero)")

                .Must(q => q.All(q => q.Question.Content != null && q.Question.Content != ""))
                .WithMessage("content must not be empty or null!")

                .Must(q => q.All(q => q.Question.OptionA != null && q.Question.OptionA != ""))
                .WithMessage("Option A must not be empty or null!")

                .Must(q => q.All(q => q.Question.OptionB != null && q.Question.OptionB != ""))
                .WithMessage("Option B must not be empty or null!")

                .Must(q => q.All(q => q.Question.OptionC != null && q.Question.OptionC != ""))
                .WithMessage("Option C must not be empty or null!")

                .Must(q => q.All(q => q.Question.OptionD != null && q.Question.OptionD != ""))
                .WithMessage("Option D must not be empty or null!")

                .Must(q => q.All(q => q.Question.CorrectAnswer != null))
                .WithMessage("correct answer must not be empty or null!");


        }
    }
}
