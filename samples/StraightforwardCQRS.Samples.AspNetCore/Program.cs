using StraightforwardCQRS.Core.AspNetCore;
using StraightforwardCQRS.Samples.Common;
using StraightforwardCQRS.Samples.Common.Decorators;
using StraightforwardCQRS.Samples.Common.PostProcessors;
using StraightforwardCQRS.Samples.Common.PreProcessors;
using StraightforwardCQRS.Samples.Common.Queries.GetUser;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddEventHandlerDecorator(typeof(CounterEventDecorator<>));

builder.Services.AddCommon();

var app = builder.Build();
app.UseCommon();
app.Run();