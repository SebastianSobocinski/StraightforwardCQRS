using System.Reflection;
using Autofac;
using StraightforwardCQRS.Core.Commands;
using StraightforwardCQRS.Core.Events;
using StraightforwardCQRS.Core.PostProcessors;
using StraightforwardCQRS.Core.PreProcessors;
using StraightforwardCQRS.Core.Queries;

namespace StraightforwardCQRS.Core.Autofac;

public static class Extensions
{
    public static ContainerBuilder AddCqrs(this ContainerBuilder builder, params Assembly[] assemblies)
    {
        builder.RegisterEvents(assemblies);
        builder.RegisterCommands(assemblies);
        builder.RegisterQueries(assemblies);
        return builder;
    }

    public static ContainerBuilder AddRequestPreProcessor(this ContainerBuilder builder, Type type)
    {
        builder.RegisterGeneric(type)
            .As(typeof(IRequestPreProcessor<>));

        return builder;
    }

    public static ContainerBuilder AddRequestPostProcessor(this ContainerBuilder builder, Type type)
    {
        builder.RegisterGeneric(type)
            .As(typeof(IRequestPostProcessor<>));

        return builder;
    }

    public static ContainerBuilder AddCommandHandlerDecorator(this ContainerBuilder builder, Type type)
    {
        builder.RegisterGenericDecorator(type, typeof(ICommandHandler<>));
        return builder;
    }
    
    public static ContainerBuilder AddQueryHandlerDecorator(this ContainerBuilder builder, Type type)
    {
        builder.RegisterGenericDecorator(type, typeof(IQueryHandler<,>));
        return builder;
    }
    
    public static ContainerBuilder AddEventHandlerDecorator(this ContainerBuilder builder, Type type)
    {
        builder.RegisterGenericDecorator(type, typeof(IEventHandler<>));
        return builder;
    }

    private static void RegisterEvents(this ContainerBuilder builder, IEnumerable<Assembly> assemblies)
    {
        builder.RegisterType<EventBus>().As<IEventBus>().InstancePerLifetimeScope();
        foreach (var assembly in assemblies)
        {
            builder.RegisterAssemblyTypes(assembly)
                .AsClosedTypesOf(typeof(IEventHandler<>)).InstancePerLifetimeScope();   
        }
    }
    
    private static void RegisterCommands(this ContainerBuilder builder, IEnumerable<Assembly> assemblies)
    {
        builder.RegisterType<CommandBus>().As<ICommandBus>().InstancePerLifetimeScope();
        foreach (var assembly in assemblies)
        {
            builder.RegisterAssemblyTypes(assembly)
                .AsClosedTypesOf(typeof(ICommandHandler<>)).InstancePerLifetimeScope();   
        }
    }

    private static void RegisterQueries(this ContainerBuilder builder,  IEnumerable<Assembly> assemblies)
    {
        builder.RegisterType<QueryBus>().As<IQueryBus>().InstancePerLifetimeScope();
        foreach (var assembly in assemblies)
        {
            builder.RegisterAssemblyTypes(assembly)
                .AsClosedTypesOf(typeof(IQueryHandler<,>)).InstancePerLifetimeScope();   
        }
    }
}