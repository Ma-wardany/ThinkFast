using FluentValidation;
using OnlineExam.Core.Features.ApplicationUser.Commands.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExam.Core.Features.ApplicationUser.Commands.Validators
{
    public class RegisterStudentCommandValidator : AbstractValidator<RegisterStudentCommand>
    {
        public RegisterStudentCommandValidator()
        {

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("email must not be empty")
                .NotNull().WithMessage("email is required");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("username must not be empty")
                .NotNull().WithMessage("username is required");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("password must not be empty")
                .NotNull().WithMessage("password is required");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage("not matched password");

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("name must not be empty")
                .NotNull().WithMessage("name is required");

            RuleFor(x => x.SchoolYear)
                .GreaterThan(0).WithMessage("invalid school year")
                .NotNull().WithMessage("school year is required");
        }
    }
}
