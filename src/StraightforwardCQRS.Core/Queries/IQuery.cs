namespace StraightforwardCQRS.Core.Queries;

public interface IQuery : IRequest { }

public interface IQuery<T> : IQuery { }