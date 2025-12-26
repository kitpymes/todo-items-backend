using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using TodoItems.Application.TodoList.Mappings;
using TodoItems.Application.TodoList.UseCases.Commands;
using TodoItems.Application.TodoList.UseCases.Queries;
using TodoItems.Domain.Aggregates.TodoListAggregate;
using TodoItems.Domain.Aggregates.TodoListAggregate.Interfaces;
using TodoItems.Infrastructure.Extensions;
using TodoItems.Infrastructure.Middlewares;
using TodoItems.Infrastructure.Persistence;
using TodoItems.Infrastructure.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args); 

builder.Services.AddControllers(); 

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true; 
    options.LowercaseQueryStrings = true;
});

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.SerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
    options.SerializerOptions.PropertyNameCaseInsensitive = true;
});

builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(ItemMapping).Assembly);

builder.Services.AddFluentValidation(typeof(AddItemCommandValidator).Assembly);

builder.Services.AddMediatR(config => {
    config.RegisterServicesFromAssembly(typeof(AddItemCommandHandler).Assembly);
    //config.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

builder.Services.AddScoped<ITodoListRepository, TodoListRepository>();
builder.Services.AddScoped<ITodoList, TodoList>();

var connection = new Microsoft.Data.Sqlite.SqliteConnection("DataSource=:memory:");
connection.Open();

builder.Services.AddDbContext<TodoListDbContext>(options =>
    options.UseSqlite(connection)
    .LogTo(Console.WriteLine, LogLevel.Information));

//builder.Services.AddDbContext<TodoListDbContext>(options => options.UseInMemoryDatabase("ItemsDb"));

var app = builder.Build(); 
if (app.Environment.IsDevelopment()) { app.UseSwagger(); app.UseSwaggerUI(); }
app.UseMiddleware<AppValidationsMiddleware>();
app.MapControllers();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<TodoListDbContext>();

        await context.Database.EnsureCreatedAsync();

        await TodoDataSeeder.SeedAsync(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocurrió un error al agregar datos en la base de datos.");
    }
}

app.Run();

public partial class Program { }