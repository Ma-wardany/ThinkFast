
using FluentValidation;
using OnlineExam.Core.Features.Questions.Commands.Models;

namespace OnlineExam.Core.Features.Questions.Commands.Validators
{
    public class AddQuestionsRangeCommandValidator : AbstractValidator<AddQuestionsRangeCommand>
    {
        public AddQuestionsRangeCommandValidator()
        {
            RuleFor(x => x.InstructorId)
                .NotEmpty().WithMessage("instructor id must not be empty!")
                .NotNull().WithMessage("instructor id is required!");


            RuleFor(x => x.ExamId)
                .GreaterThan(0).WithMessage("invalid exam id (non zero)");

            RuleFor(x => x.QuestionList)
                .NotEmpty().WithMessage("no questions (empty!!)")

                .Must(q => q.All(q => q.Content != null && q.Content != ""))
                .WithMessage("content must not be empty or null!")

                .Must(q => q.All(q => q.OptionA != null && q.OptionA != ""))
                .WithMessage("Option A must not be empty or null!")

                .Must(q => q.All(q => q.OptionB != null && q.OptionB != ""))
                .WithMessage("Option B must not be empty or null!")

                .Must(q => q.All(q => q.OptionC != null && q.OptionC != ""))
                .WithMessage("Option C must not be empty or null!")

                .Must(q => q.All(q => q.OptionD != null && q.OptionD != ""))
                .WithMessage("Option D must not be empty or null!")

                .Must(q => q.All(q => q.CorrectAnswer != null))
                .WithMessage("correct answer must not be empty or null!");
        }
    }
}

