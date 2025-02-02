using FluentValidation;
using OnlineExam.Core.Features.ApplicationUser.Commands.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExam.Core.Features.ApplicationUser.Commands.Validators
{
    public class RegisterInstructorCommandValidator : AbstractValidator<RegisterInstructorCommand>
    {
        public RegisterInstructorCommandValidator()
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

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("first name must not be empty")
                .NotNull().WithMessage("first name is required");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("last name must not be empty")
                .NotNull().WithMessage("last name is required");
        }
    }
}
