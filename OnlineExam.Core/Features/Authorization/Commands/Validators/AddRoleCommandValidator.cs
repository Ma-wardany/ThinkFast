using FluentValidation;
using OnlineExam.Core.Features.Authorization.Commands.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExam.Core.Features.Authorization.Commands.Validators
{
    public class AddRoleCommandValidator : AbstractValidator<AddRoleCommand>
    {
        public AddRoleCommandValidator()
        {
            RuleFor(x => x.RoleName)
                .NotEmpty().WithMessage("role name must not be empty!")
                .NotNull().WithMessage("role name is required!");
        }
    }
}
