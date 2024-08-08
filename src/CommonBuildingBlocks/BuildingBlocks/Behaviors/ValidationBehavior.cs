using BuildingBlocks.CQRS;
using FluentValidation;
using MediatR;


namespace BuildingBlocks.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : ICommand<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators ?? throw new ArgumentNullException(nameof(validators)); //It ensures that the validators parameter is not null when the ValidationBehavior class is instantiated.
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                var validationResult = await Task.WhenAll(
                    _validators.Select(v => v.ValidateAsync(context, cancellationToken))
                    );

                var failures = validationResult
                    .Where(v => v.Errors.Any())
                    .SelectMany(r => r.Errors)
                    .ToList();

                if (failures.Any()) 
                {
                    var errorMessage = string.Join(Environment.NewLine, failures.Select(f => f.ErrorMessage));
                    throw new ValidationException(errorMessage);
                }                
            }
            return await next();
        }
    }
}
