using StraightforwardCQRS.Core.Queries;

namespace StraightforwardCQRS.Core.PreProcessors;

public interface IQueryPreProcessor<in TQuery> : IRequestPreProcessor<TQuery> where TQuery : class, IQuery
{
}