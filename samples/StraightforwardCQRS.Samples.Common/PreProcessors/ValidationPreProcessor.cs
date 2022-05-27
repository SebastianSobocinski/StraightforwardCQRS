using FluentValidation;
using Microsoft.Extensions.Logging;
using StraightforwardCQRS.Core;
using StraightforwardCQRS.Core.PreProcessors;

namespace StraightforwardCQRS.Samples.Common.PreProcessors;

public class ValidationPreProcessor<TRequest> : IRequestPreProcessor<TRequest> 
    where TRequest : class, IRequest
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly ILogger<ValidationPreProcessor<TRequest>> _logger;

    public ValidationPreProcessor(IEnumerable<IValidator<TRequest>> validators, ILogger<ValidationPreProcessor<TRequest>> logger)
    {
        _validators = validators;
        _logger = logger;
    }

    public async Task ProcessAsync(TRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation($"Validating request. Found [{_validators.Count()}] validators");

        var validationTasks = _validators.Select(x => x.ValidateAsync(request, cancellationToken));
        var validationResults = await Task.WhenAll(validationTasks);
        var isValid = validationResults.All(x => x.IsValid);
        if (isValid)
        {
            _logger.LogInformation("Validation passed successfully");
            return;
        }

        var failedValidators = validationResults.Where(x => !x.IsValid);
        var message = string.Join(", ", failedValidators.SelectMany(x => x.Errors.Select(y => y.ErrorMessage)));
        throw new InvalidDataException(message);
    }
}