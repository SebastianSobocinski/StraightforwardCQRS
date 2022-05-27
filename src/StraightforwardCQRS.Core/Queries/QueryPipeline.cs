using StraightforwardCQRS.Core.PostProcessors;
using StraightforwardCQRS.Core.PreProcessors;

namespace StraightforwardCQRS.Core.Queries;

public class QueryPipeline<TQuery, TResult> where TQuery : class, IQuery<TResult>
{
    private readonly RequestPipelineProcessor<TQuery> _pipelineProcessor;
    private readonly Func<Task<TResult>> _handler;

    public QueryPipeline(IEnumerable<IRequestPreProcessor<TQuery>> preProcessors, 
        IEnumerable<IRequestPostProcessor<TQuery>> postProcessors,
        Func<Task<TResult>> handler)
    {
        _pipelineProcessor = new RequestPipelineProcessor<TQuery>(preProcessors, postProcessors);
        _handler = handler;
    }
    
    public async Task<TResult> ProcessAsync(TQuery query, CancellationToken cancellationToken = default)
    {
        await _pipelineProcessor.PreProcessAsync(query, cancellationToken);
        var result = await _handler.Invoke();
        await _pipelineProcessor.PostProcessAsync(query, cancellationToken);
        return result;
    }
}