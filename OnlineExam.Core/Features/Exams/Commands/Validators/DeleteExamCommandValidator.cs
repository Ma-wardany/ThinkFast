using FluentValidation;
using OnlineExam.Core.Features.Exams.Commands.Models;

namespace OnlineExam.Core.Features.Exams.Commands.Validators
{
    public class DeleteExamCommandValidator : AbstractValidator<DeleteExamCommand>
    {
        public DeleteExamCommandValidator()
        {
            RuleFor(x => x.ExamId)
                .GreaterThan(0).WithMessage("invalid exam id");

            RuleFor(x => x.InstructorId)
                    .NotNull().WithMessage("instructor id is required")
                    .NotEmpty().WithMessage("instructor id must not be empty");
        }
    }
}
