using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExam.Core.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> :
                 IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {

        private readonly IEnumerable<IValidator<TRequest>> validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            this.validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // get context in which validators will be applied
            if (validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                // validation result is array of validationResult object
                // (each vr object contain 2 props --> list of errors, bool isValid)
                var validationResults = await Task.WhenAll(validators.Select(vr => vr.ValidateAsync(context, cancellationToken)));

                if (validationResults.Any())
                {
                    var errors = validationResults.SelectMany(vr => vr.Errors).Where(e => e != null).ToList();

                    if (errors.Any())
                    {
                        var ErrorMessages = string.Join("; ", errors.Select(e => e.ErrorMessage));

                        throw new ValidationException(ErrorMessages.ToString());

                    }
                }
            }

            return await next();
        }
    }
}
