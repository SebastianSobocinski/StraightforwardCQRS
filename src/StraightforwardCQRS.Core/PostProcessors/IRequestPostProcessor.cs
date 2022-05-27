namespace StraightforwardCQRS.Core.PostProcessors;

public interface IRequestPostProcessor<in TRequest> where TRequest : class, IRequest
{
    Task ProcessAsync(TRequest request, CancellationToken cancellationToken = default);
}