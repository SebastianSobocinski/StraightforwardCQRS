using StraightforwardCQRS.Core.Queries;

namespace StraightforwardCQRS.Core.PostProcessors;

public interface IQueryPostProcessor<in TQuery> : IRequestPostProcessor<TQuery> where TQuery : class, IQuery
{
}