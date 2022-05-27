using StraightforwardCQRS.Core.PostProcessors;
using StraightforwardCQRS.Core.PreProcessors;

namespace StraightforwardCQRS.Core;

internal sealed class RequestPipelineProcessor<TRequest> where TRequest : class, IRequest
{
    private readonly IEnumerable<IRequestPreProcessor<TRequest>> _preProcessors;
    private readonly IEnumerable<IRequestPostProcessor<TRequest>> _postProcessors;

    public RequestPipelineProcessor(IEnumerable<IRequestPreProcessor<TRequest>> preProcessors,
        IEnumerable<IRequestPostProcessor<TRequest>> postProcessors)
    {
        _preProcessors = preProcessors;
        _postProcessors = postProcessors;
    }

    public async Task PreProcessAsync(TRequest request, CancellationToken cancellationToken)
    {
        foreach (var processor in _preProcessors)
        {
            await processor.ProcessAsync(request, cancellationToken);
        }
    }
    
    public async Task PostProcessAsync(TRequest request, CancellationToken cancellationToken)
    {
        foreach (var processor in _postProcessors)
        {
            await processor.ProcessAsync(request, cancellationToken);
        }
    }
}