using FluentValidation;
using OnlineExam.Core.Features.Exams.Commands.Models;

namespace OnlineExam.Core.Features.Exams.Commands.Validators
{
    public class UpdateExamCommandValidator : AbstractValidator<UpdateExamCommand>
    {
        public UpdateExamCommandValidator()
        {

            RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage("invalid school year");

            RuleFor(x => x.ExamName)
                .NotNull().WithMessage("exam name is required")
                .NotEmpty().WithMessage("exam name must not be empty")
                .MaximumLength(30);


            RuleFor(x => x.ExamCode)
                    .NotNull().WithMessage("exam code is required")
                    .NotEmpty().WithMessage("exam code must not be empty")
                    .MaximumLength(30);

            RuleFor(x => x.SchoolYear)
                    .GreaterThan(0).WithMessage("invalid school year");

            RuleFor(x => x.SubjectId)
                    .GreaterThan(0).WithMessage("invalid subject id");
        }
    }
}
