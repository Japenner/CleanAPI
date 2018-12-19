namespace Clean.Web.Application.Behaviors
{
    using FluentValidation;
    using MediatR;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Mediatr behavior to wrap commands with validation automatically.
    /// </summary>
    /// <typeparam name="TRequest">The request type.</typeparam>
    /// <typeparam name="TResponse">The response type.</typeparam>
    public class ValidatorBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IValidator<TRequest>[] validators;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidatorBehavior{TRequest, TResponse}"/> class to validate the TRequest object.
        /// </summary>
        /// <param name="validators">An array of validators for type TRequest.</param>
        /// <param name="serviceProvider">Service providers</param>
        public ValidatorBehavior(IValidator<TRequest>[] validators, IServiceProvider serviceProvider)
        {
            var activeValidators = new List<IValidator<TRequest>>(validators);
            var requestType = typeof(TRequest);
            foreach (var interfaceType in requestType.GetInterfaces())
            {
                var validator = (IValidator<TRequest>)serviceProvider.GetService(typeof(IValidator<>).MakeGenericType(interfaceType));
                if (validator != null)
                {
                    activeValidators.Add(validator);
                }
            }

            this.validators = activeValidators.ToArray();
        }

        /// <summary>
        /// Runs the validations and executes the next handler.
        /// </summary>
        /// <param name="request">The request object.</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <param name="next">The next delegate in the pipeline.</param>
        /// <returns>Returns the response object of type TResponse.</returns>
        /// <exception cref="ValidationException">Throws an exception when one or more validators returns an error.</exception>
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var failures = validators
                .Select(v => v.Validate(request))
                .SelectMany(result => result.Errors)
                .Where(error => error != null)
                .ToList();

            if (failures.Any())
            {
                throw new ValidationException("Validation exception", failures);
            }

            var response = await next();
            return response;
        }
    }
}
