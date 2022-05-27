using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using StraightforwardCQRS.Core.Commands;
using StraightforwardCQRS.Core.Queries;
using StraightforwardCQRS.Samples.Autofac;
using StraightforwardCQRS.Samples.Common.Commands.CreateUser;
using StraightforwardCQRS.Samples.Common.Commands.UpdateUser;
using StraightforwardCQRS.Samples.Common.Dtos;
using StraightforwardCQRS.Samples.Common.Queries.GetUser;
using StraightforwardCQRS.Samples.Common.Queries.GetUsers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(container =>
{
    container.RegisterModule(new ApplicationModule());
});

builder.Services.AddControllers()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateUserValidator>());

var app = builder.Build();

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

app.Run();