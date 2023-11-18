using Application.Abstractions.Messaging;
using FluentValidation;
using Domain.Shared;
using MediatR;

namespace Application.Behaviors;

public class ValidationPipelineBehavior<TRequst, TResponce> : IPipelineBehavior<TRequst, TResponce>
    where TRequst : ICommandBase
    where TResponce : Result
{
    private readonly IEnumerable<IValidator<TRequst>> _validators;

    public ValidationPipelineBehavior(IEnumerable<IValidator<TRequst>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponce> Handle(
        TRequst request,
        RequestHandlerDelegate<TResponce> next,
        CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequst>(request);

        var validationFailures = await Task.WhenAll(
            _validators.Select(validator => validator.ValidateAsync(context, cancellationToken)));

        var errors = validationFailures
            .Where(validationResult => !validationResult.IsValid)
            .SelectMany(validationResult => validationResult.Errors)
            .Select(failure => new ValidationError(
                failure.PropertyName,
                failure.ErrorMessage))
            .ToList();

        if (errors.Any())
        {
            throw new ValidationException(errors);
        }

        return await next();
    }
}