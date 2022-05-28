using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using StraightforwardCQRS.Core.Commands;
using StraightforwardCQRS.Core.Queries;
using StraightforwardCQRS.Samples.Common.Commands.CreateUser;
using StraightforwardCQRS.Samples.Common.Commands.UpdateUser;
using StraightforwardCQRS.Samples.Common.Dtos;
using StraightforwardCQRS.Samples.Common.Queries.GetUser;
using StraightforwardCQRS.Samples.Common.Queries.GetUsers;

namespace StraightforwardCQRS.Samples.Common;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddCommon(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddControllers()
           .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateUserValidator>());
        return services;
    }

    public static IApplicationBuilder UseCommon(this WebApplication app)
    {
        app.UseSwagger(); 
        app.MapGet("/users/{id}", async (IQueryBus queryBus, Guid id) =>
            await queryBus.QueryAsync(new GetUser(id))
        );

        app.MapGet("/users", async (IQueryBus queryBus) =>
            await queryBus.QueryAsync(new GetUsers())
        );

        app.MapPost("/users", async (ICommandBus commandBus, NewUserDto user) =>
        {
            var id = Guid.NewGuid();
            await commandBus.DispatchAsync(new CreateUser(id, user));
            return id;
        });

        app.MapPut("/users/{id}", async (ICommandBus commandBus, Guid id, NewUserDto user) =>
        {
            await commandBus.DispatchAsync(new UpdateUser(id, user));
        });
        
        app.UseSwaggerUI();
        return app;
    }
}