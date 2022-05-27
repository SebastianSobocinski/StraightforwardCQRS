using FluentValidation.AspNetCore;
using StraightforwardCQRS.Core.AspNetCore;
using StraightforwardCQRS.Core.Commands;
using StraightforwardCQRS.Core.Queries;
using StraightforwardCQRS.Samples.Common;
using StraightforwardCQRS.Samples.Common.Commands.CreateUser;
using StraightforwardCQRS.Samples.Common.Commands.UpdateUser;
using StraightforwardCQRS.Samples.Common.Decorators;
using StraightforwardCQRS.Samples.Common.Dtos;
using StraightforwardCQRS.Samples.Common.PostProcessors;
using StraightforwardCQRS.Samples.Common.PreProcessors;
using StraightforwardCQRS.Samples.Common.Queries.GetUser;
using StraightforwardCQRS.Samples.Common.Queries.GetUsers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IUserManager, UserManager>();
builder.Services.AddCqrs(typeof(Program).Assembly, typeof(GetUser).Assembly);

builder.Services.AddRequestPreProcessor(typeof(LoggingPreProcessor<>));
builder.Services.AddRequestPreProcessor(typeof(LoggingCommandPreProcessor<>));
builder.Services.AddRequestPreProcessor(typeof(LoggingQueryPreProcessor<>));
builder.Services.AddRequestPreProcessor(typeof(ValidationPreProcessor<>));

builder.Services.AddRequestPostProcessor(typeof(LoggingQueryPostProcessor<>));
builder.Services.AddRequestPostProcessor(typeof(LoggingPostProcessor<>));

builder.Services.AddCommandHandlerDecorator(typeof(UnitOfWorkCommandDecorator<>));
builder.Services.AddQueryHandlerDecorator(typeof(CachingQueryDecorator<,>));

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