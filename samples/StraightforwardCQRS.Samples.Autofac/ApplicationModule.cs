using Autofac;
using StraightforwardCQRS.Core.Autofac;
using StraightforwardCQRS.Core.Commands;
using StraightforwardCQRS.Samples.Common;
using StraightforwardCQRS.Samples.Common.Decorators;
using StraightforwardCQRS.Samples.Common.PostProcessors;
using StraightforwardCQRS.Samples.Common.PreProcessors;
using StraightforwardCQRS.Samples.Common.Queries.GetUser;

namespace StraightforwardCQRS.Samples.Autofac;

public class ApplicationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<UserManager>().As<IUserManager>().SingleInstance();
        builder.AddCqrs(ThisAssembly, typeof(GetUser).Assembly);
        
        builder.AddRequestPreProcessor(typeof(LoggingPreProcessor<>));
        builder.AddRequestPreProcessor(typeof(LoggingCommandPreProcessor<>));
        builder.AddRequestPreProcessor(typeof(LoggingQueryPreProcessor<>));
        builder.AddRequestPreProcessor(typeof(ValidationPreProcessor<>));
        
        builder.AddRequestPostProcessor(typeof(LoggingQueryPostProcessor<>));
        builder.AddRequestPostProcessor(typeof(LoggingPostProcessor<>));
        
        builder.AddCommandHandlerDecorator(typeof(UnitOfWorkCommandDecorator<>));
        builder.AddQueryHandlerDecorator(typeof(CachingQueryDecorator<,>));
    }
}