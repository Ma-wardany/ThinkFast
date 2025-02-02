using FluentValidation;
using OnlineExam.Core.Features.Answers.Commands.Models;

namespace OnlineExam.Core.Features.Answers.Commands.Validators
{
    public class SubmitAnswersCommandValidator : AbstractValidator<SubmitAnswersCommand>
    {
        public SubmitAnswersCommandValidator()
        {
            RuleFor(command => command.StudentId)
                .NotEmpty().WithMessage("StudentId is required.")
                .NotNull().WithMessage("StudentId cannot be null.")
                .MaximumLength(50).WithMessage("StudentId cannot exceed 50 characters.");

            RuleFor(command => command.ExamId)
                .GreaterThan(0).WithMessage("ExamId must be greater than zero.");

            RuleFor(command => command.Answers)
                .NotEmpty().WithMessage("Answers are required.")
                .Must(answers => answers != null && answers.Count > 0).WithMessage("Answers list cannot be empty.")
                .ForEach(answer =>
                {
                    answer.NotNull().WithMessage("Answer item cannot be null.");
                    answer.SetValidator(new InlineValidator<AnswerItem>
                    {
                        v => v.RuleFor(a => a.QuestionId)
                              .GreaterThan(0).WithMessage("QuestionId must be greater than zero."),

                        v => v.RuleFor(a => a.Selected)
                              .Must(selected => "ABCD".Contains(selected)).WithMessage("Selected answer must be one of 'A', 'B', 'C', or 'D'.")
                    });
                });
        }
    }
}
