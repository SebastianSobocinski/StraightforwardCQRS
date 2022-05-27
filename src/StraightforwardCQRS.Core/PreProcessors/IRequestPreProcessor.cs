namespace StraightforwardCQRS.Core.PreProcessors;

public interface IRequestPreProcessor<in TRequest> where TRequest : class, IRequest
{
    Task ProcessAsync(TRequest request, CancellationToken cancellationToken = default);
}