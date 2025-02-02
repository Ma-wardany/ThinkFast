using FluentValidation;
using OnlineExam.Core.Features.Questions.Commands.Models;

namespace OnlineExam.Core.Features.Questions.Commands.Validators
{
    public class AddQuestionCommandValidator : AbstractValidator<AddQuestionCommand>
    {
        public AddQuestionCommandValidator()
        {
            RuleFor(x => x.Content)
                .NotNull().WithMessage("question content is required!")
                .NotEmpty().WithMessage("question content must not be empty!");

            RuleFor(x => x.OptionA)
                .NotNull().WithMessage("option A is required!")
                .NotEmpty().WithMessage("option A must not be empty!");

            RuleFor(x => x.OptionB)
                .NotNull().WithMessage("option B is required!")
                .NotEmpty().WithMessage("option B must not be empty!");

            RuleFor(x => x.OptionC)
                .NotNull().WithMessage("option C is required!")
                .NotEmpty().WithMessage("option C must not be empty!");

            RuleFor(x => x.OptionD)
                .NotNull().WithMessage("option D is required!")
                .NotEmpty().WithMessage("option D must not be empty!");

            RuleFor(x => x.CorrectAnswer)
                .NotNull().WithMessage("correct answer is required!")
                .NotEmpty().WithMessage("correct answer must not be empty!");

            RuleFor(x => x.InstructorId)
                .NotNull().WithMessage("instructor id is required!")
                .NotEmpty().WithMessage("instructor id must not be empty!");

            RuleFor(x => x.ExamId)
                .GreaterThan(0).WithMessage("invalid exam id (must greater than 0)");
        }
    }
}
