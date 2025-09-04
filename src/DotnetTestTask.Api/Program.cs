using DotnetTestTask.Application;
using DotnetTestTask.Infrastructure;
using SharedKernel.Core.Persistence;

var builder = WebApplication.CreateBuilder(args);

IServiceCollection services = builder.Services;


services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var databaseInitializer = scope.ServiceProvider.GetService<IDatabaseInitializer>();
    databaseInitializer?.InitializeWithTestDataAsync(recreateDatabase: true).Wait();
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.MapControllers();

await app.RunAsync();