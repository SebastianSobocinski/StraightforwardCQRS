using Microsoft.Extensions.Logging;
using StraightforwardCQRS.Core.Commands;
using StraightforwardCQRS.Core.PreProcessors;

namespace StraightforwardCQRS.Samples.Common.PreProcessors;

public sealed class LoggingCommandPreProcessor<TCommand> : ICommandPreProcessor<TCommand> where TCommand : class, ICommand
{
    private readonly ILogger<LoggingCommandPreProcessor<TCommand>> _logger;

    public LoggingCommandPreProcessor(ILogger<LoggingCommandPreProcessor<TCommand>> logger)
    {
        _logger = logger;
    }

    public Task ProcessAsync(TCommand command, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        _logger.LogInformation($"Started command: {command.GetType().Name}, at: {now:HH:mm:ss.fff}");
        return Task.CompletedTask;
    }
}