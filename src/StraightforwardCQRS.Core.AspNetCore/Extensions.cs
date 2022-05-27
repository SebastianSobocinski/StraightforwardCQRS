using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using StraightforwardCQRS.Core.Commands;
using StraightforwardCQRS.Core.Events;
using StraightforwardCQRS.Core.PostProcessors;
using StraightforwardCQRS.Core.PreProcessors;
using StraightforwardCQRS.Core.Queries;

namespace StraightforwardCQRS.AspNetCore.DependencyInjection;

public static class Extensions
{
    public static IServiceCollection AddCqrs(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.RegisterEvents(assemblies);
        services.RegisterCommands(assemblies);
        services.RegisterQueries(assemblies);
        return services;
    }

    public static IServiceCollection AddRequestPreProcessor(this IServiceCollection services, Type type)
    {
        services.AddScoped(typeof(IRequestPreProcessor<>), type);
        return services;
    }
    
    public static IServiceCollection AddRequestPostProcessor(this IServiceCollection services, Type type)
    {
        services.AddScoped(typeof(IRequestPostProcessor<>), type);
        return services;
    }

    public static IServiceCollection AddCommandHandlerDecorator(this IServiceCollection services, Type type)
    {
        services.Decorate(typeof(ICommandHandler<>), type);
        return services;
    }
    
    public static IServiceCollection AddQueryHandlerDecorator(this IServiceCollection services, Type type)
    {
        services.Decorate(typeof(IQueryHandler<,>),type);
        return services;
    }
    
    public static IServiceCollection AddEventHandlerDecorator(this IServiceCollection services, Type type)
    {
        services.Decorate(typeof(IEventHandler<>), type);
        return services;
    }
    
    private static void RegisterEvents(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        services.AddScoped<IEventBus, EventBus>();

        services.Scan(s => s.FromAssemblies(assemblies)
            .AddClasses(x => x.AssignableTo(typeof(IEventHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime()
        );
    }
    
    private static void RegisterCommands(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        services.AddScoped<ICommandBus, CommandBus>();

        services.Scan(s => s.FromAssemblies(assemblies)
            .AddClasses(x => x.AssignableTo(typeof(ICommandHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime()
        );
    }
    
    private static void RegisterQueries(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        services.AddScoped<IQueryBus, QueryBus>();

        services.Scan(s => s.FromAssemblies(assemblies)
            .AddClasses(x => x.AssignableTo(typeof(IQueryHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime()
        );
    }
}