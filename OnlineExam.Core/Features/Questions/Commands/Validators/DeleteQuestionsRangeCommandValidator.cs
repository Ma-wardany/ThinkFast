using FluentValidation;
using MediatR;
using OnlineExam.Core.Features.Questions.Commands.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExam.Core.Features.Questions.Commands.Validators
{
    public class DeleteQuestionsRangeCommandValidator : AbstractValidator<DeleteQuestionsRangeCommand>
    {
        public DeleteQuestionsRangeCommandValidator()
        {
            RuleFor(x => x.InstructorId)
                .NotNull().WithMessage("instructor id is required!")
                .NotEmpty().WithMessage("instructor id must not be empty!");

            RuleFor(x => x.ExamId)
                .GreaterThan(0).WithMessage("invalid exam id (must greater than 0)");

            RuleFor(x => x.QuestionIDs)
                .NotEmpty().WithMessage("there are no questios IDs!")
                .Must(x => x.All(q => q > 0)).WithMessage("invalid question id (non zero)");
        }
    }
}
