using Autofac;
using Autofac.Extensions.DependencyInjection;
using StraightforwardCQRS.Samples.Autofac;
using StraightforwardCQRS.Samples.Common;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(container =>
{
    container.RegisterModule(new ApplicationModule());
});

builder.Services.AddCommon();

var app = builder.Build();
app.UseCommon();
app.Run();