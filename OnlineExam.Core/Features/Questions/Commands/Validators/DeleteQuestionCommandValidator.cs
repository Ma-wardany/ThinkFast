
using FluentValidation;
using OnlineExam.Core.Features.Questions.Commands.Models;

namespace OnlineExam.Core.Features.Questions.Commands.Validators
{
    public class DeleteQuestionCommandValidator : AbstractValidator<DeleteQuestionCommand>
    {
        public DeleteQuestionCommandValidator()
        {
            RuleFor(x => x.InstructorId)
                .NotNull().WithMessage("instructor id is required!")
                .NotEmpty().WithMessage("instructor id must not be empty!");

            RuleFor(x => x.ExamId)
                .GreaterThan(0).WithMessage("invalid exam id (must greater than 0)");

            RuleFor(x => x.QuestionId)
                .GreaterThan(0).WithMessage("invalid question id (must greater than 0)");
        }
    }
}
